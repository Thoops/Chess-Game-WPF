using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp
{ 
	/// <summary>
	/// This class is our chess board. It holds 64 BoardSquares
	/// </summary>
	public class Board
	{
		/** Empty Constructor for serialization purposes
        */
		public Board() { }

		private List<BoardSquare> m_chessBoard; /**< The list of squares that make up our board */
		public List<BoardSquare> ChessBoard
		{
			get { return this.m_chessBoard; }
			set { this.m_chessBoard = value; }
		}

		private Move m_lastMove; /**< The last move that was played*/
		public Move LastMove
		{
			get { return m_lastMove; }
			set { m_lastMove = value; }
		}

		/** Takes the row and column and returns the square that the 
		 * passed in row and column are on.
		 * @param a_row - The row the square is on.
		 * @param a_column - The column the square is on.
		 * @returns The square that the passed in row and column are on.
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public BoardSquare ReturnSquare(int a_row, int a_column)
		{
			try
			{
				return ChessBoard.Find(square => square.Row == a_row && square.Column == a_column);
			}
			catch (ArgumentNullException)
			{
				return null;
			}
		}

		/** Assigns the squares to the pieces of the players passed in
		 * Each piece has a square and this is where the get one
		 * @param a_whitePlayer - The white player in the game
		 * @param a_blackPlayer - The black player in the game
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public void AssignSquares(Player a_whitePlayer, Player a_blackPlayer)
		{
			foreach(Piece p in a_whitePlayer.Pieces)
			{
				p.Square = ChessBoard.Find(x => x.Row == p.Row && x.Column == p.Column);
			}
			foreach (Piece p in a_blackPlayer.Pieces)
			{
				p.Square = ChessBoard.Find(x => x.Row == p.Row && x.Column == p.Column);
			}
		}

		/** Constructor for Board that take the two players and creates
		 * the board to begin the game
		 * @param a_WhitePlayer - The white player in the game
		 * @param a_BlackPlayer - The black player in the game
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Board(Player a_WhitePlayer, Player a_BlackPlayer)
		{
			LastMove = new Move();
			ChessBoard = new List<BoardSquare>();

			#region Adding Squares to Chess Board

			ChessBoard.Add(new BoardSquare(0, 0, 0, "a8", a_BlackPlayer.Pieces[8]));
			ChessBoard.Add(new BoardSquare(1, 0, 0, "a7", a_BlackPlayer.Pieces[0]));
			ChessBoard.Add(new BoardSquare(2, 0, 0, "a6"));
			ChessBoard.Add(new BoardSquare(3, 0, 0, "a5"));
			ChessBoard.Add(new BoardSquare(4, 0, 0, "a4"));
			ChessBoard.Add(new BoardSquare(5, 0, 0, "a3"));
			ChessBoard.Add(new BoardSquare(6, 0, 0, "a2", a_WhitePlayer.Pieces[0]));
			ChessBoard.Add(new BoardSquare(7, 0, 0, "a1", a_WhitePlayer.Pieces[8]));

			ChessBoard.Add(new BoardSquare(0, 1, 0, "b8", a_BlackPlayer.Pieces[9]));
			ChessBoard.Add(new BoardSquare(1, 1, 0, "b7", a_BlackPlayer.Pieces[1]));
			ChessBoard.Add(new BoardSquare(2, 1, 0, "b6"));
			ChessBoard.Add(new BoardSquare(3, 1, 0, "b5"));
			ChessBoard.Add(new BoardSquare(4, 1, 0, "b4"));
			ChessBoard.Add(new BoardSquare(5, 1, 0, "b3"));
			ChessBoard.Add(new BoardSquare(6, 1, 0, "b2", a_WhitePlayer.Pieces[1]));
			ChessBoard.Add(new BoardSquare(7, 1, 0, "b1", a_WhitePlayer.Pieces[9]));

			ChessBoard.Add(new BoardSquare(0, 2, 0, "c8", a_BlackPlayer.Pieces[10]));
			ChessBoard.Add(new BoardSquare(1, 2, 0, "c7", a_BlackPlayer.Pieces[2]));
			ChessBoard.Add(new BoardSquare(2, 2, 0, "c6"));
			ChessBoard.Add(new BoardSquare(3, 2, 0, "c5"));
			ChessBoard.Add(new BoardSquare(4, 2, 0, "c4"));
			ChessBoard.Add(new BoardSquare(5, 2, 0, "c3"));
			ChessBoard.Add(new BoardSquare(6, 2, 0, "c2", a_WhitePlayer.Pieces[2]));
			ChessBoard.Add(new BoardSquare(7, 2, 0, "c1", a_WhitePlayer.Pieces[10]));

			ChessBoard.Add(new BoardSquare(0, 3, 0, "d8", a_BlackPlayer.Pieces[11]));
			ChessBoard.Add(new BoardSquare(1, 3, 0, "d7", a_BlackPlayer.Pieces[3]));
			ChessBoard.Add(new BoardSquare(2, 3, 0, "d6"));
			ChessBoard.Add(new BoardSquare(3, 3, 0, "d5"));
			ChessBoard.Add(new BoardSquare(4, 3, 0, "d4"));
			ChessBoard.Add(new BoardSquare(5, 3, 0, "d3"));
			ChessBoard.Add(new BoardSquare(6, 3, 0, "d2", a_WhitePlayer.Pieces[3]));
			ChessBoard.Add(new BoardSquare(7, 3, 0, "d1", a_WhitePlayer.Pieces[11]));

			ChessBoard.Add(new BoardSquare(0, 4, 0, "e8", a_BlackPlayer.Pieces[12]));
			ChessBoard.Add(new BoardSquare(1, 4, 0, "e7", a_BlackPlayer.Pieces[4]));
			ChessBoard.Add(new BoardSquare(2, 4, 0, "e6"));
			ChessBoard.Add(new BoardSquare(3, 4, 0, "e5"));
			ChessBoard.Add(new BoardSquare(4, 4, 0, "e4"));
			ChessBoard.Add(new BoardSquare(5, 4, 0, "e3"));
			ChessBoard.Add(new BoardSquare(6, 4, 0, "e2", a_WhitePlayer.Pieces[4]));
			ChessBoard.Add(new BoardSquare(7, 4, 0, "e1", a_WhitePlayer.Pieces[12]));

			ChessBoard.Add(new BoardSquare(0, 5, 0, "f8", a_BlackPlayer.Pieces[13]));
			ChessBoard.Add(new BoardSquare(1, 5, 0, "f7", a_BlackPlayer.Pieces[5]));
			ChessBoard.Add(new BoardSquare(2, 5, 0, "f6"));
			ChessBoard.Add(new BoardSquare(3, 5, 0, "f5"));
			ChessBoard.Add(new BoardSquare(4, 5, 0, "f4"));
			ChessBoard.Add(new BoardSquare(5, 5, 0, "f3"));
			ChessBoard.Add(new BoardSquare(6, 5, 0, "f2", a_WhitePlayer.Pieces[5]));
			ChessBoard.Add(new BoardSquare(7, 5, 0, "f1", a_WhitePlayer.Pieces[13]));

			ChessBoard.Add(new BoardSquare(0, 6, 0, "g8", a_BlackPlayer.Pieces[14]));
			ChessBoard.Add(new BoardSquare(1, 6, 0, "g7", a_BlackPlayer.Pieces[6]));
			ChessBoard.Add(new BoardSquare(2, 6, 0, "g6"));
			ChessBoard.Add(new BoardSquare(3, 6, 0, "g5"));
			ChessBoard.Add(new BoardSquare(4, 6, 0, "g4"));
			ChessBoard.Add(new BoardSquare(5, 6, 0, "g3"));
			ChessBoard.Add(new BoardSquare(6, 6, 0, "g2", a_WhitePlayer.Pieces[6]));
			ChessBoard.Add(new BoardSquare(7, 6, 0, "g1", a_WhitePlayer.Pieces[14]));

			ChessBoard.Add(new BoardSquare(0, 7, 0, "h8", a_BlackPlayer.Pieces[15]));
			ChessBoard.Add(new BoardSquare(1, 7, 0, "h7", a_BlackPlayer.Pieces[7]));
			ChessBoard.Add(new BoardSquare(2, 7, 0, "h6"));
			ChessBoard.Add(new BoardSquare(3, 7, 0, "h5"));
			ChessBoard.Add(new BoardSquare(4, 7, 0, "h4"));
			ChessBoard.Add(new BoardSquare(5, 7, 0, "h3"));
			ChessBoard.Add(new BoardSquare(6, 7, 0, "h2", a_WhitePlayer.Pieces[7]));
			ChessBoard.Add(new BoardSquare(7, 7, 0, "h1", a_WhitePlayer.Pieces[15]));
			#endregion
		}

		/** Returns the square that's name equals the string passed in
		 * @param a_squareString - the name of the square we want to get
		 * @returns The square that matches the name of the parameter
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public BoardSquare ReturnSquareFromString(string a_squareString)
		{
			return ChessBoard.Find(x => x.Name.Equals(a_squareString));
		}
	}
}
