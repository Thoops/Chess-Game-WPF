using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChessApp
{
	public class Player
	{

		public Player() { }

		private List<Piece> m_pieces; /**< The pieces that belong to this player.*/
		private List<BoardSquare> m_validSquares; /**< The range of all squares this player's pieces can move to */
		private List<BoardSquare> m_attackingSquares; /**< The range of all squares this player's pieces are attacking */
		private Color m_color; /**< The color of this player.*/
		private List<Piece> m_graveyard; /**< The pieces that this player lost.*/
		private bool m_turn; /**< True if it is this player's turn and otherwise false.*/
		private bool m_check; /**< True if this player is in check.*/
		private List<Move> m_moves; /**< All possible moves thsi player can make.*/

		public List<Piece> Pieces
		{
			get { return m_pieces; }
			set { m_pieces = value; }
		}
		public List<BoardSquare> ValidSquares
		{
			get { return m_validSquares; }
			set { m_validSquares = value; }
		}
		public List<BoardSquare> AttackSquares
		{
			get { return m_attackingSquares; }
			set { m_attackingSquares = value; }
		}
		public Color Color
		{
			get { return m_color; }
			set { m_color = value; }
		}
		public List<Piece> Graveyard
		{
			get { return m_graveyard; }
			set { m_graveyard = value; }
		}
		public bool Turn
		{
			get { return m_turn; }
			set { m_turn = value; }
		}
		public bool Check
		{
			get { return m_check; }
			set { m_check = value; }
		}
		public List<Move> Moves
		{
			get { return m_moves; }
			set { m_moves = value; }
		}

		/** Called when creating the white player. This method sets the Player's
		 * properties and gives it its piece to begin the game. The white player moves 
		 * first.
		 * @author Thomas Hooper
		 * @date March 2019
        */
		private void WhitePlayer()
		{
			this.Color = Color.White;
			this.Pieces = new List<Piece>();
			this.Graveyard = new List<Piece>();
			this.Turn = true;
			this.Check = false;

			Pieces.Add(new Pawn(6, 0, 10, Color.White, "p1"));
			Pieces.Add(new Pawn(6, 1, 10, Color.White, "p2"));
			Pieces.Add(new Pawn(6, 2, 10, Color.White, "p3"));
			Pieces.Add(new Pawn(6, 3, 10, Color.White, "p4"));
			Pieces.Add(new Pawn(6, 4, 10, Color.White, "p5"));
			Pieces.Add(new Pawn(6, 5, 10, Color.White, "p6"));
			Pieces.Add(new Pawn(6, 6, 10, Color.White, "p7"));
			Pieces.Add(new Pawn(6, 7, 10, Color.White, "p8"));

			Pieces.Add(new Rook(7, 0, 50, Color.White, "r1"));
			Pieces.Add(new Knight(7, 1, 30, Color.White, "n1"));
			Pieces.Add(new Bishop(7, 2, 30, Color.White, "b1"));
			Pieces.Add(new Queen(7, 3, 90, Color.White, "q"));
			Pieces.Add(new King(7, 4, 900, Color.White, "k"));
			Pieces.Add(new Bishop(7, 5, 30, Color.White, "b2"));
			Pieces.Add(new Knight(7, 6, 30, Color.White, "n2"));
			Pieces.Add(new Rook(7, 7, 50, Color.White, "r2"));
		}

		/** Called when creating the black player. This method sets the Player's
		 * properties and gives it its piece to begin the game. The black player
		 * moves second.
		 * @author Thomas Hooper
		 * @date March 2019
        */
		private void BlackPlayer()
		{
			this.Color = Color.Black;
			this.Pieces = new List<Piece>();
			this.Graveyard = new List<Piece>();
			this.Turn = false;
			this.Check = false;
			Pieces.Add(new Pawn(1, 0, 10, Color.Black, "p1"));
			Pieces.Add(new Pawn(1, 1, 10, Color.Black, "p2"));
			Pieces.Add(new Pawn(1, 2, 10, Color.Black, "p3"));
			Pieces.Add(new Pawn(1, 3, 10, Color.Black, "p4"));
			Pieces.Add(new Pawn(1, 4, 10, Color.Black, "p5"));
			Pieces.Add(new Pawn(1, 5, 10, Color.Black, "p6"));
			Pieces.Add(new Pawn(1, 6, 10, Color.Black, "p7"));
			Pieces.Add(new Pawn(1, 7, 10, Color.Black, "p8"));

			Pieces.Add(new Rook(0, 0, 50, Color.Black, "r1"));
			Pieces.Add(new Knight(0, 1, 30, Color.Black, "n1"));
			Pieces.Add(new Bishop(0, 2, 30, Color.Black, "b1"));
			Pieces.Add(new Queen(0, 3, 90, Color.Black, "q"));
			Pieces.Add(new King(0, 4, 900, Color.Black, "k"));
			Pieces.Add(new Bishop(0, 5, 30, Color.Black, "b2"));
			Pieces.Add(new Knight(0, 6, 30, Color.Black, "n2"));
			Pieces.Add(new Rook(0, 7, 50, Color.Black, "r2"));
		}

		/** Constructor For Player. If the color passed in is white
		 * then it calls WhitePlayer(). Else it calls BlackPlayer().
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Player(Color a_color)
		{
			ValidSquares = new List<BoardSquare>();
			AttackSquares = new List<BoardSquare>();
			if (a_color == Color.White)
			{
				WhitePlayer();
			}
			else if(a_color == Color.Black)
			{
				BlackPlayer();
			}
		}

		/** This method generates all the possible moves the player is able to make
		 * It goes through each piece and their valid squares and determines if it can
		 * make each move for each piece and square. It also evaluates if the piece can capture,
		 * castle or en passant
		 * @param a_player - The opposing player
		 * @param a_board - The board the game is played on
		 * @param a_previousMove - the previous move that was played
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public void GenerateMoves(Player a_player, Board a_board, Move a_previousMove)
		{
			List<Move> moves = new List<Move>();
			foreach(Piece p in Pieces)
			{
				foreach (BoardSquare s in p.ValidSquares)
				{
					#region Just Moving
					if (!s.Occupied)
					{
						moves.Add(new Move(s, p));
					}
					#endregion
					#region Capturing
					else
					{
						if(a_player.Pieces.Exists(x => x.Row == s.Row && x.Column == s.Column)) //BoardSquare Color
						{
							if (p.AttackingSquares.Exists(x => x.Row == s.Row && x.Column == s.Column) && s.PieceColor != p.Color)
							{
								Piece capturedPiece = a_player.Pieces.Find(x => x.Row == s.Row && x.Column == s.Column);
								moves.Add(new Move(s, p, capturedPiece));
							}
						}
					}
					#endregion
				}
			}

			#region En Passant
			if (a_previousMove != null)
			{
				if (a_previousMove.MovingPiece is Pawn && (a_previousMove.MovingPiece.Row == 3 || a_previousMove.MovingPiece.Row == 4))
				{
					if (Pieces.Exists(x => x is Pawn && x.Row == a_previousMove.MovingPiece.Row))
					{
						if (a_previousMove.MovingPiece.Color == Color.White && a_previousMove.MovingPiece.Row == 4)
						{
							if (a_previousMove.OriginalSquare.Row == 6)
							{
								List<Piece> pawns = Pieces.FindAll(x => x.Row == 4);
								foreach (Piece p in pawns)
								{
									if (Math.Abs(p.Column - a_previousMove.MovingPiece.Column) == 1)
									{
										BoardSquare square = a_board.ReturnSquare(a_previousMove.OriginalSquare.Row + 1, a_previousMove.OriginalSquare.Column);
										moves.Add(new Move(square, p) { EnPassant = true });
									}
								}
							}
						}
						else if (a_previousMove.MovingPiece.Color == Color.Black && a_previousMove.MovingPiece.Row == 3)
						{
							if (a_previousMove.OriginalSquare.Row == 1)
							{
								List<Piece> pawns = Pieces.FindAll(x => x.Row == 3);
								foreach (Piece p in pawns)
								{
									if (Math.Abs(p.Column - a_previousMove.MovingPiece.Column) == 1)
									{
										BoardSquare square = a_board.ReturnSquare(a_previousMove.OriginalSquare.Row - 1, a_previousMove.OriginalSquare.Column);
										moves.Add(new Move(square, p) { EnPassant = true });
									}
								}
							}
						}
					}
				}
			}
			#endregion
			#region Castling
			King k = (King)Pieces.Find(x => x is King);
			if (!k.HasMoved && !Check)
			{
				BoardSquare s1 = new BoardSquare();
				BoardSquare s2 = new BoardSquare();
				BoardSquare s3 = new BoardSquare();
				if (Pieces.Exists(x => x is Rook && !x.HasMoved && x.Column == 7))
				{
					Rook r = (Rook)Pieces.Find(x => x is Rook && !x.HasMoved && x.Column == 7);
					s1 = a_board.ReturnSquare(k.Row, k.Column + 1);
					s2 = a_board.ReturnSquare(k.Row, k.Column + 2);
					if (!s1.Occupied && !s2.Occupied)
					{
						moves.Add(new Move(s2, k, r, true));
					}
				}
				if (Pieces.Exists(x => x is Rook && !x.HasMoved && x.Column == 0))
				{
					Rook r = (Rook)Pieces.Find(x => x is Rook && !x.HasMoved && x.Column == 0);
					s1 = a_board.ReturnSquare(k.Row, k.Column - 1);
					s2 = a_board.ReturnSquare(k.Row, k.Column - 2);
					s3 = a_board.ReturnSquare(k.Row, k.Column - 3);

					if (!s1.Occupied && !s2.Occupied && !s3.Occupied)
					{
						moves.Add(new Move(s2, k, r, true));
					}
				}
			}
			#endregion
			Moves = moves;
		}

		/** This method gives a value to the move we pass in. It will add or subtract value
		 * based on the square it is moving to and how valuable it is for the piece that
		 * is moving. If it is a capture move then it will add the value of the enemy's piece
		 * to the move. Extra points are added if the piece has not yet moved or if it is a castle
		 * @param a_move - The move we are rating.
		 * @param a_enemy - The opposing player
		 * @param a_previousMove - the previous move that was played
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public void RateMove(Move a_move, Player a_enemy, Move a_previousMove)
		{
			RateMoveSquare(a_move);
			a_move.Value *= 10;
			int piecesValue = 0;
			int enemyPiecesValue = 0;
			this.Pieces.ForEach(x => piecesValue += x.Value);
			a_enemy.Pieces.ForEach(x => enemyPiecesValue += x.Value);
			if (a_move.Capture)
			{
				enemyPiecesValue -= a_move.CapturedPiece.Value;
			}
			else if (a_move.EnPassant)
			{
				enemyPiecesValue -= a_previousMove.MovingPiece.Value;
			}

			else if (a_move.Castle)
			{
				a_move.Value += 20;
			}

			if (!a_move.MovingPiece.HasMoved)
			{
				a_move.Value += 20;
			}
			int valueOfPieces = piecesValue - enemyPiecesValue;
			a_move.Value += valueOfPieces;
		}

		/** Takes a move and adds value to it based on if the square it is moving to is valuable
		 * @param a_move - The move we are rating based off of how valuable the square it moves to is
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public void RateMoveSquare(Move a_move)
		{
			if (a_move.MovingPiece.Color == Color.White)
			{
				if (a_move.MovingPiece is Pawn)
				{
					a_move.Value += a_move.Destination.WhitePawnValue;
				}
				else if (a_move.MovingPiece is Knight)
				{
					a_move.Value += a_move.Destination.WhiteKnightValue;
				}
				else if (a_move.MovingPiece is Bishop)
				{
					a_move.Value += a_move.Destination.WhiteBishopValue;
				}
				else if (a_move.MovingPiece is Rook)
				{
					a_move.Value += a_move.Destination.WhiteRookValue;
				}
				else if (a_move.MovingPiece is Queen)
				{
					a_move.Value += a_move.Destination.WhiteQueenValue;
				}
				else if (a_move.MovingPiece is King)
				{
					a_move.Value += a_move.Destination.WhiteKingValue;
				}
			}
			else if (a_move.MovingPiece.Color == Color.Black)
			{
				if (a_move.MovingPiece is Pawn)
				{
					a_move.Value += a_move.Destination.BlackPawnValue;
				}
				else if (a_move.MovingPiece is Knight)
				{
					a_move.Value += a_move.Destination.BlackKnightValue;
				}
				else if (a_move.MovingPiece is Bishop)
				{
					a_move.Value += a_move.Destination.BlackBishopValue;
				}
				else if (a_move.MovingPiece is Rook)
				{
					a_move.Value += a_move.Destination.BlackRookValue;
				}
				else if (a_move.MovingPiece is Queen)
				{
					a_move.Value += a_move.Destination.BlackQueenValue;
				}
				else if (a_move.MovingPiece is King)
				{
					a_move.Value += a_move.Destination.BlackKingValue;
				}
			}
		}

		/** This is called after every turn to add to the history of each piece.
		 * The history shows where the move was on each turn.
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public void UpdateHistory()
		{
			foreach(Piece p in Pieces)
			{
				p.History.Add(p.Square.Name);
			}
		}

		/** This takes a string and returns the Player's piece that's name equals the string
		 * @param a_pieceString - The string we are matching to a piece
		 * @returns The piece that's name matches the string
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public Piece ReturnPieceFromString(string a_pieceString)
		{
			return Pieces.Find(x => x.Name.Equals(a_pieceString));
		}
	}
}