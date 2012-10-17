using System;
using Newtonsoft.Json;
using Rainier.SecurityToken;
using Rainier.WebClient;

namespace Rainier.Images
{
    public interface IImageRepository
    {
        byte[] GetImageData(string query);
    }

    //public class SelfImageRepository : IImageRepository
    //{
    //     public byte[] GetImageData(string query)
    //     {
    //         return new System.Net.WebClient().DownloadData("http://rainier.apphb.com/images");
    //     }
    //}

    public class GettyImageRepository : IImageRepository
    {
        private readonly IWebClient _webClient;
        private readonly ISecurityTokenRepository _securityTokenRepository;
        private readonly Uri _requestUri = new Uri("http://connect.gettyimages.com/v1/search/SearchForImages");
        private readonly Random _rand  = new Random();

        public GettyImageRepository(IWebClient webClient, ISecurityTokenRepository securityTokenRepository)
        {
            _webClient = webClient;
            _securityTokenRepository = securityTokenRepository;
        }

        public byte[] GetImageData(string query)
        {
            return _webClient.DownloadData(GetImageUrl(query));
        }

        private Uri GetImageUrl(string query)
        {
            string imageUri = null;
            var index = _rand.Next(0, 6);

            try
            {
                var jsonRequest = BuildJsonRequest(query);
                imageUri = _webClient.GetJsonResponse(jsonRequest, _requestUri).SearchForImagesResult.Images[index].UrlWatermarkComp;
            }
            catch (Exception e)
            {
                imageUri = imageUri ?? @"http://rainier.apphb.com/images";
            }

            return new Uri(imageUri);
        }

        private string BuildJsonRequest(string query)
        {
            var token = _securityTokenRepository.GetSecurityToken();
            var jsonRequest = JsonConvert.SerializeObject(new
                {
                    RequestHeader = new
                        {
                            Token = token,
                            CoordinationId = ""
                        },
                    SearchForImages2RequestBody = new
                        {
                            FileTypes = "jpg",
                            Orientations = "",
                            Query = new {SearchPhrase = query},
                            ResultOptions = new {ItemStartNumber = 1, ItemCount = 7}
                        }
                });
            return jsonRequest;
        }
    }
}