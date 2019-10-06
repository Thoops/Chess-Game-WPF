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
    /// Interaction logic for MainPage.xaml.
	/// This is the main menu where we can decide
	/// what kind of chess game we want to play
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

		/** Called when we click the load game button and it take us to the LoadGame page
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(new Uri(@"Pages\LoadGame.xaml", UriKind.Relative));
		}

		/** Called when we click the play against computer button.
		 * It takes us to ChessGame and we play against the computer.
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date March 2019
        */
		private void CompButton_Click(object sender, RoutedEventArgs e)
		{
			SelectColor s = new SelectColor();
			//this.NavigationService.Navigate(new Uri(@"Pages\SelectColor.xaml", UriKind.Relative));
			this.NavigationService.Navigate(s);
			
		}

		/** Called when we click the play against friend button
		 * It takes us to the ConnectToPeer page so we can play against
		 * a friend
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void FriendButton_Click(object sender, RoutedEventArgs e)
		{
			//ChessGame c = new ChessGame(Color.White);
			//this.NavigationService.Navigate(c);
			//c.chessBoard.StartFriendGame();
			//SelectColor s = new SelectColor(true);
			//this.NavigationService.Navigate(new Uri(@"Pages\SelectColor.xaml", UriKind.Relative));
			ConnectToPeer cp = new ConnectToPeer();
			this.NavigationService.Navigate(cp);
		}

		/** Called when we click play against yourself button and allows us to play
		 * against ourselves
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void TestButton_Click(object sender, RoutedEventArgs e)
		{
			ChessGame c = new ChessGame(Color.White);
			this.NavigationService.Navigate(c);
			c.chessBoard.StartTestGame();
		}
	}
}