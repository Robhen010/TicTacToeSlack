using System.Collections.Generic;

namespace TicTacToe
{
    public class Game
    {
        #region Local Variables

        public Board board;
        public Board.State currState;
        public Board.Seed currPlayer;
        public Dictionary<string, Board.Seed> playerDictionary;

        #endregion

        #region Constructor

        public Game()
        {

        }
        #endregion

        #region Public Methods
        public static Game Init(Game game)
        {
            Board board = new Board();
            game.board = board.Init(board);
            game.currState = Board.State.READY;
            game.currPlayer = Board.Seed.EMPTY;
            game.playerDictionary = new Dictionary<string, Board.Seed>();

            return game;
        }

        public static Game ChallengeMadeStartGame(string p1, string p2)
        {

            if (GameHelper.game.currState == Board.State.PLAYING)
            {
                return GameHelper.game;
            }

            Game game = new Game();
            game = Game.Init(game);
            game.currPlayer = Board.Seed.X;
            game.currState = Board.State.PLAYING;
            game.playerDictionary.Add(p1, Board.Seed.X);
            game.playerDictionary.Add(p2.Replace("@", ""), Board.Seed.O); // remove @ sign from parsed second user

            return game;
        }

        public static string MakeMove(Game game, Board.Seed seed, string playerName, int row, int col)
        {


            if (row >= 0 && row < Board.ROWS && col >= 0 && col < Board.COLS
                && game.board.boardCells[row, col].content == Board.Seed.EMPTY)
            {
                game.board.boardCells[row, col].content = seed;
                game.board.currRow = row;
                game.board.currCol = col;

                // Check For a Winner
                if (game.board.CheckForWinner(game, seed))
                {
                    game.currState = Board.State.READY;
                    return "Game Over! " + playerName + " Wins \n" + Board.Draw(game.board);
                }

                // Check For a Draw
                if (game.board.CheckForDraw(game))
                {
                    game.currState = Board.State.READY;
                    return "Unfortunately the game ended in a draw :( \n" + Board.Draw(game.board);
                }

                // Switch Current Players
                game.currPlayer = seed == Board.Seed.X ? Board.Seed.O : Board.Seed.X;

                // Return Game Board
                return Board.Draw(game.board);

            }
            else
            {
                // Invalid Move
                return "This move at (" + (row) + "," + (col)
                       + ") is not valid. Try again";
            }
        }

        #endregion
    }
}