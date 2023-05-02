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
        public async Task DeleteJson()
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
        public async void DeletePort()
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };
            using (var client = new HttpClient(httpClientHandler))
            {
                var endpoint = new Uri("http://192.168.83.145:8181/onos/v1/network/configuration/ports");
                var response = await client.DeleteAsync(endpoint);
            }
        }
        public async void DeleteOnePort(string devicePort)
        {
            string input = devicePort;
            int P1 = 0;
            int P1_len = 2; 
            int P2 = 3; 
            int P2_len = 16;
            int P3 = 20;
            int P3_len = 1;

            string firstPart = input.Substring(P1, P1_len);
            string secondPart = input.Substring(P2,P2_len);
            string thirdPart = input.Substring(P3,P3_len);

            devicePort= firstPart +"%3A"+ secondPart+"%2F"+thirdPart;

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("onos", "rocks"),
            };
            using (var client = new HttpClient(httpClientHandler))
            {
                var endpoint = new Uri("http://192.168.83.145:8181/onos/v1/network/configuration/ports/"+devicePort);
                var response = await client.DeleteAsync(endpoint);
            }
        }
    }
}