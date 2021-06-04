using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace InstaAutoBot.Instagram
{
    public class PhoneNumberManager : InstaAutoBotDomainServiceBase
    {
        //http://smspva.com/new_theme_api.html

        private const string ApiKey = "JcDL2CF7HV4TnXbXx96ZQ3pnHaChMA";
        public PhoneNumberManager()
        {


        }

        public async Task<RentNewPhoneNumberResponse> RentNewPhoneNumber()
        {
            var response = await ExecuteRequest("get_number", "opt16");
            var data = JsonConvert.DeserializeObject<RentNewPhoneNumberResponse>(response.Content);
            return data;
        }

        public async Task<string> WaitForInstaVerificationCode(string phoneNumberId)
        {
            var counter = 0;
            while (counter < 10)
            {
                counter++;

                var sms = await GetSms(phoneNumberId);
                if (sms != null && string.IsNullOrEmpty(sms.Text) == false && sms.Text.Contains("Instagram"))
                {
                    return Regex.Replace(sms.Text, @"[^\d]", "");
                } 

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(20));
            }
            return string.Empty;
        }

        /// <summary>
        /// Receiving a SMS for a certain service
        /// </summary>
        /// <param name="phoneNumberId">In this method id parameter is indicated from the response to request for phone number get_number</param>
        /// <returns></returns>
        public async Task<GetSmsResponse> GetSms(string phoneNumberId)
        {
            var response = await ExecuteRequest("get_sms", "opt4",
                additionalQueryParameters: new Dictionary<string, string>()
                {
                    {"id",phoneNumberId},
                });

            var data = JsonConvert.DeserializeObject<GetSmsResponse>(response.Content);

            return data;
        }

        private RestClient GetClient()
        {
            return new RestClient("http://simsms.org");
        }


        private async Task<IRestResponse> ExecuteRequest(
            string method,
            string service,
            Dictionary<string, string> additionalQueryParameters = null,
            string country = "KG")
        {
            var client = GetClient();
            var request = new RestRequest("priemnik.php", DataFormat.Json);

            //return requests.get('http://simsms.org/priemnik.php?metod=get_number&country=KG&service=opt16&apikey=JcDL2CF7HV4TnXbXx96ZQ3pnHaChMA').json()

            request.AddQueryParameter("metod", method);
            request.AddQueryParameter("service", service);
            request.AddQueryParameter("apikey", ApiKey);
            request.AddQueryParameter("country", country);

            if (additionalQueryParameters != null &&
                additionalQueryParameters.Count > 0)
            {
                foreach (var additionalQueryParameter in additionalQueryParameters)
                {
                    request.AddQueryParameter(additionalQueryParameter.Key, additionalQueryParameter.Value);
                }
            }

            return await client.ExecuteAsync(request);
        }
    }


    public class GetSmsResponse
    {
        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("extra")]
        public string Extra { get; set; }

        [JsonProperty("karma")]
        public string Karma { get; set; }

        [JsonProperty("pass")]
        public string Pass { get; set; }

        [JsonProperty("sms")]
        public string Sms { get; set; }

        [JsonProperty("balanceOnPhone")]
        public string BalanceOnPhone { get; set; }
    }
    public class RentNewPhoneNumberResponse
    {
        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("again")]
        public string Again { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("extra")]
        public string Extra { get; set; }

        [JsonProperty("karma")]
        public string Karma { get; set; }

        [JsonProperty("pass")]
        public string Pass { get; set; }

        [JsonProperty("sms")]
        public string Sms { get; set; }

        [JsonProperty("balanceOnPhone")]
        public string BalanceOnPhone { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("CountryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("branchId")]
        public string BranchId { get; set; }

        [JsonProperty("callForwarding")]
        public string CallForwarding { get; set; }

        [JsonProperty("goipSlotId")]
        public string GoipSlotId { get; set; }

        [JsonProperty("lifeSpan")]
        public string LifeSpan { get; set; }
    }
}
