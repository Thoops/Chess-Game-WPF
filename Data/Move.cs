using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ChessApp
{
	/// <summary>
	/// This is where a Move object is created. A Move consists of at least a piece and a destination square
	/// </summary>
	[KnownType(typeof(Pawn))]
	[KnownType(typeof(Knight))]
	[KnownType(typeof(Bishop))]
	[KnownType(typeof(Rook))]
	[KnownType(typeof(Queen))]
	[KnownType(typeof(King))]
	public class Move
	{
		/** Empty Constructor for serialization purposes
        */
		public Move() { }

		private BoardSquare m_ogSquare; /**< The original square the moving piece was on */
		private BoardSquare m_dest; /**< The square the moving piece is going to */
		private Piece m_movingPiece; /**< The piece that is being moved */
		private Piece m_capturedPiece; /**< The piece that is being captured */
		private Piece m_castlingRook; /**< The rook that is being castled with */
		private bool m_castle; /**< If true then the move is a castle */
		private bool m_enPassant; /**< If true then the move is an En Passant */
		private bool m_capture; /**< If true then the move is a capture */
		private double m_value; /**< The value of the move */

		
		public BoardSquare OriginalSquare
		{
			get { return m_ogSquare; }
			set { m_ogSquare = value; }
		}
		
		public BoardSquare Destination
		{
			get { return m_dest; }
			set { m_dest = value; }
		}
		
		public Piece MovingPiece
		{
			get { return m_movingPiece; }
			set { m_movingPiece = value; }
		}
		
		public Piece CapturedPiece
		{
			get { return m_capturedPiece; }
			set { m_capturedPiece = value; }
		}
		
		public Piece CastlingRook
		{
			get { return m_castlingRook; }
			set { m_castlingRook = value; }
		}
		
		public bool Castle
		{
			get { return m_castle; }
			set { m_castle = value; }
		}
		
		public bool EnPassant
		{
			get { return m_enPassant; }
			set { m_enPassant = value; }
		}
		
		public bool Capture
		{
			get { return m_capture; }
			set { m_capture = value; }
		}
		
		public double Value
		{
			get { return m_value; }
			set { m_value = value; }
		}

		/** Constructor for Move that is used when the piece is moving and
		 * not doing anything else.
		 * @param a_square - The square the piece is moving to.
		 * @param a_piece - The piece that is moving.
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public Move(BoardSquare a_square, Piece a_piece)
		{
			Destination = a_square;
			MovingPiece = a_piece;
			OriginalSquare = a_piece.Square;
			Castle = false;
			EnPassant = false;
			Capture = false;
			Value = 0;
		}

		/** Constructor for Move that is used when the piece is capturing another piece.
		 * @param a_square - The square the piece is moving to.
		 * @param a_piece - The piece that is moving.
		 * @param a_capturedPiece - The piece being captured.
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public Move(BoardSquare a_square, Piece a_piece, Piece a_capturedPiece)
		{
			Destination = a_square;
			MovingPiece = a_piece;
			OriginalSquare = a_piece.Square;
			CapturedPiece = a_capturedPiece;
			Castle = false;
			EnPassant = false;
			Capture = true;
			Value = 0;
		}

		/** Constructor for Move that is used when the piece is capturing another piece.
		 * @param a_square - The square the king is moving to.
		 * @param a_piece - The king that is castling.
		 * @param a_rook - The rook taking part in the castle
		 * @param a_castle - This signifies we are castling
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public Move(BoardSquare a_square, Piece a_piece, Piece a_rook, bool a_castle)
		{
			Destination = a_square;
			MovingPiece = a_piece;
			OriginalSquare = a_piece.Square;
			CastlingRook = a_rook;
			Castle = true;
			EnPassant = false;
			Capture = false;
			Value = 0;
		}

		/** Method used to turn a move into its string representation
		 * I use this for connected games to send this string 
		 * over a socket
		 * @param a_move - The move we want the string representation of
		 * @returns The string representation of the move being passed in
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public static string ReturnStringFromMove(Move a_move)
		{
			string moveString = "";
			moveString += a_move.MovingPiece.Name + ", ";
			moveString += a_move.Destination.Name + ", ";
			if(a_move.MovingPiece.Color == Color.White)
			{
				moveString += "White";
			}
			else
			{
				moveString += "Black";
			}

			if (a_move.Capture)
			{
				moveString += ", Capture, " + a_move.CapturedPiece.Name;
			}
			else if (a_move.Castle)
			{
				moveString += ", Castle, " + a_move.CastlingRook.Name;
			}
			else if (a_move.EnPassant)
			{
				moveString += ", EnPassant, " + a_move.CapturedPiece.Name;
			}
			return moveString;
		}
	}
}