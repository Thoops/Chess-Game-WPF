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

namespace ChessApp
{
    /// <summary>
    /// Interaction logic for ChessGame.xaml.
	/// This is where our chessboard is located and that
	/// is where most of the code gets executed.
    /// </summary>
    public partial class ChessGame : Page
    {
		public int MoveNumber { get; set; }

		private static Action EmptyDelegate = delegate () { };
		public static void Refresh(UIElement uiElement)
		{
			uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
		}

		#region Constructors
		/** Constructor for ChessGame that is called when we want to play against
		 * the computer
		 * @param a_color - The color of the pieces we play with.
		 * @author Thomas Hooper
		 * @date March 2019
        */
		public ChessGame(Color a_color)
        {
            InitializeComponent();
			if(a_color == Color.Black)
			{
				Flip();
			}
			MoveNumber = 0;
			chessBoard.StartGame(a_color);
			MoveList.DataContext = chessBoard.Game;
		}

		/** Constructor for ChessGame that is called when we want to play against
		 * the computer and load our game from a save file
		 * @param a_game - The game we are loading.
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public ChessGame(Game a_game)
		{
			InitializeComponent();
			
			MoveNumber = 0;
			chessBoard.LoadGame(a_game);
			MoveList.DataContext = chessBoard.Game;
			
		}

		/** Constructor for ChessGame that is called when we want to play against
		 * a friend over a connection
		 * @param a_dataTransfer - The object we send and receive data from our opponent over
		 * @param a_color - The color of the pieces we play with.
		 * @author Thomas Hooper
		 * @date September 2019
        */
		public ChessGame(DataTransfer a_dataTransfer, Color a_color)
		{
			InitializeComponent();
			MoveNumber = 0;
			if(a_color == Color.Black)
			{
				Flip();
				Refresh(chessBoard);
			}
			
			chessBoard.StartConnectedGame(a_dataTransfer, a_color);
			MoveList.DataContext = chessBoard.Game;
		}
		#endregion

		/** Called when we play as black so we flip the board and pieces to be on our
		 * side
		 * @author Thomas Hooper
		 * @date June 2019
        */
		public void Flip()
		{
			chessBoard.RenderTransformOrigin = new Point(0.5, 0.5);

			ScaleTransform flipTrans = new ScaleTransform();
			flipTrans.ScaleX = -1;
			flipTrans.ScaleY = -1;
			chessBoard.RenderTransform = flipTrans;
			chessBoard.FlipPieces();
		}

		#region Save Buttons
		/** Called when we click save and want to save our game
		 * @param sender - The button we clicked
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void Save1_Click(object sender, RoutedEventArgs e)
		{
			chessBoard.SaveGame(@"C:\Users\thoop\source\repos\ChessApp\ChessApp\Saves\SaveGame1.xml");
		}

		/** Called when we click save and want to save our game
		 * @param sender - The button we clicked
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void Save2_Click(object sender, RoutedEventArgs e)
		{
			chessBoard.SaveGame(@"C:\Users\thoop\source\repos\ChessApp\ChessApp\Saves\SaveGame2.xml");
		}

		/** Called when we click save and want to save our game
		 * @param sender - The button we clicked
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void Save3_Click(object sender, RoutedEventArgs e)
		{
			chessBoard.SaveGame(@"C:\Users\thoop\source\repos\ChessApp\ChessApp\Saves\SaveGame3.xml");
		}
		#endregion

		#region Move List
		/** Called \after a move and we want to write to the move list.
		 * @param a_move - The string of the move we just made
		 * @param a_color - The color of the player that just played
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public void AddMoveToList(string a_move, Color a_color)
		{
			if(a_color == Color.White)
			{
				MoveList.Text += MoveNumber.ToString() + ") ";
				MoveList.Text += a_move;
			}
			else if(a_color == Color.Black)
			{
				MoveList.Text += "   " + a_move + "\r\n";
				MoveNumber++;
			}
		}
		#endregion

		/** Called when there is checkmate and we navigate to Game over page
		 * @author Thomas Hooper
		 * @date September 2019
        */
		private void ChessBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (chessBoard.Game.Checkmate)
			{
				string winnerString = "";
				if (chessBoard.Game.Turn == Color.Black)
				{
					winnerString = "White Wins!";
				}
				else if (chessBoard.Game.Turn == Color.White)
				{
					winnerString = "Black Wins!";
				}
				GameOver g = new GameOver(winnerString);
				this.NavigationService.Navigate(g);
			}
		}
	}
}
