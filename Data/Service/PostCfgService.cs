using System.Net;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace Dynamic_Grouping.Data.Service
{
    public class PostCfgService
    {
        public string result;
        public string serializedJson;
        public async Task PostJson(RootObject data,string ip,string port)
        {
            var json = JsonConvert.SerializeObject(data.apps.orgonosprojectvpls.vpls.vplsList);
            serializedJson = json;
            string Json = @"{""org.onosproject.vpls"":{""vpls"":{""vplsList"":" + json + "}}}";
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };
            using (var client = new HttpClient(httpClientHandler))
            {
                var content = new StringContent(Json, Encoding.UTF8, "application/json");
                var endpoint = new Uri("http://"+ip+":"+port+"/onos/v1/network/configuration/apps");
                var response = await client.PostAsync(endpoint,content);
                result = await response.Content.ReadAsStringAsync();
            }
        }
    }
    public class PostHostsService
    {
        public async void PostPorts(Dictionary<string,string>hostIface,string ip,string port)
        {
            RootObject data = new RootObject();
            data.ports = new Dictionary<string, Port>();    
                foreach (var hostiface in hostIface)
                {
                    Port newPort = new Port();
                    //newPort.interfaces = new List<Interface>();
                    Interface newIface = new Interface();
                    newIface.name = hostiface.Value;
                    newPort.interfaces.Add(newIface);
                    data.ports.Add(hostiface.Key, newPort);
                }
            var json = JsonConvert.SerializeObject(data.ports);
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };
            using (var client = new HttpClient(httpClientHandler))
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var endpoint = new Uri("http://"+ip+":"+port+"/onos/v1/network/configuration/ports");
                var response = await client.PostAsync(endpoint, content);
                var result = await response.Content.ReadAsStringAsync();
            }
        }
    }
}