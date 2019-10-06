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
	/// Interaction logic for GameOver.xaml
	/// </summary>
	public partial class GameOver : Page
	{
		/** Called when we navigate to this page and want to display the winner
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public GameOver(string a_string)
		{
			InitializeComponent();
			gameOver.Text += a_string;
		}

		/** Takes us back to the main page
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MainPage p = new MainPage();
			this.NavigationService.Navigate(p);
		}
	}
}
