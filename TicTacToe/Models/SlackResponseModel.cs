using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToe.Models
{

    public class SlackResponseModel
    {
        public string response_type { get; set; }
        public string text { get; set; }
        public Attachment[] attachments { get; set; }
    }

    public class Attachment
    {
        public string text { get; set; }
    }

}