using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{

    public class UsersController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAllOrders()
        {
            UserModel model = new UserModel();
            return Ok(model.GetAll());
        }

        // GET api/Users/5
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            UserModel model = new UserModel();
            return Ok(model.GetbyId(id));
        }

        /*// GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public string Get(int id)
        {
            return "value";
        }*/

        [HttpPost]
        public IHttpActionResult CreateUser([FromBody]UserModel.CreateModel User)
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
                    UserModel model = new UserModel();
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
        public IHttpActionResult Put(int ID, [FromBody]UserModel.UpdateModel upmodel)
        {
            if (!ModelState.IsValid || upmodel == null)
            {
                return Content(HttpStatusCode.BadRequest, "Invald Update data.");
            }
            else
            {
                try
                {
                    UserModel model = new UserModel();
                    WebApplication2201802_dbEntities1 dbEntity = new WebApplication2201802_dbEntities1();
                    USERTABLE user = dbEntity.USERTABLE.Find(ID);
                    user.UpdateAt = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
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