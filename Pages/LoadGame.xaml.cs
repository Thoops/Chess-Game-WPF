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

namespace ChessApp
{
    /// <summary>
    /// Interaction logic for LoadGame.xaml.
	/// This is where we can choose which game we ant to load to play
    /// </summary>
    public partial class LoadGame : Page
    {
		/** Called when we navigate to this page
		 * @author Thomas Hooper
		 * @date April 2019
		 */
        public LoadGame()
        {
            InitializeComponent();
        }

		/** Called when we click slot 1. The method loads the saved game in slot 1 for us to play.
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void Slot1_Click(object sender, RoutedEventArgs e)
		{
			//this.NavigationService.Navigate(new Uri(@"Pages\ChessGame.xaml", UriKind.Relative));
			Game game = new Game();
			game = Save.LoadGame(@"C:\Users\thoop\source\repos\ChessApp\ChessApp\Saves\SaveGame1.xml");
			ChessGame c = new ChessGame(game);
			this.NavigationService.Navigate(c);
			ChessBoard.Refresh(c.chessBoard.LocationGrid);
			c.chessBoard.SetPieces();
			if(c.chessBoard.Game.HumanPlayer.Color == Color.Black)
			{
				c.Flip();
			}
		}

		/** Called when we click slot 2. The method loads the saved game in slot 2 for us to play.
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void Slot2_Click(object sender, RoutedEventArgs e)
		{
			Game game = new Game();
			game = Save.LoadGame(@"C:\Users\thoop\source\repos\ChessApp\ChessApp\Saves\SaveGame2.xml");
			ChessGame c = new ChessGame(game);
			this.NavigationService.Navigate(c);
			ChessBoard.Refresh(c.chessBoard.LocationGrid);
			c.chessBoard.SetPieces();
			if (c.chessBoard.Game.HumanPlayer.Color == Color.Black)
			{
				c.Flip();
			}
		}

		/** Called when we click slot 3. The method loads the saved game in slot 3 for us to play.
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void Slot3_Click(object sender, RoutedEventArgs e)
		{
			Game game = new Game();
			game = Save.LoadGame(@"C:\Users\thoop\source\repos\ChessApp\ChessApp\Saves\SaveGame3.xml");
			ChessGame c = new ChessGame(game);
			this.NavigationService.Navigate(c);
			ChessBoard.Refresh(c.chessBoard.LocationGrid);
			c.chessBoard.SetPieces();
			if (c.chessBoard.Game.HumanPlayer.Color == Color.Black)
			{
				c.Flip();
			}
		}
	}
}
