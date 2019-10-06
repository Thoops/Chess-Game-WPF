using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
	/// <summary>
	/// This is the class for a Queen which inherits from Piece
	/// </summary>
	public class Queen : Piece
	{
		/** Empty Constructor for serialization purposes
        */
		public Queen() { }

		/** This Function sets the squares this piece is able to move to.
		 * The queen is able to move in any direction like a bishop and
		 * a rook combined.
		 * @param a_board - The chessboard the queen is on
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public override void SetValidSquares(Board a_board)
		{
			List<BoardSquare> squares = new List<BoardSquare>();
			Rook r = new Rook(this.Row, this.Column, 0, this.Color, "a");
			Bishop b = new Bishop(this.Row, this.Column, 0, this.Color, "a");
			r.SetValidSquares(a_board);
			b.SetValidSquares(a_board);
			squares = r.ValidSquares;
			squares.AddRange(b.ValidSquares);
			ValidSquares = squares;
			AttackingSquares = squares;
		}

		/** Constructor for Queen
		 * @param a_row - The row the queen is on
		 * @param a_column - The column the queen is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Queen(int a_row, int a_column, int a_value, Color a_color, string a_name)
			: base(a_row, a_column, a_value, a_color, a_name) { }

		/** Constructor for Queen that also takes a BoardSquare object
		 * @param a_row - The row the queen is on
		 * @param a_column - The column the queen is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Queen(int a_row, int a_column, int a_value, Color a_color, string a_name, BoardSquare a_square)
			: base(a_row, a_column, a_value, a_color, a_name, a_square) { }

		/** Function for writing to the game's move list when this piece
		 * moves
		 * @returns Q (for queen) + the name of the square it moved to
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public override string GetMoveString()
		{
			return "Q" + Square.Name;
		}
	}
}
