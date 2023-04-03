using Dynamic_Grouping.Data;
using Microsoft.AspNetCore.Http;
using System;
using Dynamic_Grouping.Data;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Dynamic_Grouping.Data.Service
{
    public class DeleteCfgService
    {
        public async void DeleteJson()
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };
            using (var client = new HttpClient(httpClientHandler))
            {
                var endpoint = new Uri("http://192.168.83.145:8181/onos/v1/network/configuration/apps");
                var response = await client.DeleteAsync(endpoint);
            }
        }
    }
}