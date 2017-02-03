using System;
using System.Net.Http;
using System.Threading;

namespace TicTacToe
{
  
    public class SlackManager
    {

        //private string _slackBaseUrl = "https://slack.com/api/";

        //public string GetUserList(string token)
        //{
           
        //        if (string.IsNullOrWhiteSpace(_slackBaseUrl) || string.IsNullOrWhiteSpace("users.list"))
        //        {
        //            return string.Empty;
        //        }

        //        using (var client = new HttpClient(new HttpClientHandler()))
        //        {
        //            client.BaseAddress = new Uri(_slackBaseUrl);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

        //            HttpResponseMessage response;
        //            try
        //            {
        //                response = client.GetAsync("users.list?token=" + token).Result;
        //            }
        //            catch (Exception e)
        //            {


        //                return null;
        //            }

        //            if (response.IsSuccessStatusCode)
        //            {
        //                return  response.Content.ReadAsStringAsync().Result;
        //            }

        //            return string.Empty;
        //        }
        //    }

        
    }
}