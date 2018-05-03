using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signedUWP
{
    public class Users : INotifyPropertyChanged
    {


        public int VisitorId { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }
        
        public string Arrived { get; set; }

        public string CreateTime { get; set; }

        public string UpdateTime{ get; set; }

        public long NFCid { get; set; }

        public string TagId { get; set; }

        public int EventId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
