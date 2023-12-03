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
        public List<string> militaryPowers { get; set; } = new List<string>() { "Unknown","infantry", "tank", "UAV" };
        public Dictionary<string, List<string>> vlanList { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>>  newvlanList { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string,string> portif = new Dictionary<string,string>();
        public string ip ="";
        public string porT ="8181";
        public bool ipsubmit=false;
        //import
        public GetCfgService getCfgService = new GetCfgService();
        public PostCfgService postCfgService = new PostCfgService();
        private DeleteCfgService deleteCfgService = new DeleteCfgService();

        /*private Vpls prevpls= new Vpls();
        private Vpls curvpls= new Vpls();*/
        public event Func<Task> OnDataUpdated;

        // Store your data here, e.g., a property or a field
        public string Data { get; set; }
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
            if (ipsubmit)
            {
                isfinish = false;
                Data = "Success";
                ExecuteMethodsInOrderAsync();
                if(getCfgService.hostVlan != null&&getCfgService.hostVlan.Hosts.Count!= getCfgService.hostsData.Hosts.Count)
                {
                    getCfgService.hostVlan = getCfgService.hostsData;
                }
                isfinish = true;
            }
            else
            {
                Data = "Fail";
            }
            OnDataUpdated?.Invoke();
        }
    }

}