using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Handler
{
    internal class KakaoMapService
    {
        private readonly string apiKey = "//dapi.kakao.com/v2/maps/sdk.js?appkey=d502ef8977e585cc483511208b13312e&libraries=services";
        private readonly HttpClient client;

        public KakaoMapService()
        {
            client = new HttpClient();
        }

        public async Task<JObject> GetLocationDataAsync(string address)
        {
            var requestUrl = $"https://dapi.kakao.com/v2/local/search/address.json?query={address}";
            client.DefaultRequestHeaders.Add("Authorization", $"KakaoAK {apiKey}");

            var response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

    }
}
