using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ChessApp
{
	/// <summary>
	/// This is the class for a Bishop which inherits from Piece
	/// </summary>
	public class Bishop : Piece
	{

		/** 
		 * Empty Constructor for serialization purposes
        */
		public Bishop() { }

		/** This Function sets the squares this piece is able to move to
		 * The bishop is able to move diagonally across the board
		 * and stays on the same color squares the entirety of the game
		 * @param a_board - The chessboard the bishop is on
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public override void SetValidSquares(Board a_board)
		{
			int row = this.Row;
			int column = this.Column;

			List<BoardSquare> squares = new List<BoardSquare>();

			int upRight = 8;
			int upLeft = 8;
			int downRight = 8;
			int downLeft = 8;

			foreach (BoardSquare s in a_board.ChessBoard)
			{
				if (Math.Abs(column - s.Column) == Math.Abs(row - s.Row))
				{
					if (!(row == s.Row && column == s.Column))
					{
						if (s.Occupied)
						{
							if (s.Row > row && s.Column > column && upRight > Math.Abs(column - s.Column))
							{
								upRight = Math.Abs(column - s.Column);
							}

							if (s.Row > row && s.Column < column && upLeft > Math.Abs(column - s.Column))
							{
								upLeft = Math.Abs(column - s.Column);
							}

							if (s.Row < row && s.Column < column && downLeft > Math.Abs(column - s.Column))
							{
								downLeft = Math.Abs(column - s.Column);
							}

							if (s.Row < row && s.Column > column && downRight > Math.Abs(column - s.Column))
							{
								downRight = Math.Abs(column - s.Column);
							}
						}
					}
				}
			}

			#region Getting Valid Squares

			foreach (BoardSquare s in a_board.ChessBoard)
			{
				if (Math.Abs(column - s.Column) == Math.Abs(row - s.Row) && s.Row != row && s.Column != column)
				{
					if (s.Row > row && s.Column > column && upRight >= Math.Abs(column - s.Column))
					{
						if(!(s.Row == this.Row && s.Column == this.Column))
						{
							squares.Add(s);
						}
						
					}

					if (s.Row > row && s.Column < column && upLeft >= Math.Abs(column - s.Column))
					{
						if (!(s.Row == this.Row && s.Column == this.Column))
						{
							squares.Add(s);
						}
					}

					if (s.Row < row && s.Column < column && downLeft >= Math.Abs(column - s.Column))
					{
						if (!(s.Row == this.Row && s.Column == this.Column))
						{
							squares.Add(s);
						}
					}

					if (s.Row < row && s.Column > column && downRight >= Math.Abs(column - s.Column))
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

		/** Constructor for Bishop
		 * @param a_row - The row the bishop is on
		 * @param a_column - The column the bishop is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Bishop(int a_row, int a_column, int a_value, Color a_color, string a_name)
			: base(a_row, a_column, a_value, a_color, a_name) { }

		/** Constructor for Bishop that also takess a BoardSquare object
		 * @param a_row - The row the bishop is on
		 * @param a_column - The column the bishop is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Bishop(int a_row, int a_column, int a_value, Color a_color, string a_name, BoardSquare a_square)
			: base(a_row, a_column, a_value, a_color, a_name, a_square) { }

		/** Function for writing to the game's move list when this piece
		 * moves
		 * @returns B + the name of the square it moved to
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public override string GetMoveString()
		{
			return "B" + Square.Name;
		}
	}
}
