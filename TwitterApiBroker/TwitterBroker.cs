using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TwitterApiBorker
{
    public class TwitterBroker
    {
        public string FromDate { get; set; }

        public string EndDate { get; set; }

        public async Task<string> GetTotalTweets(string twitterDomain, string bearerToken)
        {
            try
            {
                this.FromDate = this.GetUTCDate(7);
                this.EndDate = this.GetUTCDate(0);

                string getURL = string.Format("{0}/2/tweets/counts/recent?query={1}&start_time={2}&end_time={3}", twitterDomain, 10, this.FromDate, this.EndDate);

                var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, getURL);
                requestUserTimeline.Headers.Add("Authorization", "Bearer " + bearerToken);

                using (var httpClient = new HttpClient())
                {
                    await Task.Delay(12000);
                    using (HttpResponseMessage response = await httpClient.SendAsync(requestUserTimeline))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var json = JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
                            JToken jToken = JToken.Parse(json.ToString());
                            if (json != null)
                            {
                                return ((JValue)jToken.SelectToken("meta").SelectToken("total_tweet_count")).Value.ToString();
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex) {   throw ex;   }
        }

        public string GetUTCDate(int subtract)
        {
            DateTime theDate = DateTime.Now.ToUniversalTime();
            if (subtract != 0)
                theDate -= new TimeSpan(subtract - 1, 20, 55, 0);
            return (theDate.ToString("s") + "Z");
        }

        //public string GetTotalNumberOfTweets(string twitterDomain, string bearerToken)
        //{
        //    var totalTweets = this.GetTotalTweets(twitterDomain, bearerToken);

        //    if (totalTweets != null)
        //    {
        //        return totalTweets;
        //    }
            
        //    return 0;
        //}

        public int AverageNoOfTweets(string twitterDomain, string bearerToken)
        {
            var totaNoOfTweets = this.GetTotalTweets(twitterDomain, bearerToken);
            if (totaNoOfTweets != null)
            {
                int totalTweets = Convert.ToInt32(totaNoOfTweets.Result.ToString());
                var diffMinutes = (DateTime.Parse(this.EndDate.Substring(0, this.EndDate.Length - 1)) - DateTime.Parse(this.FromDate.Substring(0, this.FromDate.Length - 1))).TotalMinutes;

                return (Convert.ToInt32(totalTweets) / (int)diffMinutes);
            }
            return 0;
        }
    }
}
