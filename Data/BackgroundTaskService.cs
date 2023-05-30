using Dynamic_Grouping.Data.Service;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Dynamic_Grouping.Data
{
    public class BackgroundProgramService : IHostedService
    {
        private readonly SharedDataService _sharedDataService;
        private Timer _timer;

        public BackgroundProgramService(SharedDataService sharedDataService)
        {
            _sharedDataService = sharedDataService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //chack every 1 second.
            _timer = new Timer(UpdateData, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }

        private void UpdateData(object state)
        {
            // Update the data in the shared data service
            _sharedDataService.UpdateData();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();
            return Task.CompletedTask;
        }
    }
    public class SharedDataService
    {
        //shareData
        public List<string> militaryPowers { get; set; } = new List<string>() { "Unknown","infantry", "tank", "fighter" };
        public Dictionary<string, List<string>> vlanList { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>>  newvlanList { get; set; } = new Dictionary<string, List<string>>();
        /*public Dictionary<string, string> hostPower { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> hostIface { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, List<string>> vlanIfaces { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> powerhost { get; set; } = new Dictionary<string, List<string>>();*/
        public string ip="192.168.83.145",porT="8181";
        /*private bool changeOrNot=false;
        public bool ipsubmit=false;*/
        //import
        public GetCfgService getCfgService = new GetCfgService();
        public PostCfgService postCfgService = new PostCfgService();
        private DeleteCfgService deleteCfgService = new DeleteCfgService();

        /*private Vpls prevpls= new Vpls();
        private Vpls curvpls= new Vpls();*/
        public event Func<Task> OnDataUpdated;

        // Store your data here, e.g., a property or a field
        public int Data { get; set; }
        public bool isfinish = true;
        public async Task ExecuteMethodsInOrderAsync()
        {
            if(getCfgService.JsonData != null)
            {
                await postCfgService.PostJson(getCfgService.JsonData, getCfgService.hostsData, ip, porT);
            }
            await getCfgService.Get(ip, porT, vlanList); 
        }

        public void UpdateData()
        {
            // Update your data here, e.g., call an API or perform some calculation
            isfinish = false;
            Data++;
            ExecuteMethodsInOrderAsync();
            if(getCfgService.hostVlan != null&&getCfgService.hostVlan.Hosts.Count!= getCfgService.hostsData.Hosts.Count)
            {
                getCfgService.hostVlan = getCfgService.hostsData;
            }
            isfinish = true;
            /*if (ipsubmit)
            {
                getCfgService.GetJson(ip, porT);

                getHostsService.getHosts(ip, porT);
                if (hostPower.Count == 0)
                {
                    foreach (var host in getHostsService.hostsData.Hosts)
                    {
                        hostPower[host.devicePort] = host.militaryPower;
                    }
                }
                else
                {
                    foreach (var host in getHostsService.hostsData.Hosts)
                    {
                        host.militaryPower = hostPower[host.devicePort];
                    }
                }

                hostIface = getHostsService.MatchHostIface(hostIface, getHostsService.hostsData, getCfgService.JsonData);
                vlanIfaces = VO.getVlanCfg(getHostsService.hostsData, getCfgService.JsonData, vlanIfaces,hostIface);
                //delete port of deleted hosts
                bool exist = false;
                foreach (var port in getCfgService.JsonData.ports)
                {
                    exist = false;
                    foreach (var iface in port.Value.interfaces)
                    {
                        foreach (var host in getHostsService.hostsData.Hosts)
                        {
                            if (port.Key == host.devicePort)
                            {
                                exist = true; break;
                            }
                        }
                    }
                    if (!exist)
                    {
                        deleteCfgService.DeleteOnePort(port.Key, ip, porT);
                        //delete iface in vpls
                        foreach (var vlan in vlanIfaces)
                        {
                            foreach (var iface in vlan.Value)
                            {
                                if (hostIface[port.Key] == iface)
                                {
                                    temp[0] = vlan.Key;
                                    temp[1] = iface;
                                }
                            }
                            if (temp[0] != "")
                            {
                                vlanIfaces[temp[0]].Remove(temp[1]);
                                temp[1] = "";
                                //find other host
                                foreach (var avaiface in vlanIfaces["Available"])
                                {
                                    foreach (var hostiface in hostIface)
                                    {
                                        //find it's port
                                        if (avaiface == hostiface.Value)
                                        {
                                            foreach (var host in getHostsService.hostsData.Hosts)
                                            {
                                                //find it's host
                                                if (host.devicePort == hostiface.Key)
                                                {
                                                    //find the same power
                                                    if (host.militaryPower == hostPower[port.Key])
                                                    {
                                                        temp[1] = avaiface;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (temp[1] != "")
                                {
                                    vlanIfaces["Available"].Remove(temp[1]);
                                    vlanIfaces[temp[0]].Add(temp[1]);
                                }
                                temp[0] = "";
                                temp[1] = "";
                            }
                        }
                    }
                }
                getCfgService.JsonData = VO.setVlanCfg(getCfgService.JsonData, vlanIfaces);
                postCfgService.PostJson(getCfgService.JsonData, ip, porT);
            }*/
            // Trigger the event to notify subscribers that the data has been updated
            OnDataUpdated?.Invoke();
        }
    }

}