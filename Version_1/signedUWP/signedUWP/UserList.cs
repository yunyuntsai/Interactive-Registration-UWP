using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signedUWP
{
    public class UserList : ObservableCollection<Users>
    {
        public UserList()
        {
        }



        public Users GetUsersById(int id)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id == id)
                {
                    return Items[i];
                }
            }
            return null;
        }
        /*public ProductList GetProductsByCategoryId(int id)
        {
            ProductList list = new ProductList();
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].CategoryId == id)
                {
                    list.Add(Items[i]);
                }
            }
            return list;
        }*/

    }

}