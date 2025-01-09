using BankTest.Client.Providers;
using BankTest.Library.Models.Dto;
using System.Net.Http.Json;

namespace BankTest.Client.Services
{
    public class LoginAPI
    {
        private string _baseUrl = "";
        private HttpClient _httpClient;
        public string BaseUrl
        {
            get { return _baseUrl; }
            set
            {
                _baseUrl = value;
                if (!string.IsNullOrEmpty(_baseUrl) && !_baseUrl.EndsWith("/"))
                    _baseUrl += '/';
            }
        }
        public LoginAPI(string baseUrl, HttpClient httpClient)
        {
            BaseUrl = baseUrl;
            _httpClient = httpClient;
        }
        public async Task<List<UserInfo>> UserAll(TokenAuthenticationStateProvider tokenProvider)
        {

            await tokenProvider.RefreshTokenAsync(_httpClient);

            var result = await _httpClient.GetAsync(@"api/User/GetUserDto");

            var responseContent = await result.Content.ReadFromJsonAsync<List<UserInfo>>();

            return responseContent == null ? new List<UserInfo>() : responseContent;
        }
    }
}
