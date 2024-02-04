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
    public class ReceptionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ReceptionData/ListReceptions
        [HttpGet]
        public IEnumerable<ReceptionDto> ListReceptions()
        {
            List<Reception> Receptions = db.Receptions.ToList();
            List<ReceptionDto> ReceptionDtos = new List<ReceptionDto>();

            Receptions.ForEach(e => ReceptionDtos.Add(new ReceptionDto()
            {
                ReceptionID = e.ReceptionID,
                ReceptionName = e.ReceptionName,
                ReceptionLocation = e.ReceptionLocation,
                ReceptionDate = e.ReceptionDate,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                ReceptionPrice = e.ReceptionPrice,
                ReceptionDescription = e.ReceptionDescription
            }));

            return ReceptionDtos;
        }

        // GET: api/ReceptionData/FindReception/5
        [ResponseType(typeof(Reception))]
        [HttpGet]
        public IHttpActionResult FindReception(int id)
        {
            Reception Reception = db.Receptions.Find(id);
            ReceptionDto ReceptionDto = new ReceptionDto()
            {
                ReceptionID = Reception.ReceptionID,
                ReceptionName = Reception.ReceptionName,
                ReceptionLocation = Reception.ReceptionLocation,
                ReceptionDate = Reception.ReceptionDate,
                StartTime = Reception.StartTime,
                EndTime = Reception.EndTime,
                ReceptionPrice = Reception.ReceptionPrice,
                ReceptionDescription = Reception.ReceptionDescription
            };
            if (Reception == null)
            {
                return NotFound();
            }

            return Ok(ReceptionDto);
        }

        // POST: api/ReceptionData/UpdateReception/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateReception(int id, Reception Reception)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Reception.ReceptionID)
            {
                return BadRequest();
            }

            db.Entry(Reception).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceptionExists(id))
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

        // POST: api/ReceptionData/AddReception
        [ResponseType(typeof(Reception))]
        [HttpPost]
        public IHttpActionResult AddReception(Reception Reception)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Receptions.Add(Reception);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Reception.ReceptionID }, Reception);
        }

        // POST: api/ReceptionData/DeleteReception/5
        [ResponseType(typeof(Reception))]
        [HttpPost]
        public IHttpActionResult DeleteReception(int id)
        {
            Reception Reception = db.Receptions.Find(id);
            if (Reception == null)
            {
                return NotFound();
            }

            db.Receptions.Remove(Reception);
            db.SaveChanges();

            return Ok(Reception);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReceptionExists(int id)
        {
            return db.Receptions.Count(e => e.ReceptionID == id) > 0;
        }
    }
}