using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Http;

namespace Sitecore.DataExchange.Sfmc.Custom.Activities.Controllers
{
    public class ConfigController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            // this is only because im using ngrok for localhost tunnel
            var host = GetNgrokHost();

            //string host = HttpContext.Current.Request.Url.Host;

            var result = new
            {
                workflowApiVersion = "1.1",
                metaData = new
                {
                    icon = "sitecore.png",
                    category = "message"
                },
                type = "REST",
                userInterfaces = new
                {
                    configInspector = new
                    {
                        size = "medium"
                    }
                },
                arguments = new
                {
                    execute = new
                    {
                        inArguments = new object[]
                        {
                            new { contactIdentifier="{{Contact.Key}}"}
                        },
                        url= $"{host}/api/sfmc/execute",
                    }
                },
                configurationArguments = new
                {
                    save= new
                    {
                        url = $"{host}/api/sfmc/save"
                    },
                    publish = new
                    {
                        url = $"{host}/api/sfmc/publish"
                    },
                    validate = new
                    {
                        url = $"{host}/api/sfmc/validate"
                    },
                    stop = new
                    {
                        url = $"{host}/api/sfmc/stop"
                    }
                },
                edit = new 
                {
                    url = $"{host}/Home/Index",
                    height = 200,
                    width = 500
                }
            };

            return Ok(JToken.FromObject(result));
        }


        private string GetNgrokHost()
        {
            string req;
            using (HttpClient client = new HttpClient())
            {
                req = client.GetAsync("http://localhost:4040/api/tunnels").Result.Content.ReadAsStringAsync().Result;
            }
            JObject ngrokResponse = JObject.Parse(req);
            JArray tunnels = ngrokResponse["tunnels"] as JArray;

            var httpsTunnel = tunnels.SingleOrDefault(t => t["proto"].Value<string>().Equals("https"));

            if (httpsTunnel != null)
            {
                return (string)httpsTunnel["public_url"];
            }
            else
            {
                throw new Exception("cannot find https tunnel");
            }
        }
    }
}
