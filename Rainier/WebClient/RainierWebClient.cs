using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Rainier.WebClient
{
    public class RainierWebClient : IWebClient
    {
        public dynamic GetJsonResponse(string jsonRequest, Uri requestUri)
        {
            dynamic jsonResponse = new ExpandoObject();
            var request = BuildWebRequest(jsonRequest, requestUri);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var reader = new StreamReader(response.GetResponseStream());
                jsonResponse = JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());
            }

            return jsonResponse;
        }

        public byte[] DownloadData(Uri address)
        {
            return new System.Net.WebClient().DownloadData(address);
        }

        private HttpWebRequest BuildWebRequest(string jsonRequest, Uri requestUri)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";
            request.ContentLength = Encoding.UTF8.GetByteCount(jsonRequest);

            var writer = new StreamWriter(request.GetRequestStream());
            writer.Write(jsonRequest);
            writer.Flush();

            return request;
        }
    }
}