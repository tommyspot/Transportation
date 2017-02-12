using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Transportation.Api.Framework
{
    public class ApiContext
    {
        public Uri Uri { get; set; }
        public Route Route { get; set; }
        public string RequestBody { get; set; }
        public IHttpFileCollection HttpFileCollection { get; set; }
    }
}
