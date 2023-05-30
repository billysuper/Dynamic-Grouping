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
        public List<string> temp = new List<string>();
        public async Task PostJson(RootObject data,HostsObject hostData,string ip,string port)
        {
            //update new vpls configuration
            temp.Clear();
            foreach(var host in hostData.Hosts)
            {
                if (!temp.Contains(host.vpls))
                {
                    temp.Add(host.vpls);
                }
            }
            data.apps.orgonosprojectvpls.vpls.vplsList.Clear();
            foreach (var vlan in temp)
            {
                VplsInfo newvpls = new VplsInfo();
                newvpls.interfaces = new List<string>();
                newvpls.name = vlan;
                data.apps.orgonosprojectvpls.vpls.vplsList.Add(newvpls);
            }
            foreach (var host in hostData.Hosts)
            {
                foreach (var vpls in data.apps.orgonosprojectvpls.vpls.vplsList)
                {
                    if (host.vpls == vpls.name)
                    {
                        vpls.interfaces.Add(host.iface);
                    }
                }
            }
            //network
            var json = JsonConvert.SerializeObject(data.apps.orgonosprojectvpls.vpls.vplsList);
            serializedJson = json;
            string Json = @"{""org.onosproject.vpls"":{""vpls"":{""vplsList"":" + json + "}}}";//add encapsolution
            //port
            data.ports.Clear();
            foreach (var host in hostData.Hosts)
            {
                Port newPort = new Port();
                Interface newIface = new Interface();
                newIface.name = host.iface;
                newPort.interfaces.Add(newIface);
                data.ports.Add(host.devicePort, newPort);
            }
            var pjson = JsonConvert.SerializeObject(data.ports);
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };
            using (var client = new HttpClient(httpClientHandler))
            {
                //network
                var content = new StringContent(Json, Encoding.UTF8, "application/json");
                var endpoint = new Uri("http://"+ip+":"+port+"/onos/v1/network/configuration/apps");
                var response = await client.PostAsync(endpoint,content);
                result = await response.Content.ReadAsStringAsync();
                //port
                var pcontent = new StringContent(pjson, Encoding.UTF8, "application/json");
                var pendpoint = new Uri("http://" + ip + ":" + port + "/onos/v1/network/configuration/ports");
                var presponse = await client.PostAsync(pendpoint, pcontent);
                var presult = await presponse.Content.ReadAsStringAsync();
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