using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Jwt_Web_Client_Sample.Controllers
{
    public class BaseController : Controller
    {
        HttpClient _client = new HttpClient(); //HttpClientFactory.Create();
        private static MediaTypeWithQualityHeaderValue _mediaType = new MediaTypeWithQualityHeaderValue("application/json");

        public async Task<string> GetAsync<TEntity>(string url) where TEntity : class
        {
            var requestUrl = $"{AppData.ApiUrl}/{url}";

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(_mediaType);

            string token = HttpContext.Session.GetString(AppData.TokenName);
            if (string.IsNullOrEmpty(token) == false)
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _client.GetAsync(requestUrl);

            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<string> PostAsync<TEntity>(string url, TEntity model, string email = null) where TEntity : class
        {
            var requestUrl = $"{AppData.ApiUrl}/{url}";

            if (!string.IsNullOrEmpty(email))
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("Email", email);
            }
            string token = HttpContext.Session.GetString(AppData.TokenName);
            if (string.IsNullOrEmpty(token) == false)
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");


            string stringData = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(stringData, Encoding.UTF8, _mediaType.MediaType);

            var response = await _client.PostAsync(requestUrl, contentData);
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
