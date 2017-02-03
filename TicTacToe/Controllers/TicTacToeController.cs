using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using TicTacToe;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    [RoutePrefix("api/tictactoe")]
    public class TicTacToeController : ApiController
    {

        private string _token = "KZOgE8iLQOBwOpCPGJbPjEeC";
   

        
        [HttpPost]
        public IHttpActionResult Post(GameStartModel request)
        {

            try
            {
                SlackResponseModel response = null;
              //  _token = request.token;
                if (ValidateToken(request))
                {
   
                   if (request.text.Contains("challenge"))
                    {
                       

                        //SlackManager manager = new SlackManager();
                        //var userList =  manager.GetUserList();
                        //return Ok(userList);
                        // Pull out opponent's name 
                       // return Ok("request text: " + request.text + " opponentName: ");
                        var offSet = request.text.IndexOf('@');
                        var opponentName = request.text.Substring(offSet, request.text.Length - offSet);                    
                      
                        //Game Start
                        GameHelper.game = Game.ChallengeMadeStartGame(request.user_name, opponentName);


                        response = new SlackResponseModel()
                        {
                            response_type = "in_channel",
                            text = request.user_name + " has challenged " + opponentName + " to a game of tic tac toe." +
                                   "\n" + request.user_name + " is assigned X while " + opponentName +
                                   " is assigned O. X Moves first",
                            attachments = new[] { new Attachment() { text = Board.Draw(GameHelper.game.board) } }
                        };
                        return Ok(response);
                    }
                    else if (request.text.ToLower().Contains("play"))
                    {


                        // Validate whos turn
                        if (GameHelper.game.playerDictionary[request.user_name] != GameHelper.game.currPlayer)
                        {
                            return Ok("Please wait your turn");
                        }


                        var x = request.text.Substring(request.text.IndexOf(' ') + 1, 1);
                        var y = request.text.Substring(request.text.IndexOf(',') + 1, 1);

                        var result = Game.MakeMove(GameHelper.game, GameHelper.game.currPlayer, request.user_name,
                            Convert.ToInt32(x),
                            Convert.ToInt32(y));

                        response = new SlackResponseModel()
                        {
                            response_type = "in_channel",
                            text = request.user_name + " Plays " + x + "," + y,
                            attachments = new[] { new Attachment() { text = result } }
                        };

                        return Ok(response);

                    }
                    else if (request.text.ToLower().Contains("help"))
                    {
                        response = new SlackResponseModel()
                        {
                            response_type = "in_channel",
                            text = "Slack TicTacToe by Robert Henderson",
                            attachments = new[]
                            {
                            new Attachment()
                            {
                                text =
                                    "/ttt challenge @UserName   - Starts Game \n" +
                                    "/ttt play X,Y              - Play Move\n" +
                                    "/ttt status                - Gets Current Game Status\n" +
                                    "/ttt draw board            - Gets Current Game Board\n" +
                                    "/ttt end                   - Ends Game\n"
                            }
                        }
                        };


                        return Ok(response);
                    }
                    else if (request.text.ToLower().Contains("status"))
                    {
                        return Ok(GameHelper.game.currState.ToString());
                    }
                    else if (request.text.ToLower().Contains("draw board"))
                    {
                        response = new SlackResponseModel()
                        {
                            response_type = "ephemeral",
                            text = "Tic Tac Toe Game Board",
                            attachments = new[] { new Attachment() { text = Board.Draw(GameHelper.game.board) } }
                        };

                        return Ok(response);
                    }
                    return Ok("Text: " + request.text + " Command: " + request.command + " Token: " + request.token);

                }
                else
                {
                    return BadRequest("Invalid Token");
                }
            }
            catch (Exception e)
            {

                return BadRequest("An Exception occured: " + e.Message + "\n" + e.InnerException + " stack trace" +e.StackTrace);
            }
        }

        private bool ValidateToken(GameStartModel request)
        {
            if (request == null) return false;

            return request.token.Equals(_token);
        }

      

    }
}
