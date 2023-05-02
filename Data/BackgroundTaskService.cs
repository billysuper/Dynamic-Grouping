using Dynamic_Grouping.Data.Service;
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
            _timer = new Timer(UpdateData, null, TimeSpan.Zero, TimeSpan.FromSeconds(0.5));
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
        private GetCfgService getCfgService = new GetCfgService();
        private GetHostsService getHostsService = new GetHostsService();
        private DeleteCfgService deleteCfgService = new DeleteCfgService();
        public event Func<Task> OnDataUpdated;

        // Store your data here, e.g., a property or a field
        public int Data { get; private set; }

        public void UpdateData()
        {
            // Update your data here, e.g., call an API or perform some calculation
            Data++;
            /*getCfgService.GetJson();
            getHostsService.getHosts();
            //delete port of deleted hosts
            foreach (var port in getCfgService.JsonData.ports)
            {
                bool exist = false;
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
                if(!exist)
                {
                    deleteCfgService.DeleteOnePort(port.Key);
                }
            }*/
            // Trigger the event to notify subscribers that the data has been updated
            OnDataUpdated?.Invoke();
        }
    }

}