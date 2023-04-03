using Dynamic_Grouping.Data;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Dynamic_Grouping.Data.Service
{
    public class PostCfgService
    {
        public string result;
        public string serializedJson;
        public async void PostJson(RootObject data)
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
                var endpoint = new Uri("http://192.168.83.145:8181/onos/v1/network/configuration/apps");
                var response = await client.PostAsync(endpoint,content);
                result = await response.Content.ReadAsStringAsync();
            }
        }
    }
}