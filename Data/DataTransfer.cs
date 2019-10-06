using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Windows;


namespace ChessApp
{
	/// <summary>
	/// This is the class that allows us to communicate with 
	/// another player through a socket. It allows us to play
	/// a friend over a connection
	/// </summary>
	public class DataTransfer : INotifyPropertyChanged
	{
		private Socket m_socket  /**< The socket we communicate through*/;
		private EndPoint m_localEndpoint; /**< Your local endpoint*/
		private EndPoint m_friendEndpoint; /**< The endpoint for your opponent*/
		private string m_localIp; /**< Your IP Address*/
		private string m_friendsIp; /**< The IP address of your opponent*/
		private int m_localPort; /**< Your port number*/
		private int m_friendsPort; /**< The port number of your opponent*/
		private string m_moveString; /**< The string we receive that holds move data*/
		private bool m_changed;

		public bool StringChanged
		{
			get { return m_changed; }
			set { m_changed = value; }
		}

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
			StringChanged = true;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		
		/** Constructor for Data Transfer. It takes the local port number and the
		 * number of our friends port. It also gets the IP of this player and the player
		 * you are playing against. For oour purposes and for demonstration we have the friends
		 * IP address and the local player's IP address be the local IP address
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public DataTransfer(int a_localPort, int a_friendPort)
		{
			m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			m_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

			m_localIp = "127.0.0.1";
			m_friendsIp = "127.0.0.1";

			m_localPort = a_localPort;
			m_friendsPort = a_friendPort;
			MoveString = "";
		}

		/** This method starts the connection between this player and his friend.
		 * The socket begins to receive data from the friend.
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public void StartConnection()
		{
			m_localEndpoint = new IPEndPoint(IPAddress.Parse(m_localIp), m_localPort);
			m_socket.Bind(m_localEndpoint);
			m_friendEndpoint = new IPEndPoint(IPAddress.Parse(m_friendsIp), m_friendsPort);
			m_socket.Connect(m_friendEndpoint);

			byte[] buffer = new byte[2000];
			m_socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref m_friendEndpoint, new AsyncCallback(DataCallBack), buffer);
		}

		/** This function sends the data for our move to our friend. When it is received
		 * the friend's program will interpret our data and display our move
		 * @param a_moveString - The string we are sending to our opponent
		 * @author Thomas Hooper
		 * @date August 2019
        */
		public void SendData(string a_moveString)
		{
			ASCIIEncoding e = new ASCIIEncoding();
			byte[] data = new byte[2000];
			data = e.GetBytes(a_moveString);
			m_socket.Send(data);
		}

		/** This method is called when we receive data from our opponent
		 * @param a_result - Represents the status of the operation
		 * @author Thomas Hooper
		 * @date August 2019
        */
		private void DataCallBack(IAsyncResult a_result)
		{
			try
			{
				int size = m_socket.EndReceiveFrom(a_result, ref m_friendEndpoint);
				if (size > 0)
				{
					byte[] receivedData = new byte[1464];
					receivedData = (byte[])a_result.AsyncState;
					ASCIIEncoding eEncoding = new ASCIIEncoding();
					string receivedMessage = eEncoding.GetString(receivedData);
					MoveString = receivedMessage;
				}

				byte[] buffer = new byte[2000];
				m_socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref m_friendEndpoint, new AsyncCallback(DataCallBack), buffer);
			}
			catch (SocketException)
			{
				MessageBox.Show("You Are Not Connected To Anyone");
			}
		}
	}
}