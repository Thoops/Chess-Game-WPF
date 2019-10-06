using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChessApp
{
	/// <summary>
	/// This is the class for a Knight which inherits from Piece
	/// </summary>
	public class Knight : Piece
	{
		/** Empty Constructor for serialization purposes
        */
		public Knight() { }

		/** This Function sets the squares this piece is able to move to
		 * The knight is able to move in any direction up one and over two
		 * or vice versa
		 * @param a_board - The chessboard the knight is on
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public override void SetValidSquares(Board a_board)
		{
			int row = this.Row;
			int column = this.Column;

			int row1 = row + 2;
			int row2 = row + 1;
			int row3 = row - 1;
			int row4 = row - 2;
			int col1 = column + 2;
			int col2 = column + 1;
			int col3 = column - 1;
			int col4 = column - 2;

			List<BoardSquare> squares = new List<BoardSquare>();

			#region Getting Valid Squares
			foreach (BoardSquare s in a_board.ChessBoard)
			{
				if (s.Row == row1 || s.Row == row4)
				{
					if (s.Column == col2 || s.Column == col3)
					{

						squares.Add(s);

					}
				}

				else if (s.Row == row2 || s.Row == row3)
				{
					if (s.Column == col1 || s.Column == col4)
					{

						squares.Add(s);

					}
				}
			}
			#endregion

			ValidSquares = squares;
			AttackingSquares = squares;
		}

		/** Constructor for Knight
		 * @param a_row - The row the knight is on
		 * @param a_column - The column the knight is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Knight(int a_row, int a_column, int a_value, Color a_color, string a_name)
			: base(a_row, a_column, a_value, a_color, a_name) { }

		/** Constructor for Knight that also takes a BoardSquare object
		 * @param a_row - The row the knight is on
		 * @param a_column - The column the knight is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Knight(int a_row, int a_column, int a_value, Color a_color, string a_name, BoardSquare a_square)
			: base(a_row, a_column, a_value, a_color, a_name, a_square) { }


		/** Function for writing to the game's move list when this piece
		 * moves
		 * @returns N (for knight) + the name of the square it moved to
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public override string GetMoveString()
		{
			return "N" + Square.Name;
		}
	}
}
