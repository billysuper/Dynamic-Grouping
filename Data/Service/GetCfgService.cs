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
        public string JsonData;
        public void GetJson()
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };

            using (var client = new HttpClient(httpClientHandler))
            {
                var endpoint = new Uri("http://192.168.83.145:8181/onos/v1/network/configuration");
                var result = client.GetAsync(endpoint).Result;
                string jsonstring = result.Content.ReadAsStringAsync().Result;
                JsonData = jsonstring;
            }

        }
    }
}