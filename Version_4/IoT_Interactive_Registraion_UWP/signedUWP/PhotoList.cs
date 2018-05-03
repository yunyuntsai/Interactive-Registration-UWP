using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signedUWP
{
    public class PhotoList : ObservableCollection<Photo>
    {
    
        public PhotoList()
        {
        }



        public Photo GetPhotoById(int id)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].PhotoId == id)
                {
                    return Items[i];
                }
            }
            return null;
        }
    }
}
