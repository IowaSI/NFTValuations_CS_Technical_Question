using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NFTValuations
{
    internal static class UriParser
    {
        internal static string Parse(string url)
        {
            switch (UriType(url))
            {
                case "base64":
                    var replacedUri = url.Replace("data:application/json;base64,", "");
                    byte[] data = Convert.FromBase64String(replacedUri);
                    return Encoding.UTF8.GetString(data);
                    
                case "ipfs":
                    var newUrl = $"https://ipfs.io/ipfs/{url.Replace("ipfs://","")}";
                    return GetContent(newUrl);
                    
                default:
                    return GetContent(url);
            }            
        }

        private static string UriType(string uri)
        {
            if (uri.StartsWith("data:application/json;base64,", StringComparison.OrdinalIgnoreCase))
                return "base64";

            if (uri.StartsWith("ipfs://", StringComparison.OrdinalIgnoreCase))
                return "ipfs";

            return uri;
        }

        private static string GetContent(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;
                        return responseBody;
                    }
                    else
                    {
                        throw new Exception($"HTTP GET request failed with status code: {response.StatusCode}");
                    }
                }
            }
            catch(Exception e)
            {
                throw new Exception($"Unexpected url:{url}", e);
            }
        }
    }
}