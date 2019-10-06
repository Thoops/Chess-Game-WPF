using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
	/// <summary>
	/// This is the class for a King which inherits from Piece
	/// </summary>
	public class King : Piece
	{
		/** 
		 * Empty Constructor for serialization purposes
        */
		public King() { }

		/** This Function sets the squares this piece is able to move to
		 * The king is able to move in any direction across the board
		 * but only one space
		 * @param a_board - The chessboard the king is on
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public override void SetValidSquares(Board a_board)
		{
			List<BoardSquare> squares = new List<BoardSquare>
			{
				a_board.ReturnSquare(Row + 1, Column + 1),
				a_board.ReturnSquare(Row + 1, Column - 1),
				a_board.ReturnSquare(Row + 1, Column),
				a_board.ReturnSquare(Row - 1, Column + 1),
				a_board.ReturnSquare(Row - 1, Column - 1),
				a_board.ReturnSquare(Row - 1, Column),
				a_board.ReturnSquare(Row, Column + 1),
				a_board.ReturnSquare(Row, Column - 1),
				//a_board.ReturnSquare(Row, Column)
			};
			squares.RemoveAll(cell => cell == null);
			AttackingSquares = squares;
			List<BoardSquare> occupiedSquares = new List<BoardSquare>();
			occupiedSquares = squares.FindAll(x => x.Occupied);
			occupiedSquares.RemoveAll(x => x.PieceColor != Color);
			List<BoardSquare> valSquares = squares.Except(occupiedSquares).ToList();
			ValidSquares = valSquares;
		}

		/** This Function sets the squares the king is unable
		 * to move to
		 * @param a_player - The opposing player
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public void SetInvalidSquares(Player a_player)
		{
			InvalidSquares.Clear();
			#region Getting Invalid Squares
			foreach (Piece p in a_player.Pieces)
			{
				InvalidSquares.AddRange(p.AttackingSquares);
			}
			#endregion

			List<BoardSquare> realValidSquares = new List<BoardSquare>();
			realValidSquares = ValidSquares.Except(InvalidSquares).ToList();
			ValidSquares = realValidSquares;
		}

		/** Constructor for King
		 * @param a_row - The row the king is on
		 * @param a_column - The column the king is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public King(int a_row, int a_column, int a_value, Color a_color, string a_name)
			: base(a_row, a_column, a_value, a_color, a_name){}

		/** Constructor for King that also takess a BoardSquare object
		 * @param a_row - The row the king is on
		 * @param a_column - The column the king is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public King(int a_row, int a_column, int a_value, Color a_color, string a_name, BoardSquare a_square)
			: base(a_row, a_column, a_value, a_color, a_name, a_square) { }

		/** Function for writing to the game's move list when this piece
		 * moves
		 * @returns K (for king) + the name of the square it moved to
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public override string GetMoveString()
		{
			return "K" + Square.Name;
		}
	}
}
