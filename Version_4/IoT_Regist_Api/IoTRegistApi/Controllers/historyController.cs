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
    public class historyController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetAllOrders()
        {
            historyModel model = new historyModel();
            return Ok(model.GetAll());
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]historyModel.CreateModel history)
        {
            string logApi = "[Post] " + Request.RequestUri.ToString();
            string logForm = "Form : " + JsonConvert.SerializeObject(history);
            Debug.WriteLine(logForm);
            if (history == null)
            {
                return Content(HttpStatusCode.BadRequest, "Invalid data.");
            }
            else
            {
                try
                {
                    historyModel model = new historyModel();
                    model.Create(history);
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
