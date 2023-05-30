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
        public HostsObject hostsData ;
        public HostsObject newhostsData;
        private List<string> temp = new List<string>();
        public async Task Get(string ip, string port,Dictionary<string,List<string>> vlanList)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };
            using (var client = new HttpClient(httpClientHandler))
            {
                //network
                var endpoint = new Uri("http://" + ip + ":" + port + "/onos/v1/network/configuration");
                var result = client.GetAsync(endpoint).Result;
                string jsonstring = result.Content.ReadAsStringAsync().Result;
                JsonData = JsonConvert.DeserializeObject<RootObject>(jsonstring);
                //host
                var hendpoint = new Uri("http://" + ip + ":" + port + "/onos/v1/hosts");
                var hresult = client.GetAsync(hendpoint).Result;
                string hjsonstring = hresult.Content.ReadAsStringAsync().Result;
                newhostsData = JsonConvert.DeserializeObject<HostsObject>(hjsonstring);
            }
            //set port
            foreach (var host in newhostsData.Hosts)
            {
                foreach (var location in host.Locations)
                {
                    host.devicePort = location.ElementId + "/" + location.Port;
                }
            }
            //transfer newhostsData to hostsData
            if(hostsData == null)
            {
                hostsData = newhostsData;
            }
            else
            {
                foreach (var host in hostsData.Hosts)
                {
                    foreach (var newhost in newhostsData.Hosts)
                    {
                        if (newhost.Mac == host.Mac)
                        {
                            newhost.vpls = host.vpls;
                            newhost.iface = host.iface;
                            newhost.militaryPower = host.militaryPower;
                        }
                    }
                }
            }
            hostsData = newhostsData;
            //set interfaces to hosts
            foreach (var Port in JsonData.ports)
            {
                foreach(var iface in Port.Value.interfaces)
                {
                    foreach(var host in hostsData.Hosts)
                    {
                        if (Port.Key == host.devicePort)
                        {
                            host.iface = iface.name;
                        }
                    }
                }
            }
            //get vlan-port
            foreach (var vlan in JsonData.apps.orgonosprojectvpls.vpls.vplsList)
            {
                vlanList[vlan.name] = new List<string>();
                foreach(var iface in vlan.interfaces)
                {
                    foreach (var host in hostsData.Hosts)
                    {
                        if (host.iface == iface)
                        {
                            host.vpls = vlan.name;
                            vlanList[vlan.name].Add(host.devicePort);
                        }
                    }
                }
            }
        }
        /*public async Task GetJson(string ip, string port)
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

        public void getHosts(string ip, string port)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };

            using (var client = new HttpClient(httpClientHandler))
            {
                var endpoint = new Uri("http://" + ip + ":" + port + "/onos/v1/hosts");
                var result = client.GetAsync(endpoint).Result;
                string jsonstring = result.Content.ReadAsStringAsync().Result;
                hostsData = JsonConvert.DeserializeObject<HostsObject>(jsonstring);
            }
            foreach (var host in hostsData.Hosts)
            {
                foreach (var location in host.Locations)
                {
                    host.devicePort = location.ElementId + "/" + location.Port;
                }
            }
        }*/
    }
    /*public class GetHostsService
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
    }*/
}