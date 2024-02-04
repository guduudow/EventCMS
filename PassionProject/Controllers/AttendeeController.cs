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
        private JavaScriptSerializer jss = new JavaScriptSerializer();

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


        public ActionResult Errors()
        {
            return View();
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

            
            string jsonpayload = jss.Serialize(attendee);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            }
            else
            {
                return RedirectToAction("Errors");
            }
        }

        // GET: Attendee/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "findattendee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AttendeeDto selectedattendee = response.Content.ReadAsAsync<AttendeeDto>().Result;
            return View(selectedattendee);
        }

        // POST: Attendee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Attendee attendee)
        {
            string url = "updateattendee/" + id;
            string jsonpayload = jss.Serialize(attendee);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            } else
            {
                return RedirectToAction("Errors");
            }
        }

        // GET: Attendee/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findattendee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AttendeeDto selectedattendee = response.Content.ReadAsAsync<AttendeeDto>().Result;
            return View(selectedattendee);
        }

        // POST: Attendee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Attendee attendee)
        {
            string url = "deleteattendee/" + id;
            string jsonpayload = jss.Serialize(attendee);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Errors");
            }
        }
    }
}
