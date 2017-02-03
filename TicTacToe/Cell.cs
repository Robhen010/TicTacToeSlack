namespace TicTacToe
{

    public class Cell
    {
        #region Local Variables

        public int row, col; // Represents row and column for a cell
        public Board.Seed content; // Represent the current value of the cell

        #endregion

        #region Public Methods
        public Cell(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        /// <summary>
        /// Erases the value of the cell
        /// </summary>
		public void Erase()
        {
            content = Board.Seed.EMPTY;
        }

        /// <summary>
        /// Draws the cell value
        /// </summary>
        /// <returns></returns>
		public string Draw()
        {
            switch (content)
            {
                case Board.Seed.X: return ("  X  ");
                case Board.Seed.O: return ("  O  ");
                case Board.Seed.EMPTY: return ("  E  ");
            }
            return string.Empty;
        }
        #endregion

    }
}