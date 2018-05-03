using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace signedUWP
{
    public class DataHelper
    {
        public static UserList GetUsers(string response)
        {
            const string GetUsersQuery = "select UserID, UserName, Gender from USERS";

            Debug.WriteLine("debug--------------" );
            var UsersList = new UserList();
            var empty = new UserList();
            try
            {
                var items = JsonConvert.DeserializeObject<List<Users>>(response);
                //ListView st = JsonConvert.DeserializeObject<ListView>(response);
                for (int i = 0; i < items.Count; i++)
                {
                    UsersList.Add(items[i]);
                }
                long id = items[0].VisitorId;
                string name = items[0].Name;
                if( items.Count >0 ) return UsersList;
                else {
                    Users e = new Users();
                    e.VisitorId = 0;
                    e.Name = "null";
                    e.Arrived = "null";
                    e.Company = "null";
                    e.TagId = "null";
                    e.NFCid = 0;
                    e.UpdateTime = "null";
                    e.CreateTime = "null";

                    empty.Add(e);
                    return empty;
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }

        public static EventList GetEvents(string response)
        {
          
            Debug.WriteLine("debug--------------");
            var EventList = new EventList();
            var empty = new EventList();
            try
            {
                var items = JsonConvert.DeserializeObject<List<Events>>(response);
                //ListView st = JsonConvert.DeserializeObject<ListView>(response);
                for (int i = 0; i < items.Count; i++)
                {
                    EventList.Add(items[i]);
                }
                long id = items[0].EventId;
                string name = items[0].EventName;
                if (items.Count > 0) return EventList;
                else
                {
                    Events e = new Events();
                    e.EventId = 0;
                    e.EventName = "null";
                    e.EventType = "null";
                    e.StartDate = "null";
                    e.StartTime = "null";
                    e.EndDate = "null";
                    e.EndTime = "null";
                    empty.Add(e);
                    return empty;
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }

        public static PhotoList GetPhotos(string response)
        {

            Debug.WriteLine("debug--------------");
            var PhotoList = new PhotoList();
            var empty = new PhotoList();
            try
            {
                var items = JsonConvert.DeserializeObject<List<Photo>>(response);
                //ListView st = JsonConvert.DeserializeObject<ListView>(response);
                for (int i = 0; i < items.Count; i++)
                {
                    PhotoList.Add(items[i]);
                }
                long id = items[0].VisitorId;
                string name = items[0].PhotoUrl;
                if (items.Count > 0) return PhotoList;
                else
                {
 
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }
    }
}
