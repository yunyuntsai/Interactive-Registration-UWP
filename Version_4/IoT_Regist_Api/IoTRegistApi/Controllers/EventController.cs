using IoTRegistApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IoTRegistApi.Controllers
{
    public class EventController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAllEvent()
        {
            EventModel model = new EventModel();
            return Ok(model.GetAll());
        }


        // GET api/Users/5
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            EventModel model = new EventModel();
            return Ok(model.GetbyId(id));
        }


        [HttpPost]
        public IHttpActionResult Create([FromBody]EventModel.CreateModel event1)
        {
            string logApi = "[Post] " + Request.RequestUri.ToString();
            string logForm = "Form : " + JsonConvert.SerializeObject(event1);

            if (!ModelState.IsValid || User == null)
            {
                return Content(HttpStatusCode.BadRequest, "Invalid data.");
            }
            else
            {
                try
                {
                    EventModel model = new EventModel();
                    model.Create(event1);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, "Insert fail.");
                }
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(Int32 ID)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Invald Delete data.");
            }
            else
            {
                try
                {
                    EventModel model = new EventModel();
                    Registration_dbEntities dbEntity = new Registration_dbEntities();
                    Event_Profile e = dbEntity.Event_Profile.Find(ID);
                    Debug.WriteLine("-------------" + e.EventName);
                    model.Delete(ID);
                    Debug.WriteLine("Delete success");

                    return Ok();
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, "Clear fail.");
                }
            }

        }

        [HttpDelete]
        public IHttpActionResult DeleteAll()
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Invald Delete data.");
            }
            else
            {
                try
                {
                    EventModel model = new EventModel();
                    model.DeleteAll();
                    Debug.WriteLine("Delete success");

                    return Ok();
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, "Clear fail.");
                }
            }

        }
    }
}
