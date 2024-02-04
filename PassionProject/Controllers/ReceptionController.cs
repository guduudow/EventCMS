using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PassionProject.Migrations;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    public class ReceptionController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ReceptionController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44300/api/Receptiondata/");
        }



        // GET: Reception/List
        public ActionResult List()
        {
            //objective: communicate with my Reception data api to retreive a list of Receptions
            //curl https://localhost:44300/api/Receptiondata/listReceptions

            
            string url = "listReceptions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ReceptionDto> Receptions = response.Content.ReadAsAsync<IEnumerable<ReceptionDto>>().Result;
            //Debug.WriteLine("Number of Receptions received ");
            //Debug.WriteLine(Receptions.Count());

            return View(Receptions);
        }

        // GET: Reception/Details/5
        public ActionResult View(int id)
        {
            //objective: communicate with my Reception data api to retreive one reception
            //curl https://localhost:44300/api/Receptiondata/findReception/{id}

            
            string url = "findreception/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            ReceptionDto selectedreception = response.Content.ReadAsAsync<ReceptionDto>().Result;
            //Debug.WriteLine("Chosen Reception: ");
            //Debug.WriteLine(selectedreception.ReceptionName);

            return View(selectedreception);
        }

        public ActionResult Errors()
        {
            return View();
        }

        // GET: Reception/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Reception/Create
        [HttpPost]
        public ActionResult Create(Reception reception)
        {
            Debug.WriteLine("The name of the inputted reception is ");
            Debug.WriteLine(reception.ReceptionName);

            string url = "addreception";

            
            string jsonpayload = jss.Serialize(reception);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            } else
            {
                return RedirectToAction("Errors");
            }

            
        }

        // GET: Reception/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "findreception/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReceptionDto selectedreception = response.Content.ReadAsAsync<ReceptionDto>().Result;
            return View(selectedreception);
        }

        // POST: Reception/Update/5
        [HttpPost]
        public ActionResult Update(int id, Reception reception)
        {
            string url = "updatereception/" + id;
            string jsonpayload = jss.Serialize(reception);
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

        // GET: Reception/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findreception/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReceptionDto selectedreception = response.Content.ReadAsAsync<ReceptionDto>().Result;
            return View(selectedreception);
        }

        // POST: Reception/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Reception reception)
        {
            string url = "deletereception/" + id;
            string jsonpayload = jss.Serialize(reception);
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
