using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signedUWP
{
    public class ListView
    {
        [JsonProperty("transactions")]
        public List<Users> transactions { get; set; }

    }
}
