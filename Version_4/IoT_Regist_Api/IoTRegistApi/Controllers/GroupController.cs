using IoTRegistApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IoTRegistApi.Controllers
{
    public class GroupController : ApiController
    {

        // GET api/Users/5
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            GroupModel model = new GroupModel();
            return Ok(model.GetbyId(id));
        }
    }
}
