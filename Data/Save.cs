using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace ChessApp
{
	/// <summary>
	/// This class handles saving and loading games
	/// </summary>
	public class Save
	{
		/** We call this when we want to save our game
		 * @param a_game - The game we are saving.
		 * @param a_fileName - The name of the xml file we are saving our game to
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public void SaveGame(Game a_game, string a_fileName)
		{
			using (var stream = new FileStream(a_fileName, FileMode.OpenOrCreate))
			{
				stream.SetLength(0);
				XmlRootAttribute root = new XmlRootAttribute();
				root.ElementName = "ChessApp";
				//root.Namespace = "ChessApp";
				XmlSerializer x = new XmlSerializer(typeof(Game), root);
				x.Serialize(stream, a_game);
				stream.Close();
			}
		}

		/** We call this when we want to load our game
		 * @param a_fileName - The name of the xml file we are loading our game from
		 * @author Thomas Hooper
		 * @date April 2019
        */
		public static Game LoadGame(string a_fileName)
		{
			using (var stream = new FileStream(a_fileName, FileMode.Open))
			{
				XmlRootAttribute root = new XmlRootAttribute();
				root.ElementName = "ChessApp";
				//root.Namespace = "ChessApp";
				XmlSerializer x = new XmlSerializer(typeof(Game));
				Game newGame = (Game)x.Deserialize(stream);
				stream.Close();
				return newGame;
			}
		}
	}
}