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
    public class DeleteController : ApiController
    {
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
                    VisitorModel model = new VisitorModel();
                    Registration_dbEntities dbEntity = new Registration_dbEntities();
                    Visitors_Profile user = dbEntity.Visitors_Profile.Find(ID);
                    Debug.WriteLine("-------------" + user.VisitorName);
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
                    VisitorModel model = new VisitorModel();
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
