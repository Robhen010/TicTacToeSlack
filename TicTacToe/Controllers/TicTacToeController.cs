using System;
using System.Linq;
using System.Web.Http;
using TicTacToe.Models;
using Newtonsoft.Json;

namespace TicTacToe.Controllers
{
    [RoutePrefix("api/tictactoe")]
    public class TicTacToeController : ApiController
    {
        #region Variables

        private string _token = System.Web.Configuration.WebConfigurationManager.AppSettings["SlackTeamToken"];

        #endregion

        #region Public Methods

        /// <summary>
        /// Slack slash commands come in as a json which C# converts into a model if it matches properly
        /// This Post acceptes the json response from slack and handles it accordingly      
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post(GameStartModel request)
        {

            try
            {
                SlackResponseModel response = null;
                if (ValidateToken(request))
                {
                    if (request.text.ToLower().Contains("init"))
                    {
                        GameHelper.game = Game.Init(new Game());
                        return Ok("The game has been initialized. Please make a challenge");
                    }
                    else if (request.text.ToLower().Contains("challenge"))
                    {


                        // Parse opponent name 
                        var offSet = request.text.IndexOf('@');
                        if (offSet == -1)
                        {
                            return Ok("Invalid Challenge");
                        }

                        var opponentName = request.text.Substring(offSet, request.text.Length - offSet);

                        // Create an instance of slack manager to use slack rest api calls
                        SlackManager manager = new SlackManager();

                        // pull slack channel user list
                        var userList = manager.GetUserList();
                        SlackUserListModel userListModel = JsonConvert.DeserializeObject<SlackUserListModel>(userList);

                        // if the opponent isn't in the slack user list for the channel do not proceed
                        if (!userListModel.members.Any(x => x.name.Equals(opponentName.Replace("@", ""))))
                        {
                            return Ok("There are no users in this channel with the name " + opponentName);
                        }

                        // Check for existing game
                        if (GameHelper.game != null && GameHelper.game.currState == Board.State.PLAYING)
                        {
                            return Ok("Unfortunately a game is already in session");
                        }

                        // Game Start
                        GameHelper.game = Game.ChallengeMadeStartGame(request.user_name, opponentName);

                        // Create Response
                        response = new SlackResponseModel()
                        {
                            response_type = "in_channel",
                            text = request.user_name + " has challenged " +
                                   opponentName + " to a game of tic tac toe." +
                                   "\n" + request.user_name + " is assigned X while " +
                                   opponentName +  " is assigned O. X Moves first",
                                 
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

                        int x, y;

                        try
                        {
                            // Get coordinants for move
                            var xString = request.text.Substring(request.text.IndexOf(' ') + 1, 1);
                            var yString = request.text.Substring(request.text.IndexOf(',') + 1, 1);
                            x = Convert.ToInt32(xString);
                            y = Convert.ToInt32(yString);
                        }
                        catch (Exception e)
                        {
                            return Ok("Please enter a valid R,C coordinate. R: 0-2 and C: 0-2");
                        }

                        // play move based on current player and coordinants
                        var result = Game.MakeMove(GameHelper.game, GameHelper.game.currPlayer, request.user_name, x, y);

                        // create response
                        response = new SlackResponseModel()
                        {
                            response_type = "in_channel",
                            text = request.user_name + " plays " + x + "," + y + "\n \n",
                            attachments = new[] { new Attachment() { text = result } }
                        };

                        return Ok(response);

                    }
                    else if (request.text.ToLower().Contains("help"))
                    {
                        // create response containing help information
                        response = new SlackResponseModel()
                        {
                            response_type = "ephemeral",
                            text = "Slack TicTacToe by Robert Henderson",
                            attachments = new[]
                            {
                            new Attachment()
                            {
                                text =
                                    " The Game Board is a multidimentional array or a three by three matrix \n" +
                                    "The R represents the Rows and the C represents the Columns" +
                                    "/ttt challenge @UserName   - Starts Game \n" +
                                    "/ttt play R,C              - Play Move Where R And C Are Coordinates 0-2\n" +
                                    "/ttt status                - Gets Current Game Status\n" +
                                    "/ttt draw board            - Gets Current Game Board and Current Player\n" +
                                    "/ttt init                  - Initializes For a New One\n"
                            }
                        }
                        };


                        return Ok(response);
                    }
                    else if (request.text.ToLower().Contains("status"))
                    {
                        // Get current status of the game
                        if (GameHelper.game != null)
                        {
                            switch (GameHelper.game.currState)
                            {
                                case Board.State.PLAYING:
                                    return Ok("There is a current game in session and its " + Game.GetNameFromSeed(GameHelper.game.currPlayer) + "'s turn");

                                case Board.State.READY:
                                    return Ok("The board is ready. Please make a challenge");

                            }
                        }
                        else
                        {
                            return Ok("No game is in session");
                        }

                        return Ok(GameHelper.game.currState);
                    }
                    else if (request.text.ToLower().Contains("draw board"))
                    {
                        // Draws the game board for the entire channel
                        if (GameHelper.game != null)
                        {
                            response = new SlackResponseModel()
                            {
                                response_type = "in_channel",
                                text = "Tic Tac Toe Game Board",
                                attachments = new[] {new Attachment() {
                                    text = GameHelper.game.currPlayer != Board.Seed.EMPTY?
                                    Board.Draw(GameHelper.game.board) + "\n "+
                                    Game.GetNameFromSeed(GameHelper.game.currPlayer) +
                                    "'s turn" :  Board.Draw(GameHelper.game.board) } }
                            };
                            return Ok(response);
                        }
                        return Ok("Game is not initialized please run the /ttt destroy command and make a challenge");

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
                // Display exception message for debugging purpose 
                return BadRequest("An Exception occured: " + e.Message + "\n" + e.InnerException + " stack trace" + e.StackTrace);
            }
        }

        #endregion

        #region Private Methods

        private bool ValidateToken(GameStartModel request)
        {
            if (request == null) return false;

            return request.token.Equals(_token);
        }



        #endregion
    }
}
