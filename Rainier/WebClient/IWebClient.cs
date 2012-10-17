using System;
using System.Net;

namespace Rainier.WebClient
{
    public interface IWebClient
    {
        dynamic GetJsonResponse(string jsonRequest, Uri requestUri);
        byte[] DownloadData(Uri address);
    }
}