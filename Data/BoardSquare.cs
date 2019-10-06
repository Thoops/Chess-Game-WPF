using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{
	/// <summary>
	/// This is the class for an individual BoardSquare
	/// </summary>
	public class BoardSquare
	{
		public BoardSquare() { }
		private int m_row; /**< The row of the board the square is on. */
		public int Row
		{
			get { return m_row; }
			set { m_row = value; }
		}
		private int m_column; /**< The column of the board the square is on. */
		public int Column
		{
			get { return m_column; }
			set { m_column = value; }
		}
		private int m_value; /**< The value of the boardsquare. */
		public int Value
		{
			get { return m_value; }
			set { m_value = value; }
		}
		private bool m_occupied; /**< True if the square has a piece on it and otherwise false.  */
		public bool Occupied
		{
			get { return m_occupied; }
			set { m_occupied = value; }
		}
		private Color m_color; /**< The Color of the piece that is on this square */
		public Color PieceColor
		{
			get { return m_color; }
			set { m_color = value; }
		}
		private string m_name; /**< The name of the square */
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}
		#region White Piece Values
		private double m_whitePawnValue; /**< The value of this square for white pawns */
		private double m_whiteKnightValue; /**< The value of this square for white knights */
		private double m_whiteBishopValue; /**< The value of this square for white bishops */
		private double m_whiteRookValue; /**< The value of this square for white rooks */
		private double m_whiteQueenValue; /**< The value of this square for white queen */
		private double m_whiteKingValue; /**< The value of this square for white king */
		public double WhitePawnValue
		{
			get { return m_whitePawnValue; }
			set { m_whitePawnValue = value; }
		}
		public double WhiteKnightValue
		{
			get { return m_whiteKnightValue; }
			set { m_whiteKnightValue = value; }
		}
		public double WhiteBishopValue
		{
			get { return m_whiteBishopValue; }
			set { m_whiteBishopValue = value; }
		}
		public double WhiteRookValue
		{
			get { return m_whiteRookValue; }
			set { m_whiteRookValue = value; }
		}
		public double WhiteQueenValue
		{
			get { return m_whiteQueenValue; }
			set { m_whiteQueenValue = value; }
		}
		public double WhiteKingValue
		{
			get { return m_whiteKingValue; }
			set { m_whiteKingValue = value; }
		}
		#endregion
		#region Black Piece Values
		private double m_blackPawnValue; /**< The value of this square for black pawns */
		private double m_blackKnightValue; /**< The value of this square for black knights */
		private double m_blackBishopValue; /**< The value of this square for black bishops */
		private double m_blackRookValue; /**< The value of this square for black rooks */
		private double m_blackQueenValue; /**< The value of this square for black queen */
		private double m_blackKingValue; /**< The value of this square for black king */
		public double BlackPawnValue
		{
			get { return m_blackPawnValue; }
			set { m_blackPawnValue = value; }
		}
		public double BlackKnightValue
		{
			get { return m_blackKnightValue; }
			set { m_blackKnightValue = value; }
		}
		public double BlackBishopValue
		{
			get { return m_blackBishopValue; }
			set { m_blackBishopValue = value; }
		}
		public double BlackRookValue
		{
			get { return m_blackRookValue; }
			set { m_blackRookValue = value; }
		}
		public double BlackQueenValue
		{
			get { return m_blackQueenValue; }
			set { m_blackQueenValue = value; }
		}
		public double BlackKingValue
		{
			get { return m_blackKingValue; }
			set { m_blackKingValue = value; }
		}
		#endregion

		/** Constructor for BoardSquare
		 * @param a_row - The row the square is on
		 * @param a_column - The column the square is on
		 * @param a_value - the value of the square
		 * @param a_name - the name of the square
		 * @param a_piece - the piece that is on the square
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public BoardSquare(int a_row, int a_column, int a_value, string a_name, Piece a_piece)
		{
			Row = a_row;
			Column = a_column;
			Value = a_value;
			//Piece = a_piece;
			Occupied = true;
			PieceColor = a_piece.Color;
			Name = a_name;
			SetSquareValue();
		}

		/** Constructor for BoardSquare
		 * @param a_row - The row the square is on
		 * @param a_column - The column the square is on
		 * @param a_value - the value of the square
		 * @param a_name - the name of the square
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public BoardSquare(int a_row, int a_column, int a_value, string a_name)
		{
			Row = a_row;
			Column = a_column;
			Value = a_value;
			Occupied = false;
			Name = a_name;
			SetSquareValue();
		}

		/** Sets the values of the square for each piece
		 * @param a_PawnValue - The value this square has for a pawn
		 * @param a_KnightValue - The value this square has for a knight
		 * @param a_BishopValue - The value this square has for a bishop
		 * @param a_RookValue - The value this square has for a rook
		 * @param a_QueenValue - The value this square has for a queen
		 * @param a_KingValue - The value this square has for a king
		 * @author Thomas Hooper
		 * @date June 2019
        */
		public void SetValues(double a_PawnValue, double a_KnightValue, double a_BishopValue, double a_RookValue, double a_QueenValue, double a_KingValue, Color a_color)
		{
			if(a_color == Color.White)
			{
				WhitePawnValue = a_PawnValue;
				WhiteKnightValue = a_KnightValue;
				WhiteBishopValue = a_BishopValue;
				WhiteRookValue = a_RookValue;
				WhiteQueenValue = a_QueenValue;
				WhiteKingValue = a_KingValue;
			}
			else if (a_color == Color.Black)
			{
				BlackPawnValue = a_PawnValue;
				BlackKnightValue = a_KnightValue;
				BlackBishopValue = a_BishopValue;
				BlackRookValue = a_RookValue;
				BlackQueenValue = a_QueenValue;
				BlackKingValue = a_KingValue;
			}
		}

		/** Sets the values of the square for each piece by getting the name
		 * of the square, identifying it and calling SetValues to set each of 
		 * the values this square has for specific pieces
		 * @author Thomas Hooper
		 * @date June 2019
        */
		public void SetSquareValue()
		{
			switch (Name)
			{
				case "a1":
					SetValues(0, -5, -2, 0, -2, 2, Color.White);
					SetValues(0, -5, -2, 0, -2, -3, Color.Black);
					break;
				case "a2":
					SetValues(.5, -4, -1, -.5, -1, 2, Color.White);
					SetValues(5, -4, -1, .5, -1, -3, Color.Black);
					break;
				case "a3":
					SetValues(.5, -3, -1, -.5, -1, -1, Color.White);
					SetValues(1, -3, -1, -.5, -1, -3, Color.Black);
					break;
				case "a4":
					SetValues(0, -3, -1, -.5, 0, -2, Color.White);
					SetValues(.5, -3, -1, -.5, -.5, -3, Color.Black);
					break;
				case "a5":
					SetValues(.5, -3, -1, -.5, -.5, -3, Color.White);
					SetValues(0, -3, -1, -.5, 0, -2, Color.Black);
					break;
				case "a6":
					SetValues(1, -3, -1, -.5, -1, -3, Color.White);
					SetValues(.5, -3, -1, -.5, -1, -1, Color.Black);
					break;
				case "a7":
					SetValues(5, -4, -1, .5, -1, -3, Color.White);
					SetValues(.5, -4, -1, -.5, -1, 2, Color.Black);
					break;
				case "a8":
					SetValues(0, -5, -2, 0, -2, -3, Color.White);
					SetValues(0, -5, -2, 0, -2, 2, Color.Black);
					break;

				case "b1":
					SetValues(0, -4, -1, 0, -1, 3, Color.White);
					SetValues(0, -4, -1, 0, -1, -4, Color.Black);
					break;
				case "b2":
					SetValues(1, -2, .5, 0, 0, 2, Color.White);
					SetValues(5, -2, 0, 1, 0, -4, Color.Black);
					break;
				case "b3":
					SetValues(-.5, .5, 1, 0, .5, -2, Color.White);
					SetValues(1, 0, 0, 0, 0, -4, Color.Black);
					break;
				case "b4":
					SetValues(0, 0, 0, 0, 0, -3, Color.White);
					SetValues(.5, .5, .5, 0, 0, -4, Color.Black);
					break;
				case "b5":
					SetValues(.5, .5, .5, 0, 0, -4, Color.White);
					SetValues(0, 0, 0, 0, 0, -3, Color.Black);
					break;
				case "b6":
					SetValues(1, 0, 0, 0, 0, -4, Color.White);
					SetValues(-.5, .5, 1, 0, .5, -2, Color.Black);
					break;
				case "b7":
					SetValues(5, -2, 0, 1, 0, -4, Color.White);
					SetValues(1, -2, .5, 0, 0, 2, Color.Black);
					break;
				case "b8":
					SetValues(0, -4, -1, 0, -1, -4, Color.White);
					SetValues(0, -4, -1, 0, -1, 3, Color.Black);
					break;

				case "c1":
					SetValues(0, -3, -1, 0, -1, 1, Color.White);
					SetValues(0, -3, -1, 0, -1, -4, Color.Black);
					break;
				case "c2":
					SetValues(1, 0, 0, 0, .5, 0, Color.White);
					SetValues(5, 0, 0, 1, 0, -4, Color.Black);
					break;
				case "c3":
					SetValues(-1, 1, 1, 0, .5, -2, Color.White);
					SetValues(2, 1, .5, 0, .5, -4, Color.Black);
					break;
				case "c4":
					SetValues(0, 1.5, 1, 0, .5, -3, Color.White);
					SetValues(1, 1.5, .5, 0, .4, -4, Color.Black);
					break;
				case "c5":
					SetValues(1, 1.5, .5, 0, .4, -4, Color.White);
					SetValues(0, 1.5, 1, 0, .5, -3, Color.Black);
					break;
				case "c6":
					SetValues(2, 1, .5, 0, .5, -4, Color.White);
					SetValues(-1, 1, 1, 0, .5, -2, Color.Black);
					break;
				case "c7":
					SetValues(5, 0, 0, 1, 0, -4, Color.White);
					SetValues(1, 0, 0, 0, .5, 0, Color.Black);
					break;
				case "c8":
					SetValues(0, -3, -1, 0, -1, -4, Color.White);
					SetValues(0, -3, -1, 0, -1, 1, Color.Black);
					break;

				case "d1":
					SetValues(0, -3, -1, .5, -.5, 0, Color.White);
					SetValues(0, -3, -1, 0, -.5, -5, Color.Black);
					break;
				case "d2":
					SetValues(-2, .5, 0, 0, 0, 0, Color.White);
					SetValues(5, 0, 0, 1, 0, -5, Color.Black);
					break;
				case "d3":
					SetValues(0, 1.5, 1, 0, .5, -2, Color.White);
					SetValues(3, 1.5, 1, 0, .5, -5, Color.Black);
					break;
				case "d4":
					SetValues(2, 2, 1, 0, .5, -4, Color.White);
					SetValues(2.5, 2, 1, 0, .5, -5, Color.Black);
					break;
				case "d5":
					SetValues(2.5, 2, 1, 0, .5, -5, Color.White);
					SetValues(2, 2, 1, 0, .5, -4, Color.Black);
					break;
				case "d6":
					SetValues(3, 1.5, 1, 0, .5, -5, Color.White);
					SetValues(0, 1.5, 1, 0, .5, -2, Color.Black);
					break;
				case "d7":
					SetValues(5, 0, 0, 1, 0, -5, Color.White);
					SetValues(-2, .5, 0, 0, 0, 0, Color.Black);
					break;
				case "d8":
					SetValues(0, -3, -1, 0, -.5, -5, Color.White);
					SetValues(0, -3, -1, .5, -.5, 0, Color.Black);
					break;

				case "e1":
					SetValues(0, -3, -1, .5, -.5, 0, Color.White);
					SetValues(0, -3, -1, 0, -.5, -5, Color.Black);
					break;
				case "e2":
					SetValues(-2, .5, 0, 0, 0, 0, Color.White);
					SetValues(5, 0, 0, 1, 0, -5, Color.Black);
					break;
				case "e3":
					SetValues(0, 1.5, 1, 0, .5, -2, Color.White);
					SetValues(3, 1.5, 1, 0, .5, -5, Color.Black);
					break;
				case "e4":
					SetValues(2, 2, 1, 0, .5, -4, Color.White);
					SetValues(2.5, 2, 1, 0, .5, -5, Color.Black);
					break;
				case "e5":
					SetValues(2.5, 2, 1, 0, .5, -5, Color.White);
					SetValues(2, 2, 1, 0, .5, -4, Color.Black);
					break;
				case "e6":
					SetValues(3, 1.5, 1, 0, .5, -5, Color.White);
					SetValues(0, 1.5, 1, 0, .5, -2, Color.Black);
					break;
				case "e7":
					SetValues(5, 0, 0, 1, 0, -5, Color.White);
					SetValues(-2, .5, 0, 0, 0, 0, Color.Black);
					break;
				case "e8":
					SetValues(0, -3, -1, 0, -.5, -5, Color.White);
					SetValues(0, -3, -1, .5, -.5, 0, Color.Black);
					break;

				case "f1":
					SetValues(0, -3, -1, 0, -1, 1, Color.White);
					SetValues(0, -3, -1, 0, -1, -4, Color.Black);
					break;
				case "f2":
					SetValues(1, 0, 0, 0, .5, 0, Color.White);
					SetValues(5, 0, 0, 1, 0, -4, Color.Black);
					break;
				case "f3":
					SetValues(-1, 1, 1, 0, .5, -2, Color.White);
					SetValues(2, 1, .5, 0, .5, -4, Color.Black);
					break;
				case "f4":
					SetValues(0, 1.5, 1, 0, .5, -3, Color.White);
					SetValues(1, 1.5, .5, 0, .4, -4, Color.Black);
					break;
				case "f5":
					SetValues(1, 1.5, .5, 0, .4, -4, Color.White);
					SetValues(0, 1.5, 1, 0, .5, -3, Color.Black);
					break;
				case "f6":
					SetValues(2, 1, .5, 0, .5, -4, Color.White);
					SetValues(-1, 1, 1, 0, .5, -2, Color.Black);
					break;
				case "f7":
					SetValues(5, 0, 0, 1, 0, -4, Color.White);
					SetValues(1, 0, 0, 0, .5, 0, Color.Black);
					break;
				case "f8":
					SetValues(0, -3, -1, 0, -1, -4, Color.White);
					SetValues(0, -3, -1, 0, -1, 1, Color.Black);
					break;

				case "g1":
					SetValues(0, -4, -1, 0, -1, 3, Color.White);
					SetValues(0, -4, -1, 0, -1, -4, Color.Black);
					break;
				case "g2":
					SetValues(1, -2, .5, 0, 0, 2, Color.White);
					SetValues(5, -2, 0, 1, 0, -4, Color.Black);
					break;
				case "g3":
					SetValues(-.5, .5, 1, 0, .5, -2, Color.White);
					SetValues(1, 0, 0, 0, 0, -4, Color.Black);
					break;
				case "g4":
					SetValues(0, 0, 0, 0, 0, -3, Color.White);
					SetValues(.5, .5, .5, 0, 0, -4, Color.Black);
					break;
				case "g5":
					SetValues(.5, .5, .5, 0, 0, -4, Color.White);
					SetValues(0, 0, 0, 0, 0, -3, Color.Black);
					break;
				case "g6":
					SetValues(1, 0, 0, 0, 0, -4, Color.White);
					SetValues(-.5, .5, 1, 0, .5, -2, Color.Black);
					break;
				case "g7":
					SetValues(5, -2, 0, 1, 0, -4, Color.White);
					SetValues(1, -2, .5, 0, 0, 2, Color.Black);
					break;
				case "g8":
					SetValues(0, -4, -1, 0, -1, -4, Color.White);
					SetValues(0, -4, -1, 0, -1, 3, Color.Black);
					break;

				case "h1":
					SetValues(0, -5, -2, 0, -2, 2, Color.White);
					SetValues(0, -5, -2, 0, -2, -3, Color.Black);
					break;
				case "h2":
					SetValues(.5, -4, -1, -.5, -1, 2, Color.White);
					SetValues(5, -4, -1, .5, -1, -3, Color.Black);
					break;
				case "h3":
					SetValues(.5, -3, -1, -.5, -1, -1, Color.White);
					SetValues(1, -3, -1, -.5, -1, -3, Color.Black);
					break;
				case "h4":
					SetValues(0, -3, -1, -.5, 0, -2, Color.White);
					SetValues(.5, -3, -1, -.5, -.5, -3, Color.Black);
					break;
				case "h5":
					SetValues(.5, -3, -1, -.5, -.5, -3, Color.White);
					SetValues(0, -3, -1, -.5, 0, -2, Color.Black);
					break;
				case "h6":
					SetValues(1, -3, -1, -.5, -1, -3, Color.White);
					SetValues(.5, -3, -1, -.5, -1, -1, Color.Black);
					break;
				case "h7":
					SetValues(5, -4, -1, .5, -1, -3, Color.White);
					SetValues(.5, -4, -1, -.5, -1, 2, Color.Black);
					break;
				case "h8":
					SetValues(0, -5, -2, 0, -2, -3, Color.White);
					SetValues(0, -5, -2, 0, -2, 2, Color.Black);
					break;

				default:
					break;
			}
		}
	}
}
