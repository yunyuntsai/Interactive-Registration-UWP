using IoTRegistApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IoTRegistApi.Controllers
{
    public class ClearController : ApiController
    {
        [HttpPut]
        public IHttpActionResult Put(Int32 ID)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Invald Clear data.");
            }
            else
            {
                try
                {
                    VisitorModel model = new VisitorModel();
                    Registration_dbEntities dbEntity = new Registration_dbEntities();
                    Visitors_Profile user = dbEntity.Visitors_Profile.Find(ID);
                    Debug.WriteLine("-------------" + user.VisitorName);
                    user.VisitTime = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                    user.Arrived = "No";
                    user.NFCid = Int64.Parse("0");
                    model.Clear(ID, user);
                    Debug.WriteLine("Clear success");

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
