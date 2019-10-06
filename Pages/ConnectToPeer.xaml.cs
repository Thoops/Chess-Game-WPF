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
	/// Interaction logic for ConnectToPeer.xaml
	/// </summary>
	public partial class ConnectToPeer : Page
	{
		private Color m_color;

		/** Called when we navigate to this page
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public ConnectToPeer()
		{
			InitializeComponent();
		}

		/** Called when we want to start a game over a connection with a friend
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			DataTransfer d = null;
			try
			{
				d = new DataTransfer(Convert.ToInt32(portNumberBox1.Text), Convert.ToInt32(portNumberBox2.Text));
			}
			catch (Exception)
			{
				MessageBox.Show("Enter Port Numbers");
				return;
			}
			d.StartConnection();
			ChessGame c = new ChessGame(d, m_color);
			this.NavigationService.Navigate(c);
		}

		/** Called when we click the white button. It sets us as the white player in the game
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void WhiteButton_Click(object sender, RoutedEventArgs e)
		{
			m_color = Color.White;
			whiteButton.IsEnabled = false;
			blackButton.IsEnabled = false;
		}

		/** Called when we click the black button. It sets us as the black player in the game
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void BlackButton_Click(object sender, RoutedEventArgs e)
		{
			m_color = Color.Black;
			whiteButton.IsEnabled = false;
			blackButton.IsEnabled = false;
		}
	}
}
