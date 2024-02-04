using PassionProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using PassionProject.Models;
using PassionProject.Migrations;
using System.Web.Script.Serialization;

namespace PassionProject.Controllers
{
    public class AttendeeController : Controller
    {

        private static readonly HttpClient client;

        static AttendeeController() 
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/api/attendeedata/");
        }


        // GET: Attendee/List
        public ActionResult List()
        {
            //objective: communicate with my attendee data api to retrieve a list of attendees
            //curl https://localhost:44300/api/attendeedata/listattendees

            
            string url = "listattendees";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<AttendeeDto> attendees = response.Content.ReadAsAsync<IEnumerable<AttendeeDto>>().Result;

            //Debug.WriteLine("Number of attendees received ");
            //Debug.WriteLine(attendees.Count());

            return View(attendees);
        }

        // GET: Attendee/Details/5
        public ActionResult View(int id)
        {
            //objective: communicate with my attendee data api to retrieve one attendee
            //curl https://localhost:44300/api/attendeedata/findattendee/id

            
            string url = "findattendee/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            AttendeeDto selectedattendee = response.Content.ReadAsAsync<AttendeeDto>().Result;

            //Debug.WriteLine("Attendee received: ");
            //Debug.WriteLine(selectedattendee.FirstName);
            //Debug.WriteLine(selectedattendee.LastName);

            return View(selectedattendee);
        }

        // GET: Attendee/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Attendee/Create
        [HttpPost]
        public ActionResult Create(Attendee attendee)
        {
            Debug.WriteLine("The json payload is ");
            //Debug.WriteLine(attendee.FirstName + " " + attendee.LastName);
            //objective add a new attendee to the API
            string url = "addattendee";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(attendee);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            client.PostAsync(url, content);

            return RedirectToAction("List");
        }

        // GET: Attendee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Attendee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Attendee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Attendee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
