using System.Text;

namespace TicTacToe
{
 
    public class Board
    {
        #region Constant Members

        public const int ROWS = 3;
        public const int COLS = 3;

        public enum Seed
        {
            EMPTY, X, O
        }

        public enum State
        {
            READY, PLAYING, DRAW, XWINS, OWINS
        }

        #endregion

        #region Local Members

        public Cell[,] boardCells;
        public int currRow, currCol;

        #endregion

        #region Constructor

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

        /// <summary>
        /// Initializes the board for use
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks the board for a winner
        /// </summary>
        /// <param name="game"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public bool CheckForWinner(Game game, Seed seed)
        {
            bool result =

             (currRow == currCol                                         // Diagonal
                  && game.board.boardCells[0, 0].content == seed
                  && game.board.boardCells[1, 1].content == seed
                  && game.board.boardCells[2, 2].content == seed

             || currRow + currCol == 2                                   // Diagonal
                  && game.board.boardCells[0, 2].content == seed
                  && game.board.boardCells[1, 1].content == seed
                  && game.board.boardCells[2, 0].content == seed
                ||
                game.board.boardCells[currRow, 0].content == seed        // 3 vertical
                  && game.board.boardCells[currRow, 1].content == seed
                  && game.board.boardCells[currRow, 2].content == seed


                || game.board.boardCells[0, currCol].content == seed     // 3 horizontal
                  && game.board.boardCells[1, currCol].content == seed
                  && game.board.boardCells[2, currCol].content == seed);
            return result;
        }

        /// <summary>
        /// Checks the board for a draw
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Displayes the board
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static string Draw(Board board)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    if (col == 0) sb.Append("|");
                    sb.Append("" + board.boardCells[row, col].Draw());
                    sb.Append("|");
                }
                if (row == 0 || row == 1)
                    sb.Append("\n|----+---+----|\n");

            }
            return sb.ToString();
        }

        #endregion
    }


}