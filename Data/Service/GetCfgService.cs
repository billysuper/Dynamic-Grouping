using Newtonsoft.Json;
using System.Net;

namespace Dynamic_Grouping.Data.Service
{
    public class GetCfgService
    {
        public RootObject JsonData;
        public HostsObject hostsData ;
        public HostsObject newhostsData;
        public HostsObject hostVlan;
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
            //set interfaces to hosts
            foreach (var Port in JsonData.ports)
            {
                foreach (var iface in Port.Value.interfaces)
                {
                    foreach (var host in newhostsData.Hosts)
                    {
                        if (Port.Key == host.devicePort)
                        {
                            host.iface = iface.name;
                        }
                    }
                }
            }
            //get vlan
            foreach (var vlan in vlanList)
            {
                vlan.Value.Clear();
            }
            foreach (var vlan in JsonData.apps.orgonosprojectvpls.vpls.vplsList)
            {
                vlanList[vlan.name] = new List<string>();
                foreach (var iface in vlan.interfaces)
                {
                    foreach (var host in newhostsData.Hosts)
                    {
                        if (host.iface == iface)
                        {
                            host.vpls = vlan.name;
                            vlanList[vlan.name].Add(host.devicePort);
                        }
                    }
                }
            }
            //transfer newhostsData to hostsData
            if (hostsData == null)
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
                            //newhost.vpls = host.vpls;
                            //newhost.iface = host.iface;
                            newhost.militaryPower = host.militaryPower;
                        }
                    }
                }
            }
            //compare
            hostsData = compareAndmove(newhostsData,hostsData,ip, port);
        }
        private HostsObject compareAndmove(HostsObject newdata,HostsObject data,string ip,string porT)
        {
            if (newdata.Hosts.Count != data.Hosts.Count)
            {
                //find broken host
                List<string> notexist = new List<string>();
                List<string> exist = new List<string>();
                foreach (var host in data.Hosts)
                {
                    notexist.Add(host.devicePort);
                }
                foreach (var newhost in newdata.Hosts)
                {
                    exist.Add(newhost.devicePort);
                }
                notexist = notexist.Except(exist).ToList();
                foreach (var nport in notexist)
                {
                    foreach (var host in data.Hosts)
                    {
                        foreach(var newhost in newdata.Hosts)
                        {
                            if (nport == host.devicePort && host.militaryPower == newhost.militaryPower && newhost.vpls == "Available")
                            {
                                newhost.vpls = host.vpls;
                                break;
                            }
                        }
                    }
                }
            }
            return newdata;
        }
    }
}