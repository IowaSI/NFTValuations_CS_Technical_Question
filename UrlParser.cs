using NFTValuations.Enums;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NFTValuations
{
    internal static class UrlParser
    {
        internal static string Parse(string url)
        {
            switch (UriType(url))
            {
                case UrlEnums.UrlType.base64:
                    var replacedUri = url.Replace("data:application/json;base64,", "");
                    byte[] data = Convert.FromBase64String(replacedUri);
                    return Encoding.UTF8.GetString(data);

                case UrlEnums.UrlType.json:
                    return url.Replace("data:application/json;utf8,", "");

                case UrlEnums.UrlType.ipfs:
                    var newUrl = $"https://ipfs.io/ipfs/{url.Replace("ipfs://","")}";
                    return GetContent(newUrl);
                    
                default:
                    return GetContent(url);
            }            
        }

        private static UrlEnums.UrlType UriType(string uri)
        {
            if (uri.StartsWith("data:application/json;base64,", StringComparison.OrdinalIgnoreCase))
                return UrlEnums.UrlType.base64;

            if (uri.StartsWith("ipfs://", StringComparison.OrdinalIgnoreCase))
                return UrlEnums.UrlType.ipfs;

            if (uri.StartsWith("data:application/json;", StringComparison.OrdinalIgnoreCase))
                return UrlEnums.UrlType.json;

            return UrlEnums.UrlType.origin;
        }

        private static string GetContent(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    
                    var response = client.GetAsync(url).Result;
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