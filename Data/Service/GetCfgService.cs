using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

using System;
using System.IO;
using Dynamic_Grouping.Data;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Dynamic_Grouping.Data.Service
{
    public class GetCfgService
    {
        public RootObject JsonData;
        public async Task GetJson(string ip, string port)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };

            using (var client = new HttpClient(httpClientHandler))
            {
                var endpoint = new Uri("http://"+ip+":"+port+"/onos/v1/network/configuration");
                var result = client.GetAsync(endpoint).Result;
                string jsonstring = result.Content.ReadAsStringAsync().Result;
                JsonData = JsonConvert.DeserializeObject<RootObject>(jsonstring); 
            }

        }
    }
    public class GetHostsService
    {
        public HostsObject hostsData;
        public void getHosts(string ip,string port)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };

            using (var client = new HttpClient(httpClientHandler))
            {
                var endpoint = new Uri("http://"+ip+":"+port+"/onos/v1/hosts");
                var result = client.GetAsync(endpoint).Result;
                string jsonstring = result.Content.ReadAsStringAsync().Result;
                hostsData= JsonConvert.DeserializeObject<HostsObject>(jsonstring);
            }
            foreach (var host in hostsData.Hosts)
            {
                foreach (var location in host.Locations)
                {
                    host.devicePort = location.ElementId+"/"+location.Port;
                }
            }
        }
        public Dictionary<string,string> MatchHostIface ( Dictionary<string,string> hostIface,HostsObject hostsData,RootObject data)
        {
            foreach (var port in data.ports)
            {
                foreach (var iface in port.Value.interfaces)
                {
                    foreach(var host in hostsData.Hosts)
                    {
                        if (port.Key == host.devicePort)
                        {
                            hostIface[host.devicePort] = iface.name;
                        }
                    }
                }
            }
            return hostIface;
        }
        public Dictionary<string, string> MatchHostPower(Dictionary<string, string> hostIface, HostsObject hostsData, RootObject data)
        {
            foreach (var port in data.ports)
            {
                foreach (var iface in port.Value.interfaces)
                {
                    foreach (var host in hostsData.Hosts)
                    {
                        if (port.Key == host.devicePort)
                        {
                            hostIface[host.devicePort] = iface.name;
                        }
                    }
                }
            }
            return hostIface;
        }
    }
}