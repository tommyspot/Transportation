using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Transportation.IntegrationTests
{
    public class ApiHelper
    {
        private string responseContent;

        public WebResponse Response { get; set; }

        public HttpWebResponse HttpResponse
        {
            get { return (HttpWebResponse)Response; }
        }            

        public string ResponseContent
        {
            get {

                if (responseContent == null)
                {
                    var encoding = UTF8Encoding.UTF8;
                    using (var reader = new System.IO.StreamReader(Response.GetResponseStream(), encoding))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
                return responseContent;
            }
        }

        public JObject ResponseJson
        {
            get
            {
                return JObject.Parse(ResponseContent);
            }
        }

        public void PerformRequest(Dictionary<string, object> properties, string url, string method)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method;
            request.Accept = "application/json";
            request.ContentType = "application/json";

            if (method.ToUpper() != "GET")
            {
                var json = new JObject();
                
                foreach (string key in properties.Keys)
                {
                    object value = properties[key];

                    if (value != null)
                    {
                        if (value is JToken)
                        {
                            json.Add(key, value as JToken);
                        }
                        else
                        {
                            json.Add(key, value.ToString());
                        }
                    }
                    else
                    {
                        json.Add(key, null);
                    }
                }

                var sw = new StreamWriter(request.GetRequestStream());
                var serialized = JsonConvert.SerializeObject(json);
                sw.Write(serialized);
                sw.Close();
            }

            Response = request.GetResponse();
        }

        public void PerformRequest(string url, string method)
        {
            PerformRequest(new Dictionary<string, object>(), url, method);
        }
    }
}
