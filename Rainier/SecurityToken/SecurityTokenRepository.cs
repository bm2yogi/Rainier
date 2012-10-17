using System;
using Newtonsoft.Json;
using Rainier.WebClient;

namespace Rainier.SecurityToken
{
    public class SecurityTokenRepository : ISecurityTokenRepository
    {
        private readonly Uri _requestUri = new Uri("https://connect.gettyimages.com/v1/session/CreateSession");
        private readonly IWebClient _webClient;
        private string _token;

        public SecurityTokenRepository(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public string GetSecurityToken()
        {
            return _token ?? (_token = FetchSecurityToken());
        }

        private string FetchSecurityToken()
        {
            var jsonRequest = BuildJsonRequest();
            var jsonResponse = _webClient.GetJsonResponse(jsonRequest, _requestUri);

            return jsonResponse.CreateSessionResult.Token;
        }

        private string BuildJsonRequest()
        {
            return JsonConvert.SerializeObject(new
            {
                RequestHeader = new
                {
                    Token = "",
                    Detail = "",
                    CoordinationId = ""
                },
                CreateSessionRequestBody = new
                {
                    // I removed the username and password information.
                    // You can sign up for your own API account at https://api.gettyimages.com/
                    SystemId = "REMOVED",
                    SystemPassword = "REMOVED",
                    UserName = "REMOVED",
                    UserPassword = "REMOVED"
                }
            });
        }
    }
}