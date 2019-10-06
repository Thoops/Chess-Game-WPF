using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.ComponentModel;

namespace ChessApp
{
	/// <summary>
	/// Interaction logic for ChessBoard.xaml. This is where the whole game is
	/// played and displayed.
	/// </summary>
	public partial class ChessBoard : UserControl, INotifyPropertyChanged
	{
		#region For Connected Games
		private bool m_connGame; /**< True if this is a game played over a connection */
		public bool ConnectedGame
		{
			get { return m_connGame; }
			set { m_connGame = value; }
		}
		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private DataTransfer m_dataTransfer; /**< Our object to send and receive data from our opponent */
		private string m_currentMoveString; /**< The string of the current move played in the game */

		/** Called when we want to start a game over a connection with a friend
		 * @param a_dataTransfer - The data transfer object for sending and receiving data
		 * @param a_color - The color of the pieces you are playing with
		 * @author Thomas Hooper
		 * @date September 2019
        */
		public void StartConnectedGame(DataTransfer a_transfer, Color a_color)
		{
			
			m_game = new Game(true);
			ComputerGame = false;
			ConnectedGame = true;
			m_currentMoveString = "";

			SetDataContext();
			m_dataTransfer = a_transfer;

			if (a_color == Color.Black)
			{
				GetMoveFromFriend();
			}
		}

		/** Gets the move our opponent sent and played by interpreting the string we pass in.
		 * @param a_moveString - The string we are extracting our opponent's move from 
		 * @returns Our opponent's move
		 * @author Thomas Hooper
		 * @date September 2019
        */
		public Move ReturnMoveFromString(string a_moveString)
		{
			Move move = new Move();
			string pattern = @"\w+";
			Regex rx = new Regex(pattern);
			MatchCollection matches = rx.Matches(a_moveString);

			//Piece for our move
			Piece piece = null;

			//Square for our move
			BoardSquare square = new BoardSquare();

			if (matches[2].ToString().Equals("White"))
			{
				piece = m_game.WhitePlayer.ReturnPieceFromString(matches[0].ToString());
			}
			else if (matches[2].ToString().Equals("Black"))
			{
				piece = m_game.BlackPlayer.ReturnPieceFromString(matches[0].ToString());
			}

			square = m_game.ChessBoard.ReturnSquareFromString(matches[1].ToString());

			if(matches.Count > 3)
			{
				//Capture Move
				if (matches[3].ToString().Equals("Capture"))
				{
					if(piece.Color == Color.White)
					{
						Piece capturedPiece = m_game.BlackPlayer.ReturnPieceFromString(matches[4].ToString());
						move = new Move(square, piece, capturedPiece);
					}
					else if(piece.Color == Color.Black)
					{
						Piece capturedPiece = m_game.WhitePlayer.ReturnPieceFromString(matches[4].ToString());
						move = new Move(square, piece, capturedPiece);
					}
				}
				//Castling Move
				else if (matches[3].ToString().Equals("Castle"))
				{
					if (piece.Color == Color.White)
					{
						Piece rook = m_game.WhitePlayer.ReturnPieceFromString(matches[4].ToString());
						move = new Move(square, piece, rook, true);
					}
					else if (piece.Color == Color.Black)
					{
						Piece rook = m_game.BlackPlayer.ReturnPieceFromString(matches[4].ToString());
						move = new Move(square, piece, rook, true);
					}
				}
				//En Passant
				else if (matches[3].ToString().Equals("EnPassant"))
				{
					if (piece.Color == Color.White)
					{
						Piece capturedPawn = m_game.BlackPlayer.ReturnPieceFromString(matches[4].ToString());
						move = new Move()
						{
							MovingPiece = piece,
							Destination = square,
							OriginalSquare = piece.Square,
							EnPassant = true
						};
					}
					else if (piece.Color == Color.Black)
					{
						Piece capturedPawn = m_game.WhitePlayer.ReturnPieceFromString(matches[5].ToString());
						move = new Move()
						{
							MovingPiece = piece,
							Destination = square,
							OriginalSquare = piece.Square,
							EnPassant = true
						};
					}
				}
			}
			else
			{
				move = new Move(square, piece);
			}
			
			return move;
		}

		/** Plays the move our opponent sent to us. 
		 * @param a_move - The move our friend played that we are now going to execute on our end
		 * @author Thomas Hooper
		 * @date September 2019
        */
		public void ConnectedMove(Move a_move)
		{
			object piece = new object();
			try
			{
				piece = ReturnPiece(a_move.MovingPiece.Row, a_move.MovingPiece.Column);
			}
			catch (NullReferenceException)
			{
				MessageBox.Show("NullReferenceException");
			}
			if (a_move.Capture)
			{
				m_game.CurrentMove = a_move;
				m_game.ChangeSerializedData();
				UIElement pieceView = ReturnPiece(a_move.Destination.Row, a_move.Destination.Column);
				Capture(a_move, piece, pieceView, false);
			}

			else
			{
				m_game.CurrentMove = a_move;
				m_game.ChangeSerializedData();
				Move(a_move, piece, false);
			}
		}

		/** Gets the move our opponent sent in string form. First we wait until the data is sent
		 * and then we call ConnectedMove to play our friend's move
		 * @author Thomas Hooper
		 * @date September 2019
        */
		public void GetMoveFromFriend()
		{
			try
			{
				while (true)
				{
					if (!m_dataTransfer.MoveString.Equals(m_currentMoveString))
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return;
			}
			m_currentMoveString = m_dataTransfer.MoveString;
			Move move = ReturnMoveFromString(m_dataTransfer.MoveString);
			ConnectedMove(move);
		}
		#endregion

		#region Properties
		private UIElement m_pieceView;
		public UIElement PieceView
		{
			get { return m_pieceView; }
			set
			{
				m_pieceView = value;
			}
		}
		private static Action EmptyDelegate = delegate () { };
		public static void Refresh(UIElement uiElement)
		{
			uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
		}
		private Piece m_pieceData; /**< The data for our selected piece */
		private Game m_game; /**< The data for the game being playes */
		public Game Game { get { return m_game; } set { m_game = value; } }
		public int MoveNum { get; set; }
		private bool ComputerGame { get; set; }
		#endregion

		#region Starting Game: ChessBoard, StartGame, SetDataContext
		/** Constructor for ChessBoard that just initializes the control
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public ChessBoard()
		{
			InitializeComponent();
			MoveNum = 0;
			Thread.Sleep(100);
		}
		/** Starts a game against the computer
		 * @param a_color - The color pieces we play with
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public void StartGame(Color a_color)
		{
			m_game = new Game(a_color);
			ComputerGame = true;
			ConnectedGame = false;
			SetDataContext();
			if(m_game.ComputerPlayer.Color == Color.White)
			{
				MoveComputer();
				m_game.ChangeTurns();
				WriteToMoveList();
			}
		}

		/** Starts a game against yourself
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public void StartTestGame()
		{
			m_game = new Game(true);
			ComputerGame = false;
			ConnectedGame = false;
			SetDataContext();
		}

		/** Sets the data context for our Piece UserControls so they can be bound to
		 * the row and column properties of their respective pieces
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void SetDataContext()
		{
			#region Setting Data Context White
			whiteRookA.DataContext = m_game.WR1;
			whiteKnightB.DataContext = m_game.WN1;
			whiteBishopC.DataContext = m_game.WB1;
			whiteQueen.DataContext = m_game.WQ;
			whiteKing.DataContext = m_game.WK;
			whiteBishopF.DataContext = m_game.WB2;
			whiteKnightG.DataContext = m_game.WN2;
			whiteRookH.DataContext = m_game.WR2;
			whitePawn1.DataContext = m_game.WP1;
			whitePawn2.DataContext = m_game.WP2;
			whitePawn3.DataContext = m_game.WP3;
			whitePawn4.DataContext = m_game.WP4;
			whitePawn5.DataContext = m_game.WP5;
			whitePawn6.DataContext = m_game.WP6;
			whitePawn7.DataContext = m_game.WP7;
			whitePawn8.DataContext = m_game.WP8;
			#endregion
			#region Setting Data Context Black
			blackRookA.DataContext = m_game.BR1;
			blackKnightB.DataContext = m_game.BN1;
			blackBishopC.DataContext = m_game.BB1;
			blackQueen.DataContext = m_game.BQ;
			blackKing.DataContext = m_game.BK;
			blackBishopF.DataContext = m_game.BB2;
			blackKnightG.DataContext = m_game.BN2;
			blackRookH.DataContext = m_game.BR2;
			blackPawn1.DataContext = m_game.BP1;
			blackPawn2.DataContext = m_game.BP2;
			blackPawn3.DataContext = m_game.BP3;
			blackPawn4.DataContext = m_game.BP4;
			blackPawn5.DataContext = m_game.BP5;
			blackPawn6.DataContext = m_game.BP6;
			blackPawn7.DataContext = m_game.BP7;
			blackPawn8.DataContext = m_game.BP8;
			#endregion
		}
		#endregion

		#region Click Event Handlers
		/** This is what is called when we click on an empty square. It first checks 
		 * that a piece is selected and if the move is valid it makes the move
		 * @param sender - the Border that we clicked on
		 * @param e - provides data for mouse button related events
		 * @author Thomas Hooper
		 * @date February 2019
        */
		private void Space_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if(m_pieceData == null)
			{
				return;
			}
			Border b = sender as Border;
			BoardSquare square = m_game.ChessBoard.ReturnSquare(Grid.GetRow(b), Grid.GetColumn(b));
			Move move = new Move(square, m_pieceData);

			if (ConnectedGame)
			{
				//m_conn.SendMove(move);
				if (m_game.ValidMove(move))
				{
					Move(move, sender, ComputerGame);
					string moveString = ChessApp.Move.ReturnStringFromMove(move);
					m_dataTransfer.SendData(moveString);
					try
					{
						GetMoveFromFriend();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}
				}
			}
			else
			{
				Move(move, sender, ComputerGame);
			}
		}

		/** This is what is called when we click on a piece. It first checks 
		 * if we want to select this piece and this will happen if we click on 
		 * a piece we own. Else it will capture the piece clicked on if the move is valid
		 * @param sender - the Piece UserControl that we clicked on
		 * @param e - provides data for mouse button related events
		 * @author Thomas Hooper
		 * @date February 2019
        */
		private void Piece_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			UIElement pieceView = (UIElement)sender;
			BoardSquare square = m_game.ChessBoard.ReturnSquare(Grid.GetRow(pieceView), Grid.GetColumn(pieceView));
			//Piece piece = square.Piece;
			Piece piece = m_game.ReturnPiece(Grid.GetRow(pieceView), Grid.GetColumn(pieceView));
			#region Select
			if (piece.Color == m_game.Turn)
			{
				//m_game.Select(m_pieceData);
				PieceView = pieceView; //Selected piece = pieceView
				m_pieceData = piece; //Selected piece data = piece
			}
			#endregion
			#region Capture
			else if (piece.Color != m_game.Turn && m_pieceData != null)
			{
				Move move = new Move(square, m_pieceData, piece); //Creating a capture move
																  //bool legalMove = m_game.Move(move); 
				if (ConnectedGame)
				{
					if (m_game.ValidMove(move))
					{
						try
						{
							Capture(move, sender, pieceView, ComputerGame);
							string moveString = ChessApp.Move.ReturnStringFromMove(move);
							m_dataTransfer.SendData(moveString);
							GetMoveFromFriend();
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.ToString());
						}
					}
				}
				else
				{
					Capture(move, sender, pieceView, ComputerGame);
				}
			}
			#endregion
		}

		#endregion

		/** This is called if we play as black so we flip the board to make sure
		 * the pieces are oriented correctly.
		 * @author Thomas Hooper
		 * @date June 2019
        */
		public void FlipPieces()
		{
			foreach (UIElement piece in LocationGrid.Children)
			{
				if (!(piece is Border))
				{
					piece.RenderTransformOrigin = new Point(0.5, 0.5);

					ScaleTransform flipTrans = new ScaleTransform();
					flipTrans.ScaleX = -1;
					flipTrans.ScaleY = -1;
					piece.RenderTransform = flipTrans;
				}
			}
		}

		/** This returns the UIElement at the specified row and column
		 * @param a_row - The row the piece is on
		 * @param a_column - The column the piece is on
		 * @returns The UIElement at a_row and a_column
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public UIElement ReturnPiece(int a_row, int a_column)
		{
			
			foreach (UIElement u in LocationGrid.Children)
			{
				
				if (u is Border)
				{
					continue;
				}
				
				if (Grid.GetRow(u) == a_row && Grid.GetColumn(u) == a_column)
				{
					
					return u;
				}
			}
			
			return null;
		}

		/** Called after we made a move and it is the computer's turn. The computer will find
		 * the best move and then move.
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public void MoveComputer()
		{
			//m_game.ComputerMove();
			m_game.ComputerPlayer.GenerateMoves(m_game.HumanPlayer, m_game.ChessBoard, m_game.PreviousMove);
			Move move = m_game.ComputerMove();

			//m_pieceData = m_game.CurrentMove.MovingPiece;
			if (move.Capture)
			{
				//UIElement captPiece = ReturnPiece(m_game.CurrentMove.CapturedPiece.Row, m_game.CurrentMove.CapturedPiece.Column);
				UIElement captPiece = ReturnPiece(move.CapturedPiece.Row, move.CapturedPiece.Column);
				LocationGrid.Children.Remove(captPiece);
			}
			else if (move.EnPassant)
			{
				UIElement pieceView = new UIElement();
				if (move.MovingPiece.Color == Color.White)
				{
					pieceView = ReturnPiece(move.MovingPiece.Row + 1, move.MovingPiece.Column);
				}
				else if (move.MovingPiece.Color == Color.Black)
				{
					pieceView = ReturnPiece(move.MovingPiece.Row - 1, move.MovingPiece.Column);
				}
				LocationGrid.Children.Remove(pieceView);
			}

			m_game.Move();
			m_pieceData = m_game.CurrentMove.MovingPiece;
			#region Upgrading Pawn
			if (m_pieceData is Pawn)
			{
				if(m_pieceData.Color == Color.White)
				{
					if(m_pieceData.Row == 0)
					{
						UpgradePawn("Queen");
					}
				}
				else if (m_pieceData.Color == Color.Black)
				{
					if (m_pieceData.Row == 7)
					{
						UpgradePawn("Queen");
					}
				}
			}
			#endregion

		}

		/** Called after a move is made. It writes to the MoveList textbox the last move played.
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public void WriteToMoveList()
		{

			string moveString = m_game.CurrentMove.MovingPiece.GetMoveString();

			if (m_game.CurrentMove.Capture)
			{
				if (m_game.CurrentMove.MovingPiece is Pawn)
				{
					moveString = moveString.Insert(0, m_game.CurrentMove.OriginalSquare.Name[0].ToString());
				}
				moveString = moveString.Insert(1, "x");
			}
			#region Checking Player and if there was other piece of same type
			if (m_game.CurrentMove.MovingPiece.Color == Color.White)
			{
				if (m_game.WhitePlayer.Pieces.Exists(x => x != m_game.CurrentMove.MovingPiece
				 && x.ValidSquares.Contains(m_game.CurrentMove.Destination) && x.Name[0].Equals(m_game.CurrentMove.MovingPiece.Name[0])))
				{
					moveString = moveString.Insert(1, m_game.CurrentMove.OriginalSquare.Name[0].ToString());
				}
			}
			else if (m_game.CurrentMove.MovingPiece.Color == Color.Black)
			{
				if (m_game.BlackPlayer.Pieces.Exists(x => x != m_game.CurrentMove.MovingPiece
				 && x.ValidSquares.Contains(m_game.CurrentMove.Destination) && x.Name[0].Equals(m_game.CurrentMove.MovingPiece.Name[0])))
				{
					moveString = moveString.Insert(1, m_game.CurrentMove.OriginalSquare.Name[0].ToString());
				}
			}
			#endregion
			if (m_game.CurrentMove.Castle)
			{
				if (m_game.CurrentMove.Destination.Name == "g1" || m_game.CurrentMove.Destination.Name == "g8")
				{
					moveString = "O-O";
				}
				else if (m_game.CurrentMove.Destination.Name == "c1" || m_game.CurrentMove.Destination.Name == "c8")
				{
					moveString = "O-O-O";
				}
			}
			if (m_game.WhitePlayer.Check || m_game.BlackPlayer.Check)
			{
				moveString += "+";
			}
			if (m_game.Checkmate)
			{
				moveString.Remove(moveString.Length - 1);
				moveString += "#";
			}
			if (m_game.CurrentMove.MovingPiece.Color == Color.White)
			{
				MoveNum++;
				m_game.MoveString += MoveNum.ToString() + ") ";
				m_game.MoveString += moveString;
			}
			else if (m_game.CurrentMove.MovingPiece.Color == Color.Black)
			{
				m_game.MoveString += "   " + moveString;
				m_game.MoveString += "\r\n";
			}

		}

		#region Functions for upgrading pawn
		/** This is called when our pawn has reached the final rank and is upgrading
		 * @param a_pieceName - the piece we want to upgrade our pawn to
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public void UpgradePawn(string a_pieceName)
		{
			UIElement pieceView = new UIElement();
			pieceView = ReturnPiece(m_pieceData.Row, m_pieceData.Column);
			#region Changing pieceView
			if (a_pieceName.Equals("Queen"))
			{
				Piece piece = new Queen(m_pieceData.Row, m_pieceData.Column, 90, m_pieceData.Color, m_pieceData.Name)
				{
					Square = m_pieceData.Square
				};
				m_game.UpgradePawn(m_pieceData, piece);
				if (m_pieceData.Color == Color.White)
				{
					LocationGrid.Children.Remove(pieceView);
					WhiteQueen newView = new WhiteQueen();
					LocationGrid.Children.Add(newView);
					SetViewDataContext(newView);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
				}
				else if (m_pieceData.Color == Color.Black)
				{
					LocationGrid.Children.Remove(pieceView);
					BlackQueen newView = new BlackQueen();
					LocationGrid.Children.Add(newView);
					SetViewDataContext(newView);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
				}
			}
			else if (a_pieceName.Equals("Rook"))
			{
				Piece piece = new Rook(m_pieceData.Row, m_pieceData.Column, 50, m_pieceData.Color, m_pieceData.Name)
				{
					Square = m_pieceData.Square
				};
				m_game.UpgradePawn(m_pieceData, piece);
				if (m_pieceData.Color == Color.White)
				{
					LocationGrid.Children.Remove(pieceView);
					WhiteRook newView = new WhiteRook();
					LocationGrid.Children.Add(newView);
					SetViewDataContext(newView);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
				}
				else if (m_pieceData.Color == Color.Black)
				{
					LocationGrid.Children.Remove(pieceView);
					BlackRook newView = new BlackRook();
					LocationGrid.Children.Add(newView);
					SetViewDataContext(newView);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
				}
			}
			else if (a_pieceName.Equals("Bishop"))
			{
				Piece piece = new Bishop(m_pieceData.Row, m_pieceData.Column, 30, m_pieceData.Color, m_pieceData.Name)
				{
					Square = m_pieceData.Square
				};
				m_game.UpgradePawn(m_pieceData, piece);
				if (m_pieceData.Color == Color.White)
				{
					LocationGrid.Children.Remove(pieceView);
					WhiteBishop newView = new WhiteBishop();
					LocationGrid.Children.Add(newView);
					SetViewDataContext(newView);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
				}
				else if (m_pieceData.Color == Color.Black)
				{
					LocationGrid.Children.Remove(pieceView);
					BlackBishop newView = new BlackBishop();
					LocationGrid.Children.Add(newView);
					SetViewDataContext(newView);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
				}
			}
			else if (a_pieceName.Equals("Knight"))
			{
				Piece piece = new Knight(m_pieceData.Row, m_pieceData.Column, 30, m_pieceData.Color, m_pieceData.Name)
				{
					Square = m_pieceData.Square
				};
				m_game.UpgradePawn(m_pieceData, piece);
				if (m_pieceData.Color == Color.White)
				{
					LocationGrid.Children.Remove(pieceView);
					WhiteKnight newView = new WhiteKnight();
					LocationGrid.Children.Add(newView);
					SetViewDataContext(newView);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
				}
				else if (m_pieceData.Color == Color.Black)
				{
					LocationGrid.Children.Remove(pieceView);
					BlackKnight newView = new BlackKnight();
					LocationGrid.Children.Add(newView);
					SetViewDataContext(newView);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
				}
			}
			#endregion

		}


		/** This is called when we click on the final rank for our pawn
		 * and click on one of the menu items that appear. It gets the
		 * piece we want to upgrade to and upgrades oour pawn.
		 * @param sender - The menu item we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			MenuItem m = (MenuItem)sender;
			string name = m.Name;
			UpgradePawn(name);
			if (m_game.CurrentMove.Capture)
			{
				UIElement pieceView = new UIElement();
				pieceView = ReturnPiece(m_game.CurrentMove.Destination.Row, m_game.CurrentMove.Destination.Column);
				LocationGrid.Children.Remove(pieceView);
			}
			m_game.Move();
			Refresh(LocationGrid);
			WriteToMoveList();
			m_game.ChangeTurns();
			if (m_game.Checkmate)
			{
				
				if (m_game.Turn == Color.Black)
				{
					MessageBox.Show("White Wins");
				}
				else if (m_game.Turn == Color.White)
				{
					MessageBox.Show("Black Wins");
				}
				return;
			}

			if (ComputerGame)
			{
				MoveComputer();
				WriteToMoveList();
				m_game.ChangeTurns();
				if (m_game.Checkmate)
				{
					
					if (m_game.Turn == Color.Black)
					{
						MessageBox.Show("White Wins");
					}
					else if (m_game.Turn == Color.White)
					{
						MessageBox.Show("Black Wins");
					}
					return;
				}
			}
		}

		/** Called when we finally upgrade the pawn and now we have to set the data context
		 * for our new upgraded piece
		 * @param a_view - The control of which we are setting the data context
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void SetViewDataContext(UserControl a_view)
		{
			Binding rowBind = new Binding
			{
				Path = new PropertyPath("Row"),
				Mode = BindingMode.TwoWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			Binding columnBind = new Binding
			{
				Path = new PropertyPath("Column"),
				Mode = BindingMode.TwoWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			if (a_view is WhiteQueen || a_view is WhiteRook || a_view is WhiteBishop || a_view is WhiteKnight)
			{
				switch (m_pieceData.Name[1])
				{
					case '1':
						rowBind.Source = m_game.WP1;
						columnBind.Source = m_game.WP1;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '2':
						rowBind.Source = m_game.WP2;
						columnBind.Source = m_game.WP2;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '3':
						rowBind.Source = m_game.WP3;
						columnBind.Source = m_game.WP3;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '4':
						rowBind.Source = m_game.WP4;
						columnBind.Source = m_game.WP4;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '5':
						rowBind.Source = m_game.WP5;
						columnBind.Source = m_game.WP5;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '6':
						rowBind.Source = m_game.WP6;
						columnBind.Source = m_game.WP6;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '7':
						rowBind.Source = m_game.WP7;
						columnBind.Source = m_game.WP7;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '8':
						rowBind.Source = m_game.WP8;
						columnBind.Source = m_game.WP8;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
				}
				m_game.CurrentMove.MovingPiece = m_game.WhitePlayer.Pieces.Find(x =>
				x.Row == m_game.CurrentMove.MovingPiece.Row && x.Column == m_game.CurrentMove.MovingPiece.Column);
			}
			else if (a_view is BlackQueen || a_view is BlackRook || a_view is BlackBishop || a_view is BlackKnight)
			{
				switch (m_pieceData.Name[1])
				{
					case '1':
						rowBind.Source = m_game.BP1;
						columnBind.Source = m_game.BP1;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '2':
						rowBind.Source = m_game.BP2;
						columnBind.Source = m_game.BP2;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '3':
						rowBind.Source = m_game.BP3;
						columnBind.Source = m_game.BP3;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '4':
						rowBind.Source = m_game.BP4;
						columnBind.Source = m_game.BP4;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '5':
						rowBind.Source = m_game.BP5;
						columnBind.Source = m_game.BP5;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '6':
						rowBind.Source = m_game.BP6;
						columnBind.Source = m_game.BP6;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '7':
						rowBind.Source = m_game.BP7;
						columnBind.Source = m_game.BP7;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
					case '8':
						rowBind.Source = m_game.BP8;
						columnBind.Source = m_game.BP8;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
						break;
				}
				m_game.CurrentMove.MovingPiece = m_game.BlackPlayer.Pieces.Find(x =>
				x.Row == m_game.CurrentMove.MovingPiece.Row && x.Column == m_game.CurrentMove.MovingPiece.Column);
			}
		}
		#endregion

		/** This is called when we are making a move. If we are playing the computer
		 * then we call ComputerMove() at the end of this method.
		 * @param a_move - The move we are making
		 * @param a_sender - The Border we clicked
		 * @param a_comp - If true then it is a game against the computer
		 * @author Thomas Hooper
		 * @date June 2019
        */
		private void Move(Move a_move, object a_sender, bool a_comp)
		{
			if (m_game.ValidMove(a_move))
			{
				#region Checking if piece is pawn and upgrading
				if (m_pieceData is Pawn)
				{
					if (m_pieceData.Color == Color.White)
					{
						if (m_pieceData.Row == 1)
						{
							ContextMenu choiceMenu = this.FindResource("ChoiceMenu") as ContextMenu;
							choiceMenu.PlacementTarget = a_sender as Button;
							choiceMenu.IsOpen = true;
							return;
						}
					}
					else if (m_pieceData.Color == Color.Black)
					{
						if (m_pieceData.Row == 6)
						{
							ContextMenu choiceMenu = this.FindResource("ChoiceMenu") as ContextMenu;
							choiceMenu.PlacementTarget = a_sender as Button;
							choiceMenu.IsOpen = true;
							return;
						}
					}
				}
				#endregion
				m_game.Move();
				if (m_game.CurrentMove.EnPassant)
				{
					UIElement pieceView = new UIElement();
					if (m_game.CurrentMove.MovingPiece.Color == Color.White)
					{
						pieceView = ReturnPiece(m_game.CurrentMove.MovingPiece.Row + 1, m_game.CurrentMove.MovingPiece.Column);
					}
					else if (m_game.CurrentMove.MovingPiece.Color == Color.Black)
					{
						pieceView = ReturnPiece(m_game.CurrentMove.MovingPiece.Row - 1, m_game.CurrentMove.MovingPiece.Column);
					}

					LocationGrid.Children.Remove(pieceView);
				}

				Refresh(LocationGrid);
				m_game.ChangeTurns();
				WriteToMoveList();
				if (m_game.Checkmate)
				{
					
					if(m_game.Turn == Color.Black)
					{
						MessageBox.Show("White Wins");
					}
					else if (m_game.Turn == Color.White)
					{
						MessageBox.Show("Black Wins");
					}
					return;
				}
				if (a_comp)
				{
					MoveComputer();
					m_game.ChangeTurns();
					WriteToMoveList();
					if (m_game.Checkmate)
					{
						
						if (m_game.Turn == Color.Black)
						{
							MessageBox.Show("White Wins");
						}
						else if (m_game.Turn == Color.White)
						{
							MessageBox.Show("Black Wins");
						}
						return;
					}
				}
			}
		}

		/** This is called when we are making a capture move. If we are playing the computer
		 * then we call ComputerMove() at the end of this method just like in Move().
		 * @param a_move - The move we are making
		 * @param a_sender - The Piece we clicked
		 * @param a_pieceView - The UIElement to be removed because it is being captured
		 * @param a_comp - If true then it is a game against the computer
		 * @author Thomas Hooper
		 * @date June 2019
        */
		private void Capture(Move a_move, object a_sender, UIElement a_pieceView, bool a_comp)
		{
			if (m_game.ValidMove(a_move))
			{
				#region Checking if piece is pawn and upgrading
				if (m_pieceData is Pawn)
				{
					if (m_pieceData.Color == Color.White)
					{
						if (m_pieceData.Row == 1)
						{
							ContextMenu choiceMenu = this.FindResource("ChoiceMenu") as ContextMenu;
							choiceMenu.PlacementTarget = a_sender as Button;
							choiceMenu.IsOpen = true;
							return;
						}
					}
					else if (m_pieceData.Color == Color.Black)
					{
						if (m_pieceData.Row == 6)
						{
							ContextMenu choiceMenu = this.FindResource("ChoiceMenu") as ContextMenu;
							choiceMenu.PlacementTarget = a_sender as Button;
							choiceMenu.IsOpen = true;
							return;
						}
					}
				}
				#endregion
				m_game.Move();
				LocationGrid.Children.Remove(a_pieceView);
				Refresh(LocationGrid);
				m_game.ChangeTurns();
				WriteToMoveList();

				if (m_game.Checkmate)
				{
					
					if (m_game.Turn == Color.Black)
					{
						MessageBox.Show("White Wins");
					}
					else if (m_game.Turn == Color.White)
					{
						MessageBox.Show("Black Wins");
					}
					return;
				}

				if (a_comp)
				{
					MoveComputer();
					m_game.ChangeTurns();
					WriteToMoveList();
					if (m_game.Checkmate)
					{
						if (m_game.Turn == Color.Black)
						{
							MessageBox.Show("White Wins");
						}
						else if (m_game.Turn == Color.White)
						{
							MessageBox.Show("Black Wins");
						}
						return;
					}
				}
			}
		}

		#region Saving and Loading Game Methods: SaveGame, LoadGame, SetPieces, AddViewWithContext, SetContextOnLoaded
		/** Called when we want to save our game
		 * @param a_fileName - The file we are saving our game to
		 * @author Thomas Hooper
		 * @date June 2019
        */
		public void SaveGame(string a_fileName)
		{
			Save s = new Save();
			s.SaveGame(m_game, a_fileName);
		}

		/** Called when we want to load a game
		 * @param a_game - The game we are loading
		 * @author Thomas Hooper
		 * @date June 2019
        */
		public void LoadGame(Game a_game)
		{
			m_game = new Game();
			m_game.GameLoaded(a_game);
			ComputerGame = true;
			SetDataContext();
		}

		/** Called when we load a game and have to set our pieces in the correct spots.
		 * First it removes all of the piece UIElements repopulates the board with new ones
		 * corresponding to the correct piece type and position of each piece
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public void SetPieces()
		{
			#region Removing the pieces
			List<UIElement> piecesToRemove = new List<UIElement>();
			foreach (UIElement u in LocationGrid.Children)
			{
				if (u is Border)
				{
					continue;
				}
				else
				{
					piecesToRemove.Add(u);
				}
			}
			foreach(UIElement u in piecesToRemove)
			{
				LocationGrid.Children.Remove(u);
			}
			#endregion

			#region Adding Pieces
			
			foreach(Piece p in m_game.WhitePlayer.Pieces)
			{
				m_pieceData = p;
				AddViewWithContext(p);
			}

			foreach (Piece p in m_game.BlackPlayer.Pieces)
			{
				m_pieceData = p;
				AddViewWithContext(p);
			}
			#endregion
		}

		/** We pass in a piece and add a view of the piece on our board. We then bind
		 * a_piece to the view so it updates its position when a_piece.Row or a_piece.Column
		 * are changed.
		 * @param a_piece - The piece we are adding a view of.
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void AddViewWithContext(Piece a_piece)
		{
			if(a_piece.Color == Color.White)
			{
				if (a_piece is King)
				{
					//WhiteKing newView = this.FindResource("NewWhiteKing") as WhiteKing;
					WhiteKing newView = new WhiteKing();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Queen)
				{
					//WhiteQueen newView = this.FindResource("NewWhiteQueen") as WhiteQueen;
					WhiteQueen newView = new WhiteQueen();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Rook)
				{
					//WhiteRook newView = this.FindResource("NewWhiteRook") as WhiteRook;
					WhiteRook newView = new WhiteRook();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Bishop)
				{
					//WhiteBishop newView = this.FindResource("NewWhiteBishop") as WhiteBishop;
					WhiteBishop newView = new WhiteBishop();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Knight)
				{
					//WhiteKnight newView = this.FindResource("NewWhiteKnight") as WhiteKnight;
					WhiteKnight newView = new WhiteKnight();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Pawn)
				{
					//WhitePawn newView = this.FindResource("NewWhitePawn") as WhitePawn;
					WhitePawn newView = new WhitePawn();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
			}
			else if(a_piece.Color == Color.Black)
			{
				if (a_piece is King)
				{
					//BlackKing newView = this.FindResource("NewBlackKing") as BlackKing;
					BlackKing newView = new BlackKing();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Queen)
				{
					//BlackQueen newView = this.FindResource("NewBlackQueen") as BlackQueen;
					BlackQueen newView = new BlackQueen();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Rook)
				{
					//BlackRook newView = this.FindResource("NewBlackRook") as BlackRook;
					BlackRook newView = new BlackRook();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Bishop)
				{
					//BlackBishop newView = this.FindResource("NewBlackBishop") as BlackBishop;
					BlackBishop newView = new BlackBishop();
					
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Knight)
				{
					//BlackKnight newView = this.FindResource("NewBlackKnight") as BlackKnight;
					BlackKnight newView = new BlackKnight();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
				else if (a_piece is Pawn)
				{
					//BlackPawn newView = this.FindResource("NewBlackPawn") as BlackPawn;
					BlackPawn newView = new BlackPawn();
					LocationGrid.Children.Add(newView);
					SetContextOnLoaded(newView, a_piece);
					newView.MouseLeftButtonDown += Piece_MouseLeftButtonDown;
					return;
				}
			}
		}

		/** We use this method to bind the row and column of a_view in the grid to the
		 * Row and Column properties of a_piece so the view can update when Row or Column are changed.
		 * @param a_view - The view we are binding to the properties of a_piece.
		 * @param a_piece - The piece we are adding a view of.
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void SetContextOnLoaded(UserControl a_view, Piece a_piece)
		{
			Binding rowBind = new Binding
			{
				Path = new PropertyPath("Row"),
				Mode = BindingMode.TwoWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			Binding columnBind = new Binding
			{
				Path = new PropertyPath("Column"),
				Mode = BindingMode.TwoWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			if (a_piece.Color == Color.White)
			{
				if (a_view is WhitePawn)
				{
					switch (a_piece.Name[1])
					{
						case '1':
							rowBind.Source = m_game.WP1;
							columnBind.Source = m_game.WP1;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '2':
							rowBind.Source = m_game.WP2;
							columnBind.Source = m_game.WP2;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '3':
							rowBind.Source = m_game.WP3;
							columnBind.Source = m_game.WP3;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '4':
							rowBind.Source = m_game.WP4;
							columnBind.Source = m_game.WP4;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '5':
							rowBind.Source = m_game.WP5;
							columnBind.Source = m_game.WP5;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '6':
							rowBind.Source = m_game.WP6;
							columnBind.Source = m_game.WP6;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '7':
							rowBind.Source = m_game.WP7;
							columnBind.Source = m_game.WP7;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '8':
							rowBind.Source = m_game.WP8;
							columnBind.Source = m_game.WP8;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
					}
				}
				else if(a_view is WhiteBishop)
				{
					if (a_piece.Name.Contains("p"))
					{
						SetViewDataContext(a_view);
					}
					else if(a_piece.Name[1] == '1')
					{
						rowBind.Source = m_game.WB1;
						columnBind.Source = m_game.WB1;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
					else if (a_piece.Name[1] == '2')
					{
						rowBind.Source = m_game.WB2;
						columnBind.Source = m_game.WB2;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
				}
				else if (a_view is WhiteKnight)
				{
					if (a_piece.Name.Contains("p"))
					{
						SetViewDataContext(a_view);
					}
					else if (a_piece.Name[1] == '1')
					{
						rowBind.Source = m_game.WN1;
						columnBind.Source = m_game.WN1;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
					else if (a_piece.Name[1] == '2')
					{
						rowBind.Source = m_game.WN2;
						columnBind.Source = m_game.WN2;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
				}
				else if (a_view is WhiteRook)
				{
					if (a_piece.Name.Contains("p"))
					{
						SetViewDataContext(a_view);
					}
					else if (a_piece.Name[1] == '1')
					{
						rowBind.Source = m_game.WR1;
						columnBind.Source = m_game.WR1;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
					else if (a_piece.Name[1] == '2')
					{
						rowBind.Source = m_game.WR2;
						columnBind.Source = m_game.WR2;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
				}
				else if (a_view is WhiteQueen)
				{
					if (a_piece.Name.Contains("p"))
					{
						SetViewDataContext(a_view);
					}
					else if (a_piece.Name[0] == 'q')
					{
						rowBind.Source = m_game.WQ;
						columnBind.Source = m_game.WQ;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
				}
				else if (a_view is WhiteKing)
				{
					rowBind.Source = m_game.WK;
					columnBind.Source = m_game.WK;
					a_view.SetBinding(Grid.RowProperty, rowBind);
					a_view.SetBinding(Grid.ColumnProperty, columnBind);
				}
			}

			else if (a_piece.Color == Color.Black)
			{
				if (a_view is BlackPawn)
				{
					switch (a_piece.Name[1])
					{
						case '1':
							rowBind.Source = m_game.BP1;
							columnBind.Source = m_game.BP1;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '2':
							rowBind.Source = m_game.BP2;
							columnBind.Source = m_game.BP2;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '3':
							rowBind.Source = m_game.BP3;
							columnBind.Source = m_game.BP3;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '4':
							rowBind.Source = m_game.BP4;
							columnBind.Source = m_game.BP4;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '5':
							rowBind.Source = m_game.BP5;
							columnBind.Source = m_game.BP5;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '6':
							rowBind.Source = m_game.BP6;
							columnBind.Source = m_game.BP6;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '7':
							rowBind.Source = m_game.BP7;
							columnBind.Source = m_game.BP7;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
						case '8':
							rowBind.Source = m_game.BP8;
							columnBind.Source = m_game.BP8;
							a_view.SetBinding(Grid.RowProperty, rowBind);
							a_view.SetBinding(Grid.ColumnProperty, columnBind);
							break;
					}
				}
				else if (a_view is BlackBishop)
				{
					if (a_piece.Name.Contains("p"))
					{
						SetViewDataContext(a_view);
					}
					else if (a_piece.Name[1] == '1')
					{
						rowBind.Source = m_game.BB1;
						columnBind.Source = m_game.BB1;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
					else if (a_piece.Name[1] == '2')
					{
						rowBind.Source = m_game.BB2;
						columnBind.Source = m_game.BB2;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
				}
				else if (a_view is BlackKnight)
				{
					if (a_piece.Name.Contains("p"))
					{
						SetViewDataContext(a_view);
					}
					else if (a_piece.Name[1] == '1')
					{
						rowBind.Source = m_game.BN1;
						columnBind.Source = m_game.BN1;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
					else if (a_piece.Name[1] == '2')
					{
						rowBind.Source = m_game.BN2;
						columnBind.Source = m_game.BN2;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
				}
				else if (a_view is BlackRook)
				{
					if (a_piece.Name.Contains("p"))
					{
						SetViewDataContext(a_view);
					}
					else if (a_piece.Name[1] == '1')
					{
						rowBind.Source = m_game.BR1;
						columnBind.Source = m_game.BR1;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
					else if (a_piece.Name[1] == '2')
					{
						rowBind.Source = m_game.BR2;
						columnBind.Source = m_game.BR2;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
				}
				else if (a_view is BlackQueen)
				{
					if (a_piece.Name.Contains("p"))
					{
						SetViewDataContext(a_view);
					}
					else if (a_piece.Name[0] == 'q')
					{
						rowBind.Source = m_game.BQ;
						columnBind.Source = m_game.BQ;
						a_view.SetBinding(Grid.RowProperty, rowBind);
						a_view.SetBinding(Grid.ColumnProperty, columnBind);
					}
				}
				else if (a_view is BlackKing)
				{
					rowBind.Source = m_game.BK;
					columnBind.Source = m_game.BK;
					a_view.SetBinding(Grid.RowProperty, rowBind);
					a_view.SetBinding(Grid.ColumnProperty, columnBind);
				}
			}
		}
		#endregion
	}
}