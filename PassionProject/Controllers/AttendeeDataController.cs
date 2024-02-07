using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject.Migrations;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    public class AttendeeDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AttendeeData/ListAttendees
        [HttpGet]
        public IEnumerable<AttendeeDto> ListAttendees()
        {
            List<Attendee> Attendees = db.Attendees.ToList();
            List<AttendeeDto> AttendeeDtos = new List<AttendeeDto>();

            Attendees.ForEach(a => AttendeeDtos.Add(new AttendeeDto()
            {
                AttendeeID = a.AttendeeID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber
            }));

            return AttendeeDtos;
        }

        // GET: api/AttendeeData/FindAttendee/5
        [ResponseType(typeof(Attendee))]
        [HttpGet]
        public IHttpActionResult FindAttendee(int id)
        {
            Attendee Attendee = db.Attendees.Find(id);
            AttendeeDto AttendeeDto = new AttendeeDto()
            {
                AttendeeID = Attendee.AttendeeID,
                FirstName = Attendee.FirstName,
                LastName = Attendee.LastName,
                Email = Attendee.Email,
                PhoneNumber = Attendee.PhoneNumber
            };
            if (Attendee == null)
            {
                return NotFound();
            }

            return Ok(AttendeeDto);
        }

        // POST: api/AttendeeData/UpdateAttendee/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAttendee(int id, Attendee attendee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attendee.AttendeeID)
            {
                return BadRequest();
            }

            db.Entry(attendee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AttendeeData/AddAttendee
        [ResponseType(typeof(Attendee))]
        [HttpPost]
        public IHttpActionResult AddAttendee(Attendee attendee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Attendees.Add(attendee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = attendee.AttendeeID }, attendee);
        }

        // POST: api/AttendeeData/DeleteAttendee/5
        [ResponseType(typeof(Attendee))]
        [HttpPost]
        public IHttpActionResult DeleteAttendee(int id)
        {
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return NotFound();
            }

            db.Attendees.Remove(attendee);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttendeeExists(int id)
        {
            return db.Attendees.Count(e => e.AttendeeID == id) > 0;
        }

        ///<summary>
        ///Gathers information about attendees related to a specific event
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: Event has the names of the attendees going to the event
        /// </returns>
        /// <param name="id">Reception ID</param>
        /// <example>
        /// GET: api/AttendeeData/ListAttendeesForEvent/2
        /// </example>
        /// 
        [HttpGet]
        public IHttpActionResult ListAttendeesForReception(int id)
        {
            //all attendees that have events which match with our id
            List<Attendee> Attendees = db.Attendees.Where(
               a=>a.Receptions.Any(
                r=>r.ReceptionID==id
                )).ToList();
            List<AttendeeDto> AttendeeDtos = new List<AttendeeDto>();

            Attendees.ForEach(a => AttendeeDtos.Add(new AttendeeDto()
            {
                AttendeeID = a.AttendeeID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber
            }));

            return Ok(AttendeeDtos);
        }

        ///<summary>
        ///Gathers information about attendees related to a specific event
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: dropdown showing all the attendees that have not signed up for particular event
        /// </returns>
        /// <param name="id">Reception ID</param>
        /// <example>
        /// GET: api/AttendeeData/ListAttendeesNotSignedUpForReception/2
        /// </example>
        /// 
        [HttpGet]
        public IHttpActionResult ListAttendeesNotSignedUpForReception(int id)
        {
            //all attendees that have events which match with our id
            List<Attendee> Attendees = db.Attendees.Where(
               a => !a.Receptions.Any(
                r => r.ReceptionID == id
                )).ToList();
            List<AttendeeDto> AttendeeDtos = new List<AttendeeDto>();

            Attendees.ForEach(a => AttendeeDtos.Add(new AttendeeDto()
            {
                AttendeeID = a.AttendeeID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber
            }));

            return Ok(AttendeeDtos);
        }

        /// <summary>
        /// Adds a particular attendee with a particular reception
        /// </summary>
        /// <param name="attendeeid">The attendee ID primary key</param>
        /// <param name="receptionid">The reception ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AttendeeData/AddAttendeeToReception/9/1
        /// </example>
        [HttpPost]
        [Route("api/attendeedata/AddAttendeeToReception/{receptionid}/{attendeeid}")]
        public IHttpActionResult AddAttendeeToReception (int receptionid, int attendeeid)
        {
            Attendee SelectedAttendee = db.Attendees.Include(a => a.Receptions).Where(a => a.AttendeeID == attendeeid).FirstOrDefault();
            Reception SelectedReception = db.Receptions.Find(receptionid);

            if (SelectedAttendee == null || SelectedReception == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input attende id is: " + attendeeid);
            Debug.WriteLine("selected attendee name is: " + SelectedAttendee.FirstName + " " + SelectedAttendee.LastName);
            Debug.WriteLine("input reception id is: " + receptionid);
            Debug.WriteLine("selected reception name is: " + SelectedReception.ReceptionName);


            SelectedAttendee.Receptions.Add(SelectedReception);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes a particular attendee with a particular reception
        /// </summary>
        /// <param name="attendeeid">The attendee ID primary key</param>
        /// <param name="receptionid">The reception ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AttendeeData/RemoveAttendeeFromReception/9/1
        /// </example>
        [HttpPost]
        [Route("api/attendeedata/RemoveAttendeeFromReception/{receptionid}/{attendeeid}")]
        public IHttpActionResult RemoveAttendeeFromReception(int receptionid, int attendeeid)
        {
            Attendee SelectedAttendee = db.Attendees.Include(a => a.Receptions).Where(a => a.AttendeeID == attendeeid).FirstOrDefault();
            Reception SelectedReception = db.Receptions.Find(receptionid);

            if (SelectedAttendee == null || SelectedReception == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input attende id is: " + attendeeid);
            Debug.WriteLine("selected attendee name is: " + SelectedAttendee.FirstName + " " + SelectedAttendee.LastName);
            Debug.WriteLine("input reception id is: " + receptionid);
            Debug.WriteLine("selected reception name is: " + SelectedReception.ReceptionName);


            SelectedAttendee.Receptions.Remove(SelectedReception);
            db.SaveChanges();

            return Ok();
        }


    }
}