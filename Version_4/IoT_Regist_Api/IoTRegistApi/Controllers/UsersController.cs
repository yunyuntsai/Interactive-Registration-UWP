using IoTRegistApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace IoTRegistApi.Controllers
{
    public class UsersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAllOrders()
        {
            VisitorModel model = new VisitorModel();
            return Ok(model.GetAll());
        }

        // GET api/Users/5
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            VisitorModel model = new VisitorModel();
            return Ok(model.GetbyId(id));
        }


        [HttpPost]
        public IHttpActionResult CreateUser([FromBody]VisitorModel.CreateModel User)
        {
            string logApi = "[Post] " + Request.RequestUri.ToString();
            string logForm = "Form : " + JsonConvert.SerializeObject(User);

            if (!ModelState.IsValid || User == null)
            {
                return Content(HttpStatusCode.BadRequest, "Invalid data.");
            }
            else
            {
                try
                {
                    VisitorModel model = new VisitorModel();
                    model.Create(User);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, "Insert fail.");
                }
            }

        }

        [HttpPut]
        public IHttpActionResult Put(int ID, [FromBody]VisitorModel.UpdateModel upmodel)
        {
            if (!ModelState.IsValid || upmodel == null)
            {
                return Content(HttpStatusCode.BadRequest, "Invald Update data.");
            }
            else
            {
                try
                {
                    VisitorModel model = new VisitorModel();
                    Registration_dbEntities dbEntity = new Registration_dbEntities();
                    Visitors_Profile user = dbEntity.Visitors_Profile.Find(ID);

                    user.VisitTime = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                    user.Arrived = "Yes";                    
                    model.Update(ID, user, upmodel);
                    Debug.WriteLine("update success");

                    return Ok();
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, "Update fail.");
                }
            }

        }
    }
}
