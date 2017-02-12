using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Transportation.Api.Framework
{
    public class HttpPostedFileWrapper : IHttpPostedFile
    {       
        public HttpPostedFileWrapper(HttpPostedFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            this.innerFile = file;
        }

        private HttpPostedFile innerFile = null;

        public int ContentLength { get { return this.innerFile.ContentLength; } }

        public string ContentType { get { return this.innerFile.ContentType; } }

        public string FileName { get { return this.innerFile.FileName; } }

        public Stream InputStream { get { return this.innerFile.InputStream; } }


        public void SaveAs(string filename)
        {
            this.innerFile.SaveAs(filename);
        }
    }
}
