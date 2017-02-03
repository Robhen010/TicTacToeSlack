using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace TicTacToe
{
    /*
     Start Game             /ttt @userName 
     Display Current Board  /ttt CurrentBoard

         */

    public class Board
    {
        #region Constant Members

        public const int ROWS = 3;
        public const int COLS = 3;

        #endregion

        #region Local Members

        public Cell[,] boardCells;
        public int currRow, currCol;

        #endregion

        #region Constructors

        public Board()
        {
            boardCells = new Cell[ROWS, COLS];

            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    boardCells[row, col] = new Cell(row, col);
                }
            }
        }
        #endregion

        #region Init

        public Board Init(Board board)
        {

            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    board.boardCells[row, col].Erase();
                }
            }
            return board;
        }

        #endregion

        #region Methods

        public bool CheckForWinner(Game game, Seed seed)
        {

            bool result =

             (currRow == currCol            // Diagonal
                  && game.board.boardCells[0, 0].content == seed
                  && game.board.boardCells[1, 1].content == seed
                  && game.board.boardCells[2, 2].content == seed

             || currRow + currCol == 2    // Diagonal
                  && game.board.boardCells[0, 2].content == seed
                  && game.board.boardCells[1, 1].content == seed
                  && game.board.boardCells[2, 0].content == seed
                ||
                game.board.boardCells[currRow, 0].content == seed         // 3-in-the-row
                  && game.board.boardCells[currRow, 1].content == seed
                  && game.board.boardCells[currRow, 2].content == seed


                || game.board.boardCells[0, currCol].content == seed      // 3-in-the-column
                  && game.board.boardCells[1, currCol].content == seed
                  && game.board.boardCells[2, currCol].content == seed);
            return result;
        }

        public bool CheckForDraw(Game game)
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    if (game.board.boardCells[row, col].content == Seed.EMPTY)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static string Draw(Board board)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    if (col == 0) sb.Append("|");
                    sb.Append("" + board.boardCells[row, col].Draw());   // each cell paints itself

                    //if (col == 0 || col == 1)
                        sb.Append("|");
                }
              if (row == 0 || row == 1)            
                    sb.Append("\n|----+---+----|\n");
                
            }
            return sb.ToString();
        }

        #endregion
    }

    public enum Seed
    {
        EMPTY, X, O
    }

    public enum State
    {
       READY, PLAYING, DRAW, XWINS, OWINS
    }

    public class Game
    {

        public Board board;
        public State currState;
        public Seed currPlayer;     
        public Dictionary<string, Seed> playerDictionary; 

        public Game()
        {
            
        }
        public static Game Init( Game game)
        {
            Board board = new Board();
            game.board = board.Init(board);
            game.currState = State.READY;
            game.currPlayer = Seed.EMPTY;
            game.playerDictionary = new Dictionary<string, Seed>();

            return game;
        }

        public static Game ChallengeMadeStartGame(string p1, string p2) 
        {

            if (GameHelper.game == null || GameHelper.game.currState == State.PLAYING)
            {
                return null;
            }

            Game game = new Game();
            game = Game.Init(game);
            game.currPlayer = Seed.X;
            game.currState = State.PLAYING;
            game.playerDictionary.Add(p1,Seed.X);
            game.playerDictionary.Add(p2.Replace("@",""),Seed.O); // remove @ sign from parsed second user
      
            return game;
        }

        public static string MakeMove(Game game,Seed seed, string playerName, int row, int col)
        {


            if (row >= 0 && row < Board.ROWS && col >= 0 && col < Board.COLS
                  && game.board.boardCells[row, col].content == Seed.EMPTY)
            {
                game.board.boardCells[row, col].content = seed;
                game.board.currRow = row;
                game.board.currCol = col;
             
                // Check For a Winner
                if (game.board.CheckForWinner(game,seed))
                {
                    game.currState = State.READY;
                    return "Game Over! " + playerName + " Wins \n" + Board.Draw(game.board);
                }

                // Check For a Draw
                if (game.board.CheckForDraw(game))
                {
                    game.currState = State.READY;
                    return "Unfortunately the game ended in a draw :( \n" + Board.Draw(game.board);
                }

                // Switch Current Players
                game.currPlayer = seed == Seed.X ? Seed.O : Seed.X;

                // Return Game Board
                return Board.Draw(game.board);

            }
            else
            {
                // Invalid Move
               return "This move at (" + (row ) + "," + (col)
                      + ") is not valid. Try again";
            }        
        }
    }
}