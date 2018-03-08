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

        public int Id { get; set; }
        //public string ProductCode { get { return ProductID.ToString(); } }
    
        public string Name { get; set; }

        public int Age { get; set; }
        
        public string Arrived { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    
        public long TagId { get; set; }
        //public decimal UnitPrice { get; set; }
        //public string UnitPriceString { get { return UnitPrice.ToString("######.00"); } }
        //public int UnitsInStock { get; set; }
        //public string UnitsInStockString { get { return UnitsInStock.ToString("#####0"); } }
        //public int CategoryId { get; set; }

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
