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
    /// Interaction logic for SelectColor.xaml
	/// This is where you select what color you play the computer as
    /// </summary>
    public partial class SelectColor : Page
    {
		
        public SelectColor()
        {
            InitializeComponent();
        }

		/** Called when we click the white button. We go to ChessGame as the white player
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date March 2019
        */
		private void WhiteButton_Click(object sender, RoutedEventArgs e)
		{
			//this.NavigationService.Navigate(new Uri(@"Pages\ChessGame.xaml", UriKind.Relative));
			ChessGame p = new ChessGame(Color.White);
			this.NavigationService.Navigate(p);
		}

		/** Called when we click the black button. We go to ChessGame as the black player
		 * @param sender - The button we clicked
		 * @param e - Contains state information
		 * @author Thomas Hooper
		 * @date April 2019
        */
		private void BlackButton_Click(object sender, RoutedEventArgs e)
		{
			//this.NavigationService.Navigate(new Uri(@"Pages\ChessGame.xaml", UriKind.Relative));
			ChessGame p = new ChessGame(Color.Black);
			this.NavigationService.Navigate(p);
		}
	}
}
