using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TransferVendorData.Exceptions;
using TransferVendorData.Web.Models;
using TransferVendorData.Web.Services.Base;

namespace TransferVendorData.Web.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IConfiguration _configuration;

        public AuthorizationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddTokenAsync(HttpClient httpClient)
        {
            var formData = new Dictionary<string, string>
                {
                    {"grant_type", _configuration["Authentication:grantType"]},
                    {"client_id", _configuration["Authentication:clientId"]},
                    {"client_secret", _configuration["Authentication:clientSecret"]},
                    {"scope", _configuration["Endpoints:redirectUrl"] + _configuration["Authentication:scope"]}
                };

            HttpResponseMessage response = await httpClient.PostAsync(_configuration["Authentication:accessTokenUrl"], new FormUrlEncodedContent(formData));

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                Token token = JsonConvert.DeserializeObject<Token>(jsonContent);
                    
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ServiceAuthenticationException(response.ReasonPhrase);
            }
            else
                throw new HttpRequestExceptionEx(response.StatusCode, response.ReasonPhrase);

        }
    }
}
