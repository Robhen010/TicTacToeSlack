using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToe
{


	public class Cell
	{
		private int row, col;
		public Seed content;

		public Cell(int row, int col)
		{
			this.row = row;
			this.col = col;
		}

		public void Erase()
		{
			content = Seed.EMPTY;
		}

		public string Draw()
		{
			switch (content)
			{
				case Seed.X: return("  X  "); 
				case Seed.O: return("  O  "); 
				case Seed.EMPTY: return("  E  "); 
			}
		    return string.Empty;
		}


	}
}