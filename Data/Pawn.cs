using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ChessApp
{
	/// <summary>
	/// This is the class for a Pawn which inherits from Piece
	/// </summary>
	public class Pawn : Piece
	{
		/** Empty Constructor for serialization purposes
        */
		public Pawn() { }

		/** This Function sets the squares this piece is able to move to
		 * The pawn is able to move in front of itself one space, unless 
		 * it has not moved then it can move up two spaces. It can't capture
		 * a piece in its same column but it can capture a piece on its
		 * diagonals in the direction it can move. This calls WhitePawnSquares
		 * if it is white or BlackPawnSquares if it is black to set its valid squares.
		 * @param a_board - The chessboard the pawn is on
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public override void SetValidSquares(Board a_board)
		{
			if(this.Color == Color.White)
			{
				WhitePawnSquares(a_board);
			}
			else if (this.Color == Color.Black)
			{
				BlackPawnSquares(a_board);
			}
		}

		/** This function is called for white pawns which in this app
		 * start in row 6 and move in the negative direction. It gets
		 * the squares from the board directly in front of it and if 
		 * it hasn't moved it can move two rows up. If the squares on its
		 * diagonals in front of it are occupied by a piece of the opposite color
		 * then those squares are also added because the pawn is able to
		 * capture them
		 * @param a_board - The chessboard the pawn is on
		 * @author Thomas Hooper
		 * @date February 2019
        */
		private void WhitePawnSquares(Board a_board)
		{
			List<BoardSquare> squares = new List<BoardSquare>();
			List<BoardSquare> attSquares = new List<BoardSquare>();

			BoardSquare square1;
			BoardSquare square2;

			square1 = a_board.ReturnSquare(Row - 1, Column);
			square2 = a_board.ReturnSquare(Row - 2, Column);

			#region Getting Move Squares
			try
			{
				if (!square1.Occupied)
				{
					squares.Add(square1);
				}
			}
			catch (Exception)
			{

			}

			try
			{
				if (Row == 6)
				{
					if (!(square1.Occupied && square2.Occupied))
					{
						squares.Add(square2);
					}
				}
			}
			catch (Exception)
			{

			}
			#endregion

			#region Getting Attack Squares
			BoardSquare attSquare1 = a_board.ReturnSquare(Row - 1, Column - 1);
			BoardSquare attSquare2 = a_board.ReturnSquare(Row - 1, Column + 1);

			try
			{
				if(attSquare1.Occupied)
				{
					squares.Add(attSquare1);
				}
				attSquares.Add(attSquare1);
			}
			catch (Exception)
			{

			}
			try
			{
				if (attSquare2.Occupied)
				{
					squares.Add(attSquare2);
				}
				attSquares.Add(attSquare2);
			}
			catch (Exception)
			{

			}
			#endregion
			AttackingSquares = attSquares;
			ValidSquares = squares;
		}

		/** This function is called for black pawns which in this app
		 * start in row 1 and move in the positive direction. It gets
		 * the squares from the board directly in front of it and if 
		 * it hasn't moved it is able to move two rows up. If the squares on its
		 * diagonals directly in front of it are occupied by a piece of the opposite color
		 * then those squares are also added because the pawn is able to
		 * capture them
		 * @param a_board - The chessboard the pawn is on
		 * @author Thomas Hooper
		 * @date February 2019
        */
		private void BlackPawnSquares(Board a_board)
		{
			List<BoardSquare> squares = new List<BoardSquare>();
			List<BoardSquare> attSquares = new List<BoardSquare>();

			BoardSquare square1;
			BoardSquare square2;

			square1 = a_board.ReturnSquare(Row + 1, Column);
			square2 = a_board.ReturnSquare(Row + 2, Column);

			#region Getting Move Squares
			try
			{
				if (!square1.Occupied)
				{
					squares.Add(square1);
				}
			}
			catch (Exception)
			{

			}

			try
			{
				if (Row == 1)
				{
					if (!(square1.Occupied && square2.Occupied))
					{
						squares.Add(square2);
					}
				}
			}
			catch (Exception)
			{

			}
			#endregion

			#region Getting Attack Squares
			BoardSquare attSquare1 = a_board.ReturnSquare(Row + 1, Column - 1);
			BoardSquare attSquare2 = a_board.ReturnSquare(Row + 1, Column + 1);

			try
			{
				if (attSquare1.Occupied)
				{
					squares.Add(attSquare1);
				}
				attSquares.Add(attSquare1);
			}
			catch (Exception)
			{

			}
			try
			{
				if (attSquare2.Occupied)
				{
					squares.Add(attSquare2);
				}
				attSquares.Add(attSquare2);
			}
			catch (Exception)
			{

			}
			#endregion
			AttackingSquares = attSquares;
			ValidSquares = squares;
		}

		/** Constructor for Pawn
		 * @param a_row - The row the pawn is on
		 * @param a_column - The column the pawn is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Pawn(int a_row, int a_column, int a_value, Color a_color, string a_name)
			: base(a_row, a_column, a_value, a_color, a_name){}

		/** Constructor for Pawn that also takes a BoardSquare object
		 * @param a_row - The row the pawn is on
		 * @param a_column - The column the pawn is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Pawn(int a_row, int a_column, int a_value, Color a_color, string a_name, BoardSquare a_square)
			: base(a_row, a_column, a_value, a_color, a_name, a_square) { }

		/** Function for writing to the game's move list when this piece
		 * moves
		 * @returns The name of the square it moved to
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public override string GetMoveString()
		{
			return Square.Name;
		}


	}
}
