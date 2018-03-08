using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;

namespace WebApplication2.Controllers
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
                    UserModel model = new UserModel();
                    WebApplication2201802_dbEntities1 dbEntity = new WebApplication2201802_dbEntities1();
                    USERTABLE user = dbEntity.USERTABLE.Find(ID);
                    Debug.WriteLine("-------------" + user.UserName);
                    user.UpdateAt = DateTime.Parse(DateTime.UtcNow.AddHours(8).ToString());
                    user.Arrived = "No";
                    user.TagID = Int64.Parse("0");
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
