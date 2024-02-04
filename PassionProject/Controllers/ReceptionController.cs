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

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(reception);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            client.PostAsync(url, content);

            return RedirectToAction("List");
        }

        // GET: Reception/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reception/Edit/5
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

        // GET: Reception/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reception/Delete/5
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
