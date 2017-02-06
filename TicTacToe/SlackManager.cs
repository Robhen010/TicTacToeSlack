using System;
using System.Net.Http;
using System.Threading;

namespace TicTacToe
{

    public class SlackManager
    {

        private string _slackBaseUrl = "https://slack.com/api/";
        private string _token = System.Web.Configuration.WebConfigurationManager.AppSettings["SlackManagerToken"];
        private string _userListUrl = "users.list";

        public string GetUserList()
        {

            if (string.IsNullOrWhiteSpace(_slackBaseUrl) || string.IsNullOrWhiteSpace(_userListUrl))
            {
                return string.Empty;
            }

            using (var client = new HttpClient(new HttpClientHandler()))
            {
                client.BaseAddress = new Uri(_slackBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

                HttpResponseMessage response;
                try
                {
                    response = client.GetAsync("users.list?token=" + _token).Result;
                }
                catch (Exception e)
                {
                    
                    //TODO Log here 

                    return null;
                }

                return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : string.Empty;
            }
        }


    }
}