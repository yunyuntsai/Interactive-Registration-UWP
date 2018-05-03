using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signedUWP
{
    public class Photo
    {
        /*
         "VisitorId": 6,
        "Name": "Alice",
        "Company": "MS",
        "PhotoId": 6,
        "PhotoUrl": "https://visitorimageblob.blob.core.windows.net/picture-2018-04-30/Alice-12-36-31.jpg",
        "PhotoName": "Alice"
        */

        public int VisitorId { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public int PhotoId { get; set; }

        public string PhotoUrl { get; set; }

        public string PhotoName { get; set; }

    }
}
