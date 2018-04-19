using IoTRegistApi.Models;
using System;
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
    public class PhotoController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAllOrders()
        {
            PhotoModel model = new PhotoModel();
            return Ok(model.GetAll());
        }

        // GET api/Users/5
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            PhotoModel model = new PhotoModel();
            return Ok(model.GetbyId(id));
        }

        [HttpPost]
        public IHttpActionResult CreatePhoto([FromBody]PhotoModel.CreateModel photo)
        {
            string logApi = "[Post] " + Request.RequestUri.ToString();
            string logForm = "Form : " + JsonConvert.SerializeObject(photo);

            if (!ModelState.IsValid || User == null)
            {
                return Content(HttpStatusCode.BadRequest, "Invalid data.");
            }
            else
            {
                try
                {
                    PhotoModel model = new PhotoModel();
                    model.Create(photo);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, "Insert fail.");
                }
            }
        }
        }
}