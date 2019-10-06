using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
	/// <summary>
	/// This is the class for a Rook which inherits from Piece
	/// </summary>
	public class Rook : Piece
	{
		/** Empty Constructor for serialization purposes
        */
		public Rook() { }

		/** This Function sets the squares this piece is able to move to.
		 * The rook is able to move across any row or any column for any amount
		 * of squares.
		 * @param a_board - The chessboard the rook is on
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public override void SetValidSquares(Board a_board)
		{
			int row = this.Row;
			int column = this.Column;

			int up = 8;
			int right = 8;
			int down = 0;
			int left = 0;

			List<BoardSquare> squares = new List<BoardSquare>();

			foreach (BoardSquare s in a_board.ChessBoard)
			{
				if (s.Row == row || s.Column == column)
				{
					if (!(s.Column == column && s.Row == row))
					{
						if (s.Occupied)
						{
							if (row > s.Row && down < s.Row)
							{
								down = s.Row;
							}
							else if (row < s.Row && up > s.Row)
							{
								up = s.Row;
							}

							if (column > s.Column && left < s.Column)
							{
								left = s.Column;
							}
							else if (column < s.Column && right > s.Column)
							{
								right = s.Column;
							}
						}
					}
				}
			}

			#region Getting Valid Squares
			foreach (BoardSquare s in a_board.ChessBoard)
			{
				if (s.Row == row || s.Column == column && !(s.Column == column && s.Row == row))
				{
					if (s.Row <= up && s.Row >= down && s.Column <= right && s.Column >= left)
					{
						if (!(s.Row == this.Row && s.Column == this.Column))
						{
							squares.Add(s);
						}
					}
				}
			}
			#endregion

			ValidSquares = squares;
			AttackingSquares = squares;
		}

		/** Constructor for Rook
		 * @param a_row - The row the rook is on
		 * @param a_column - The column the rook is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Rook(int a_row, int a_column, int a_value, Color a_color, string a_name)
			: base(a_row, a_column, a_value, a_color, a_name) { }

		/** Constructor for Rook that also takes a BoardSquare object
		 * @param a_row - The row the rook is on
		 * @param a_column - The column the rook is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Rook(int a_row, int a_column, int a_value, Color a_color, string a_name, BoardSquare a_square)
			: base(a_row, a_column, a_value, a_color, a_name, a_square) { }

		/** Function for writing to the game's move list when this piece
		 * moves
		 * @returns R (for rook) + the name of the square it moved to
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public override string GetMoveString()
		{
			return "R" + Square.Name;
		}
	}
}
