using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
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
    }
}