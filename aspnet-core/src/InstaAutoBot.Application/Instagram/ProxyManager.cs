using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstaAutoBot.Instagram
{
    public class ProxyManager : InstaAutoBotDomainServiceBase
    {
        public ProxyManager()
        {


        }

        public async Task<HttpClientHandler> GetWebProxy()
        {
            var username = "iproyal965";
            var password = "8sPVRi7InvSWtivS_country-UnitedStates";
            
            var proxy = new WebProxy()
            {
                Address = new Uri($"http://rproxie.iproyal.com:31112"),
                //Credentials = CredentialCache.DefaultCredentials
                Credentials = new NetworkCredential(username, password)
            };

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            return httpClientHandler;
        }

    }
}
