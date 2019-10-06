using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;


namespace ChessApp
{
	/// <summary>
	/// This is where all the game logic is
	/// </summary>
	[Serializable, XmlRoot("ChessApp")]
	public class Game : INotifyPropertyChanged
	{
		//For Serialization Purposes
		public Game() { }
		#region Each White Piece Used For Data Binding
		public Piece WP1 { get; set; }
		public Piece WP2 { get; set; }
		public Piece WP3 { get; set; }
		public Piece WP4 { get; set; }
		public Piece WP5 { get; set; }
		public Piece WP6 { get; set; }
		public Piece WP7 { get; set; }
		public Piece WP8 { get; set; }
		public Piece WR1 { get; set; }
		public Piece WN1 { get; set; }
		public Piece WB1 { get; set; }
		public Piece WQ { get; set; }
		public Piece WK { get; set; }
		public Piece WB2 { get; set; }
		public Piece WN2 { get; set; }
		public Piece WR2 { get; set; }
		#endregion
		#region Each Black Piece Used For Data Binding
		public Piece BP1 { get; set; }
		public Piece BP2 { get; set; }
		public Piece BP3 { get; set; }
		public Piece BP4 { get; set; }
		public Piece BP5 { get; set; }
		public Piece BP6 { get; set; }
		public Piece BP7 { get; set; }
		public Piece BP8 { get; set; }
		public Piece BR1 { get; set; }
		public Piece BN1 { get; set; }
		public Piece BB1 { get; set; }
		public Piece BQ { get; set; }
		public Piece BK { get; set; }
		public Piece BB2 { get; set; }
		public Piece BN2 { get; set; }
		public Piece BR2 { get; set; }
		#endregion

		#region Properties
		private Color m_turn; /**< The color of ther player whose turn it is */
		private Player m_whitePlayer; /**< The player with the white pieces who goes first */
		private Player m_blackPlayer; /**< The player with the black piece who goes second */
		private Board m_board; /**< The chess board of our game */
		private Move m_currentMove; /**< The current move that is being played */
		private Move m_previousMove; /**< The move that was played in the previous round */
		private bool m_checkmate; /**< True if a player is in checkmate and if so then it is game over */
		private bool m_draw; /**< True if the game is a draw */

		public Player WhitePlayer
		{
			get { return m_whitePlayer; }
			set { m_whitePlayer = value; }
		}
		public Player BlackPlayer
		{
			get { return m_blackPlayer; }
			set { m_blackPlayer = value; }
		}
		public Player HumanPlayer { get; set; }
		public Player ComputerPlayer { get; set; }
		public Player MovingPlayer { get; set; }
		public Board ChessBoard
		{
			get { return m_board; }
			set { m_board = value; }
		}
		public Color Turn
		{
			get { return m_turn; }
			set
			{
				m_turn = value;
			}
		}
		public Piece SelectedPiece { get; set; }
		public Move CurrentMove
		{
			get { return m_currentMove; }
			set { m_currentMove = value; }
		}
		public Move PreviousMove
		{
			get { return m_previousMove; }
			set { m_previousMove = value; }
		}
		public bool Checkmate
		{
			get { return m_checkmate; }
			set { m_checkmate = value; }
		}
		public bool Draw
		{
			get { return m_draw; }
			set { m_draw = value; }
		}
		private string m_moveString; /**< The string that represents the game that has been played so far */
		public string MoveString
		{
			get { return m_moveString; }
			set
			{
				m_moveString = value;
				NotifyPropertyChanged("MoveString");
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

		#region Methods for Starting Game And Constructors: Game, GameLoaded, SetPieceProperties
		/** Constructor for Game when you start a new game against the computer
		 * @param a_color - the color of the pieces of the human player
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public Game(Color a_humanColor)
		{
			WhitePlayer = new Player(Color.White);
			BlackPlayer = new Player(Color.Black);
			ChessBoard = new Board(WhitePlayer, BlackPlayer);
			ChessBoard.AssignSquares(WhitePlayer, BlackPlayer);
			MovingPlayer = WhitePlayer;
			if (a_humanColor == Color.White)
			{
				HumanPlayer = WhitePlayer;
				ComputerPlayer = BlackPlayer;
			}
			else if (a_humanColor == Color.Black)
			{
				HumanPlayer = BlackPlayer;
				ComputerPlayer = WhitePlayer;
			}
			SetPieceProperties();//For data binding
			PreviousMove = new Move();
			CurrentMove = new Move();
			Turn = Color.White;
			CalculateValidSquares();
			SelectedPiece = null;
			Checkmate = false;
			Draw = false;
		}

		/** Constructor for Game when you want to play against yourself
		 * or a friend over a connection.
		 * @param a_true - bool to signify we want to play against a friend or ourself
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public Game(bool a_true)
		{
			WhitePlayer = new Player(Color.White);
			BlackPlayer = new Player(Color.Black);
			ChessBoard = new Board(WhitePlayer, BlackPlayer);
			ChessBoard.AssignSquares(WhitePlayer, BlackPlayer);
			MovingPlayer = WhitePlayer;
			#region White Pieces For Binding
			WP1 = WhitePlayer.Pieces[0];
			WP2 = WhitePlayer.Pieces[1];
			WP3 = WhitePlayer.Pieces[2];
			WP4 = WhitePlayer.Pieces[3];
			WP5 = WhitePlayer.Pieces[4];
			WP6 = WhitePlayer.Pieces[5];
			WP7 = WhitePlayer.Pieces[6];
			WP8 = WhitePlayer.Pieces[7];

			WR1 = WhitePlayer.Pieces[8];
			WN1 = WhitePlayer.Pieces[9];
			WB1 = WhitePlayer.Pieces[10];
			WQ = WhitePlayer.Pieces[11];
			WK = WhitePlayer.Pieces[12];
			WB2 = WhitePlayer.Pieces[13];
			WN2 = WhitePlayer.Pieces[14];
			WR2 = WhitePlayer.Pieces[15];
			#endregion
			#region Black Pieces For Binding
			BP1 = BlackPlayer.Pieces[0];
			BP2 = BlackPlayer.Pieces[1];
			BP3 = BlackPlayer.Pieces[2];
			BP4 = BlackPlayer.Pieces[3];
			BP5 = BlackPlayer.Pieces[4];
			BP6 = BlackPlayer.Pieces[5];
			BP7 = BlackPlayer.Pieces[6];
			BP8 = BlackPlayer.Pieces[7];

			BR1 = BlackPlayer.Pieces[8];
			BN1 = BlackPlayer.Pieces[9];
			BB1 = BlackPlayer.Pieces[10];
			BQ = BlackPlayer.Pieces[11];
			BK = BlackPlayer.Pieces[12];
			BB2 = BlackPlayer.Pieces[13];
			BN2 = BlackPlayer.Pieces[14];
			BR2 = BlackPlayer.Pieces[15];
			#endregion
			PreviousMove = new Move();
			CurrentMove = new Move();
			Turn = Color.White;
			CalculateValidSquares();
			SelectedPiece = null;
			Checkmate = false;
			Draw = false;
		}

		/** It is like the constructors of this class but this is called when
		 * we have loaded a game from a save file. It copies into our game all
		 * of the properties of the game being passed in
		 * @param a_game - the game we have loaded from a save file
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void GameLoaded(Game a_game)
		{
			WhitePlayer = a_game.WhitePlayer;
			BlackPlayer = a_game.BlackPlayer;
			ChessBoard = a_game.ChessBoard;
			ChessBoard.AssignSquares(WhitePlayer, BlackPlayer);
			Turn = a_game.Turn;
			if(Turn == Color.White) { MovingPlayer = WhitePlayer; }
			else { MovingPlayer = BlackPlayer; }
			if(a_game.HumanPlayer.Color == Color.White)
			{
				HumanPlayer = WhitePlayer;
				ComputerPlayer = BlackPlayer;
			}
			else
			{
				HumanPlayer = BlackPlayer;
				ComputerPlayer = WhitePlayer;
			}
			SetPieceProperties();
			PreviousMove = a_game.PreviousMove;
			CurrentMove = a_game.CurrentMove;
			CalculateValidSquares();
			SelectedPiece = null;
			Checkmate = a_game.Checkmate;
			Draw = a_game.Draw;
		}

		/** We use this method to have our properties for each piece be a reference
		 * to the piece objects of the players. We do this because these properties
		 * are used for data binding the view models in our ChessBoard control to the
		 * row and column properties of their respective pieces.
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public void SetPieceProperties()
		{
			foreach (Piece p in WhitePlayer.Pieces)
			{
				switch (p.Name)
				{
					case "p1":
						WP1 = p;
						break;
					case "p2":
						WP2 = p;
						break;
					case "p3":
						WP3 = p;
						break;
					case "p4":
						WP4 = p;
						break;
					case "p5":
						WP5 = p;
						break;
					case "p6":
						WP6 = p;
						break;
					case "p7":
						WP7 = p;
						break;
					case "p8":
						WP8 = p;
						break;
					case "r1":
						WR1 = p;
						break;
					case "n1":
						WN1 = p;
						break;
					case "b1":
						WB1 = p;
						break;
					case "q":
						WQ = p;
						break;
					case "k":
						WK = p;
						break;
					case "b2":
						WB2 = p;
						break;
					case "n2":
						WN2 = p;
						break;
					case "r2":
						WR2 = p;
						break;
				}
			}
			foreach (Piece p in BlackPlayer.Pieces)
			{
				switch (p.Name)
				{
					case "p1":
						BP1 = p;
						break;
					case "p2":
						BP2 = p;
						break;
					case "p3":
						BP3 = p;
						break;
					case "p4":
						BP4 = p;
						break;
					case "p5":
						BP5 = p;
						break;
					case "p6":
						BP6 = p;
						break;
					case "p7":
						BP7 = p;
						break;
					case "p8":
						BP8 = p;
						break;
					case "r1":
						BR1 = p;
						break;
					case "n1":
						BN1 = p;
						break;
					case "b1":
						BB1 = p;
						break;
					case "q":
						BQ = p;
						break;
					case "k":
						BK = p;
						break;
					case "b2":
						BB2 = p;
						break;
					case "n2":
						BN2 = p;
						break;
					case "r2":
						BR2 = p;
						break;
				}
			}
		}
		#endregion

		#region Functions called after a turn: ChangeTurns, CalculateValidSquares, CheckForCheckmate, CheckForStalemate, CheckForRepetition
		/** This is called after the end of every turn. It calculates the squares for each player's pieces.
		 * Then it changes the turn to be the color opposite the player that just moved. It also generates
		 * the moves for the player that is about to move. If a player is in check then it also checks for
		 * checkmate.
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public void ChangeTurns()
		{
			PreviousMove = CurrentMove;//This was after calcvalidsquares b4 so idk why i did that
			CalculateValidSquares();//This calculates the valid squares for all the pieces both players have.
			#region Changing Turns and Updating History and Generating Moves
			if (Turn == Color.White)
			{
				Turn = Color.Black;
				MovingPlayer = BlackPlayer;
				WhitePlayer.UpdateHistory();
				BlackPlayer.GenerateMoves(BlackPlayer, ChessBoard, PreviousMove);
			}
			else
			{
				Turn = Color.White;
				MovingPlayer = WhitePlayer;
				BlackPlayer.UpdateHistory();
				WhitePlayer.GenerateMoves(WhitePlayer, ChessBoard, PreviousMove);
			}
			#endregion
			#region Checking for check and checkmate
			if (WhitePlayer.Check)
			{
				Game game = SerializeGame();
				if (game.CheckForCheckmate())//Might cause problems
				{
					Checkmate = true;
				}
			}
			else if (BlackPlayer.Check)
			{
				Game game = SerializeGame();
				if (game.CheckForCheckmate())//Might cause problems
				{
					Checkmate = true;
				}
			}
			#endregion

			#region Checking For Draws
			/*
			if (!Checkmate)
			{
				if (CheckForStalemate(MovingPlayer))
				{
					Draw = true;
				}
				else if (CheckForRepetition())
				{
					Draw = true;
				}
			}
			*/
			#endregion
		}

		/** This method clears the valid squares of both players and resets the squares for each
		 * piece of both players after a round. It also gets the squares the king is unable to move
		 * because the king is not allowed to move into check.
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public void CalculateValidSquares()
		{
			WhitePlayer.ValidSquares.Clear();
			WhitePlayer.AttackSquares.Clear();
			BlackPlayer.ValidSquares.Clear();
			BlackPlayer.AttackSquares.Clear();

			foreach (Piece p in WhitePlayer.Pieces)
			{
				p.SetValidSquares(ChessBoard);
				WhitePlayer.ValidSquares.AddRange(p.ValidSquares);
				WhitePlayer.AttackSquares.AddRange(p.AttackingSquares);
			}
			foreach (Piece p in BlackPlayer.Pieces)
			{
				p.SetValidSquares(ChessBoard);
				BlackPlayer.ValidSquares.AddRange(p.ValidSquares);
				BlackPlayer.AttackSquares.AddRange(p.AttackingSquares);
			}
			King wk = (King)WhitePlayer.Pieces.Find(x => x is King);
			King bk = (King)BlackPlayer.Pieces.Find(x => x is King);

			wk.SetInvalidSquares(BlackPlayer);
			bk.SetInvalidSquares(WhitePlayer);

			if (BlackPlayer.AttackSquares.Exists(x => x.Row == wk.Row && x.Column == wk.Column))
			{
				WhitePlayer.Check = true;
			}
			else
			{
				WhitePlayer.Check = false;
			}
			if (WhitePlayer.AttackSquares.Exists(x => x.Row == bk.Row && x.Column == bk.Column))
			{
				BlackPlayer.Check = true;
			}
			else
			{
				BlackPlayer.Check = false;
			}
			if(ComputerPlayer != null)
			{
				if (ComputerPlayer.Color == Turn)
				{
					ComputerPlayer.GenerateMoves(HumanPlayer, ChessBoard, PreviousMove);
				}
			}

		}

		/** This method is called when a player is in check. First it checks if the king has a valid
		 * square it can move to and if it does then there is no checkmate. If there is no valid square
		 * it generates the moves for the checked player and returns true if there is not a possible move
		 * for the checked player and false if there is a possible move.
		 * @returns true if there is checkmate and false otherwise
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public bool CheckForCheckmate()
		{
			Player a_checkedPlayer = WhitePlayer.Check ? WhitePlayer : BlackPlayer;
			Player a_checkingPlayer = WhitePlayer.Check ? BlackPlayer : WhitePlayer;
			King k = (King)a_checkedPlayer.Pieces.Find(x => x is King);
			if (k.ValidSquares.Count > 0)
			{
				return false;
			}
			a_checkedPlayer.GenerateMoves(a_checkingPlayer, ChessBoard, CurrentMove);
			return !PossibleMove(a_checkedPlayer);
		}

		//So far this only calls !PossibleMove so we should change it probably
		public bool CheckForStalemate(Player a_player)
		{
			return !PossibleMove(a_player);
		}

		//We need to finish this probably
		public bool CheckForRepetition()
		{
			//Getting number of times the piece has been on the square it is on
			int numOfReps = CurrentMove.MovingPiece.History.FindAll(x => x.Equals(CurrentMove.Destination.Name)).Count;
			if (numOfReps >= 3)
			{

			}
			return false;
		}
		#endregion

		#region Functions that check if a move is valid: ValidMove, ValidCapture, ValidCastle, ValidSquare, ValidEnPassant
		/** This method takes a move and scans the game to see if the move is valid. If it is
		 * then it returns true and ohterwise it returns false
		 * @param a_move - the move we are checking the validity of
		 * @returns true if the move is valid and false otherwise
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public bool ValidMove(Move a_move)
		{
			CurrentMove = a_move;
			#region Checking if castle
			if (CurrentMove.MovingPiece is King && !CurrentMove.MovingPiece.HasMoved)
			{
				if (CurrentMove.Destination.Column == CurrentMove.MovingPiece.Column + 2 || CurrentMove.Destination.Column == CurrentMove.MovingPiece.Column - 2)
				{
					if (CurrentMove.MovingPiece.Row == 0 || CurrentMove.MovingPiece.Row == 7)
					{
						if (CurrentMove.MovingPiece.Column > CurrentMove.Destination.Column)
						{
							Piece p = ReturnPiece(CurrentMove.Destination.Row, CurrentMove.Destination.Column - 2);
							CurrentMove.Castle = true;
							CurrentMove.CastlingRook = p;
						}
						else
						{
							Piece p = ReturnPiece(CurrentMove.Destination.Row, CurrentMove.Destination.Column + 1);
							CurrentMove.Castle = true;
							CurrentMove.CastlingRook = p;
						}
					}
				}
			}
			#endregion
			#region Checking if en passant
			if (CurrentMove.MovingPiece is Pawn && !CurrentMove.Capture && CurrentMove.MovingPiece.HasMoved)
			{
				if (CurrentMove.MovingPiece.AttackingSquares.Exists(x => x.Row == CurrentMove.Destination.Row && x.Column == CurrentMove.Destination.Column))
				{
					CurrentMove.EnPassant = true;
				}
			}
			#endregion
			Game game = SerializeGame();
			return game.LegalMove();
		}

		/** This method takes a capture move and checks if the piece it is trying to capture is an opposite
		 * color. If it is then it returns true and if not it returns false.
		 * @param a_move - the capture move we are checking the validity of
		 * @returns true if the capture  is valid and false otherwise
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public bool ValidCapture(Move a_move)
		{
			if (a_move.MovingPiece.Color == a_move.CapturedPiece.Color)
			{
				return false;
			}
			return true;
		}

		/** This method takes a castling move and checks its validity. If it is a valid
		 * castle then it returns true and otherwise it returns false.
		 * @param a_move - the castle we are checking the validity of
		 * @returns true if the castle is valid and false otherwise
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public bool ValidCastle(Move a_move)
		{
			//If king or rook have moved then no bueno
			if (a_move.MovingPiece.HasMoved || a_move.CastlingRook.HasMoved)
			{
				return false;
			}
			BoardSquare s1 = new BoardSquare();
			BoardSquare s2 = new BoardSquare();
			BoardSquare s3 = new BoardSquare();
			#region Getting Squares to left or right of king
			if (a_move.MovingPiece.Column > a_move.CastlingRook.Column)
			{
				s1 = ChessBoard.ReturnSquare(a_move.MovingPiece.Row, a_move.MovingPiece.Column - 1);
				s2 = ChessBoard.ReturnSquare(a_move.MovingPiece.Row, a_move.MovingPiece.Column - 2);
				s3 = ChessBoard.ReturnSquare(a_move.MovingPiece.Row, a_move.MovingPiece.Column - 3);
				if (s1.Occupied || s2.Occupied || s3.Occupied)
				{
					return false;
				}
			}
			else
			{
				s1 = ChessBoard.ReturnSquare(a_move.MovingPiece.Row, a_move.MovingPiece.Column + 1);
				s2 = ChessBoard.ReturnSquare(a_move.MovingPiece.Row, a_move.MovingPiece.Column + 2);
				if (s1.Occupied || s2.Occupied)
				{
					return false;
				}
			}
			#endregion
			
			//Getting Square king is on
			BoardSquare movPieceSquare = ChessBoard.ReturnSquare(a_move.MovingPiece.Row, a_move.MovingPiece.Column);
			#region Making sure no squares are attacked
			if (a_move.MovingPiece.Color == Color.White)
			{
				if (BlackPlayer.AttackSquares.Contains(s1) || BlackPlayer.AttackSquares.Contains(s2) || BlackPlayer.AttackSquares.Contains(movPieceSquare))
				{
					return false;
				}
			}
			else if (a_move.MovingPiece.Color == Color.Black)
			{
				if (WhitePlayer.AttackSquares.Contains(s1) || WhitePlayer.AttackSquares.Contains(s2) || WhitePlayer.AttackSquares.Contains(movPieceSquare))
				{
					return false;
				}
			}
			#endregion

			return true;
		}

		/** This method takes a move and checks if the piece can move to the
		 * square it is trying to move to
		 * @param a_move - the move we are checking the validity of
		 * @returns true if the move's destination square is valid and false otherwise
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public bool ValidSquare(Move a_move)
		{
			return a_move.MovingPiece.ValidSquares.Exists(x => x.Name.Equals(a_move.Destination.Name)) ? true : false;
		}

		/** This method takes an en passant move and checks its validity. If it is a valid
		 * en passant then it returns true and otherwise it returns false.
		 * @param a_move - the en passant we are checking the validity of
		 * @returns true if the en passant is valid and false otherwise
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public bool ValidEnPassant(Move a_move)
		{
			//Getting the moving and non-moving players
			#region Obvious cases of no en passant
			if(!(a_move.MovingPiece is Pawn) || a_move.MovingPiece.Row != 3 && a_move.MovingPiece.Row != 4)
			{
				return false;
			}
			if(!(PreviousMove.MovingPiece is Pawn) || PreviousMove.MovingPiece.Row != 3 && PreviousMove.MovingPiece.Row != 4)
			{
				return false;
			}
			if(Math.Abs(PreviousMove.MovingPiece.Column - a_move.MovingPiece.Column) != 1)
			{
				return false;
			}
			if(PreviousMove.MovingPiece.Color == Color.White)
			{
				if(PreviousMove.OriginalSquare.Row != 6)
				{
					return false;
				}
			}
			else if(PreviousMove.MovingPiece.Color == Color.Black)
			{
				if(PreviousMove.OriginalSquare.Row != 1)
				{
					return false;
				}
			}
			#endregion
			return true;
		}
		#endregion

		#region Actually moving and changing data: Move, Castle, Capture, MovePiece, RemovePiece, UpgradePawn
		/** This is what is called when a player is making a move. It goes through
		 * each flag seeing if it is something other than a basic move. It matches up
		 * with the type of move it is and makes the move corresponding to its type.
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void Move()
		{
			#region Checking type of move and making move
			if (CurrentMove.Capture)
			{
				Capture(CurrentMove);
			}
			else if (CurrentMove.Castle)
			{
				Castle(CurrentMove);
			}
			else if (CurrentMove.EnPassant)
			{
				EnPassant(CurrentMove);
			}
			else
			{
				MovePiece(CurrentMove);
			}
			#endregion
		}

		/** This is what is called when a player is castling. It is called from Move().
		 * It first moves the king to the square it is moving to and then moves the
		 * rook to the square next to the king.
		 * @param a_move - The castle that is being executed
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void Castle(Move a_move)
		{
			//Moving king here
			MovePiece(a_move);

			#region Actually changing data for rook and it's destination
			BoardSquare rookSquare = ChessBoard.ReturnSquare(a_move.CastlingRook.Row, a_move.CastlingRook.Column);
			rookSquare.Occupied = false;
			BoardSquare destSquare = new BoardSquare();
			if (a_move.Destination.Column == 2)
			{
				destSquare = ChessBoard.ReturnSquare(a_move.Destination.Row, a_move.Destination.Column + 1);
			}
			else if (a_move.Destination.Column == 6)
			{
				destSquare = ChessBoard.ReturnSquare(a_move.Destination.Row, a_move.Destination.Column - 1);
			}
			a_move.CastlingRook.Row = destSquare.Row;
			a_move.CastlingRook.Column = destSquare.Column;
			a_move.CastlingRook.Square = destSquare;
			a_move.CastlingRook.HasMoved = true;
			destSquare.Occupied = true;
			destSquare.PieceColor = a_move.CastlingRook.Color;
			#endregion
		}

		/** This is what is called when a player is doing an en passant. It is called from Move().
		 * It first removes the pawn that is being taken and then moves the pawn that is moving
		 * to its destination
		 * @param a_move - The en passant that is being executed
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void EnPassant(Move a_move)
		{ 
			RemovePiece(PreviousMove.MovingPiece);
			MovePiece(a_move);
		}

		/** This is what is called when a player is capturing. It is called from Move().
		 * It first removes the captured piece and then moves the piece doing the capturing
		 * to the square the captured piece was
		 * @param a_move - The capture that is being executed
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void Capture(Move a_move)
		{
			RemovePiece(a_move.CapturedPiece);
			MovePiece(a_move);
		}

		/** This is called for the basic function of moving a piece to it's destination square
		 * It sets the square the piece was moving from to not Occupied. Then it changed the row
		 * and column and square of the piece being moved to the new square.
		 * @param a_move - The basic move that is being executed
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void MovePiece(Move a_move)
		{
			#region Actually changing data for piece and square
			a_move.MovingPiece.Square.Occupied = false;
			a_move.MovingPiece.Row = a_move.Destination.Row;
			a_move.MovingPiece.Column = a_move.Destination.Column;
			a_move.MovingPiece.Square = a_move.Destination;
			a_move.MovingPiece.HasMoved = true;
			a_move.Destination.Occupied = true;
			a_move.Destination.PieceColor = a_move.MovingPiece.Color;
			#endregion
		}

		/** This is called to remove a piece from the game
		 * @param a_piece - The piece being removed from the game
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void RemovePiece(Piece a_piece)
		{
			a_piece.Square.Occupied = false;
			#region Removing From Player
			if (a_piece.Color == Color.White)
			{
				//Piece p = WhitePlayer.Pieces.Find(x => x.Name == a_piece.Name);
				WhitePlayer.Pieces.Remove(a_piece);
			}
			else
			{
				//Piece p =BlackPlayer.Pieces.Find(x => x.Name == a_piece.Name);
				BlackPlayer.Pieces.Remove(a_piece);
			}
			#endregion
		}

		/** This method is called when a pawn has reached the final square it can move to.
		 * This takes the pawn that moved and replaces it with the new upgraded piece. The
		 * player chooses which piece to upgrade to.
		 * @param a_originalPawn - The original pawn that is being upgraded
		 * @param a_newPiece - The new piece we are upgrading the pawn to
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public void UpgradePawn(Piece a_originalPawn, Piece a_newPiece)
		{
			if (a_newPiece.Color == Color.White)
			{
				int index = WhitePlayer.Pieces.FindIndex(x => x.Name == a_originalPawn.Name);
				WhitePlayer.Pieces[index] = a_newPiece;
				switch (a_newPiece.Name[1])
				{
					case '1':
						WP1 = WhitePlayer.Pieces[index];
						break;
					case '2':
						WP2 = WhitePlayer.Pieces[index];
						break;
					case '3':
						WP3 = WhitePlayer.Pieces[index];
						break;
					case '4':
						WP4 = WhitePlayer.Pieces[index];
						break;
					case '5':
						WP5 = WhitePlayer.Pieces[index];
						break;
					case '6':
						WP6 = WhitePlayer.Pieces[index];
						break;
					case '7':
						WP7 = WhitePlayer.Pieces[index];
						break;
					case '8':
						WP8 = WhitePlayer.Pieces[index];
						break;
				}

			}
			else if (a_newPiece.Color == Color.Black)
			{
				int index = BlackPlayer.Pieces.FindIndex(x => x.Name == a_originalPawn.Name);
				BlackPlayer.Pieces[index] = a_newPiece;
				switch (index)
				{
					case 0:
						BP1 = BlackPlayer.Pieces[index];
						break;
					case 1:
						BP2 = BlackPlayer.Pieces[index];
						break;
					case 2:
						BP3 = BlackPlayer.Pieces[index];
						break;
					case 3:
						BP4 = BlackPlayer.Pieces[index];
						break;
					case 4:
						BP5 = BlackPlayer.Pieces[index];
						break;
					case 5:
						BP6 = BlackPlayer.Pieces[index];
						break;
					case 6:
						BP7 = BlackPlayer.Pieces[index];
						break;
					case 7:
						BP8 = BlackPlayer.Pieces[index];
						break;
				}
			}
		}
		#endregion

		#region Saving and loading temp file
		/** This is called when we want to save our current game to a temp file
		 * so we can serialize it to do things like check if a move is valid.
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void SaveThis()
		{
			Save s = new Save();
			s.SaveGame(this, @"C:\Users\thoop\source\repos\ChessApp\ChessApp\Saves\TempSaveGame.xml");
		}

		/** This is called when we want to load a game from a temp file
		 * to check if a move is valid.
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public Game LoadThis()
		{
			Game game = Save.LoadGame(@"C:\Users\thoop\source\repos\ChessApp\ChessApp\Saves\TempSaveGame.xml");
			return game;
		}

		/** This is called when we want get a copy of our current game
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public Game SerializeGame()
		{
			SaveThis();
			return LoadThis();
		}
		#endregion

		#region Functions that test the legality of a move: LegalMove, CheckMove, PossibleMove
		/** This calls CheckMove to see if the current move is legal.
		 * @returns true if move is legal and false otherwise.
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public bool LegalMove()
		{
			return CheckMove();
		}

		/** This is called to check if a move is legal. First we make sure that our copied game has
		 * all the correct references so we call ChangeSerializedData(). Then we go through each 
		 * type the move can be and see if it breaks any rules. If it doesn't then it is a valid move.
		 * @returns true if move is legal and false otherwise.
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public bool CheckMove()
		{
			#region For when the deserialized object calls it we must make sure data is correct
			ChangeSerializedData();
			#endregion
			#region Return false if it is not the moving piece's turn
			if (CurrentMove.MovingPiece.Color != Turn)
			{
				return false;
			}
			#endregion
			#region Return false if not a valid square
			if (!ValidSquare(CurrentMove) && !CurrentMove.Castle & !CurrentMove.EnPassant)
			{
				return false;
			}
			#endregion

			#region Checking type of move and making move
			if (CurrentMove.Capture)
			{
				if (!ValidCapture(CurrentMove))
				{
					return false;
				}
				Capture(CurrentMove);
			}
			else if (CurrentMove.Castle)
			{
				try
				{
					if (!ValidCastle(CurrentMove))
					{
						return false;
					}
					Castle(CurrentMove);
				}
				catch (Exception) { return false; }
			}
			else if (CurrentMove.EnPassant)
			{
				try
				{
					if (!ValidEnPassant(CurrentMove))
					{
						return false;
					}
					EnPassant(CurrentMove);
				}
				catch (Exception) { return false; }
			}
			else
			{
				MovePiece(CurrentMove);
			}
			#endregion
			CalculateValidSquares();
			if (CurrentMove.MovingPiece.Color == Color.White)
			{
				if (WhitePlayer.Check)
				{
					return false;
				}
			}
			else if (CurrentMove.MovingPiece.Color == Color.Black)
			{
				if (BlackPlayer.Check)
				{
					return false;
				}
			}
			return true;
		}

		/** Checks if the player that is being passed in has a possible move it can play.
		 * @param a_player - This is checking if this player has a move to play.
		 * @returns True if the player has at least one move to play and false otherwise.
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public bool PossibleMove(Player a_player)
		{
			foreach (Move m in a_player.Moves)
			{
				if (ValidMove(m))
				{
					return true;
				}
			}
			return false;
		}
		#endregion

		#region Computer functions: ComputerMove, FindBestMove, 
		/** This is called when the computer is moving. It returns the best move for the computer.
		 * @returns The best move the computer can make.
		 * @author Thomas Hooper
		 * @date June 2019
        */
		public Move ComputerMove()
		{
			Move move = FindBestMove();
			if (ValidMove(move))
			{
				return move;
			}
			else
			{
				ComputerPlayer.Moves.Remove(move);
				return ComputerMove();
			}
		}

		/** Finds the best move for the computer but the move may not be valid. 
		 * @returns The best move the for the computerthat may not be legal
		 * @author Thomas Hooper
		 * @date June 2019
        */
		public Move FindBestMove()
		{
			foreach(Move m in ComputerPlayer.Moves)
			{
				ComputerPlayer.RateMove(m, HumanPlayer, PreviousMove);
			}
			Move bestMove = ComputerPlayer.Moves.Aggregate((x, y) => x.Value > y.Value ? x : y);
			return bestMove;
		}

		#endregion

		#region Other Functions: ReturnPiece, ChangeSerializedData, Select
		/** This returns the piece in the game at the passed in row and column
		 * @param a_row - The row the piece we are getting is at
		 * @param a_column - The column the piece we are getting is at
		 * @returns The piece at the specified row and column
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public Piece ReturnPiece(int a_row, int a_column)
		{
			foreach (Piece p in WhitePlayer.Pieces)
			{
				if (p.Row == a_row && p.Column == a_column)
				{
					return p;
				}
			}

			foreach (Piece p in BlackPlayer.Pieces)
			{
				if (p.Row == a_row && p.Column == a_column)
				{
					return p;
				}
			}
			return null;
		}

		/** Called when we have serialized and deserialized this game to get a copy of it.
		 * This makes sure all the objects of this game have the correct references to objects
		 * in other classes
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void ChangeSerializedData()
		{
			CurrentMove.MovingPiece = ReturnPiece(CurrentMove.MovingPiece.Row, CurrentMove.MovingPiece.Column);
			CurrentMove.Destination = ChessBoard.ReturnSquare(CurrentMove.Destination.Row, CurrentMove.Destination.Column);
			CurrentMove.OriginalSquare = ChessBoard.ReturnSquare(CurrentMove.OriginalSquare.Row, CurrentMove.OriginalSquare.Column);
			CurrentMove.MovingPiece.Square = CurrentMove.OriginalSquare;
			if (CurrentMove.Capture)
			{
				CurrentMove.CapturedPiece = ReturnPiece(CurrentMove.CapturedPiece.Row, CurrentMove.CapturedPiece.Column);
			}
			else if (CurrentMove.Castle)
			{
				CurrentMove.CastlingRook = ReturnPiece(CurrentMove.CastlingRook.Row, CurrentMove.CastlingRook.Column);
			}
		}
		#endregion
	}
}