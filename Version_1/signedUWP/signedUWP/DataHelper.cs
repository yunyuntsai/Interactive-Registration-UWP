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
            try
            {
                var items = JsonConvert.DeserializeObject<List<Users>>(response);
                //ListView st = JsonConvert.DeserializeObject<ListView>(response);
                for (int i = 0; i < items.Count; i++)
                {
                    UsersList.Add(items[i]);
                }
                long id = items[0].Id;
                string name = items[0].Name;
                return UsersList;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine("Exception: " + eSql.Message);
            }
            return null;
        }
    }
}
