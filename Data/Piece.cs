using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChessApp
{
	/// <summary>
	/// Abstract class Piece that 6 classes inherit from
	/// </summary>
	[XmlInclude(typeof(Pawn))]
	[XmlInclude(typeof(Knight))]
	[XmlInclude(typeof(Bishop))]
	[XmlInclude(typeof(Rook))]
	[XmlInclude(typeof(Queen))]
	[XmlInclude(typeof(King))]
	public abstract class Piece : INotifyPropertyChanged
	{
		/** Empty Constructor for serialization purposes
        */
		public Piece() { }
		protected int m_row; /**< The row of the board the piece is on */
		protected int m_column; /**< The column of the board the piece is on */
		protected int m_value; /**< The value of the piece */
		protected List<BoardSquare> m_validSquares; /**< The valid squares the piece can move to */
		protected List<BoardSquare> m_attackingSquares; /**< The squares the piece is attacking */
		protected List<BoardSquare> m_invalidSquares; /**< The squares the piece can't move to that it originally could*/
		protected bool m_hasMoved; /**< True if the piece has moved and false if not */
		protected string m_name; /**< The name of the piece which is used like an id */
		protected List<string> m_history; /**< Move history of the piece in string form */
		protected BoardSquare m_square; /**< The square of the board the piece is on */

		public int Value { get; set; }
		public Color Color { get; set; }
		public List<BoardSquare> ValidSquares
		{
			get { return m_validSquares; }
			set { m_validSquares = value; }
		}
		public List<BoardSquare> AttackingSquares
		{
			get { return m_attackingSquares; }
			set { m_attackingSquares = value; }
		}
		public List<BoardSquare> InvalidSquares
		{
			get { return m_invalidSquares; }
			set { m_invalidSquares = value; }
		}
		public BoardSquare Square
		{
			get { return m_square; }
			set
			{
				m_square = value;
			}
		}
		public bool HasMoved
		{
			get { return m_hasMoved; }
			set { m_hasMoved = value; }
		}
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}
		public List<string> History
		{
			get { return m_history; }
			set { m_history = value; }
		}

		public int Row
		{
			get { return m_row; }
			set
			{
				m_row = value;
				NotifyPropertyChanged("Row");
			}
		}
		public int Column
		{
			get { return m_column; }
			set
			{
				m_column = value;
				NotifyPropertyChanged("Column");
			}
		}

		/** This represents the event raised when the property Row or Column
		 * are changed
		 */
		public event PropertyChangedEventHandler PropertyChanged;

		/** Called when Row or Column are changed. The purpose for this is
		 * our viewmodels of the pieces are bound to the properties Row and Column
		 * so anytime they change this is called and they are updated properly
		 */
		public void NotifyPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/** Constructor for Piece
		 * @param a_row - The row the piece is on
		 * @param a_column - The column the piece is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Piece(int a_row, int a_column, int a_value, Color a_color, string a_name)
		{
			Row = a_row;
			Column = a_column;
			Value = a_value;
			Color = a_color;
			HasMoved = false;
			ValidSquares = new List<BoardSquare>();
			InvalidSquares = new List<BoardSquare>();
			AttackingSquares = new List<BoardSquare>();
			History = new List<string>();
			Name = a_name;
		}

		/** Constructor for Piece that also takes a BoardSquare object
		 * @param a_row - The row the piece is on
		 * @param a_column - The column the piece is on
		 * @param a_value - the value of the piece
		 * @param a_color - the color of the piece (Black or White)
		 * @param a_name - the name of the piece
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public Piece(int a_row, int a_column, int a_value, Color a_color, string a_name, BoardSquare a_square)
		{
			Row = a_row;
			Column = a_column;
			Value = a_value;
			Color = a_color;
			HasMoved = false;
			ValidSquares = new List<BoardSquare>();
			InvalidSquares = new List<BoardSquare>();
			AttackingSquares = new List<BoardSquare>();
			History = new List<string>();
			Name = a_name;
			Square = a_square;
		}

		/** This Function sets the squares this piece is able to move to.
		 * This is an abstract class so the classes that extend this class
		 * implement this function. It takes the board and sets the valid squares
		 * the piece is able to move to
		 * @param a_board - The chessboard the piece is on
		 * @author Thomas Hooper
		 * @date February 2019
        */
		public abstract void SetValidSquares(Board a_board);

		/** Function for writing to the game's move list when this piece
		 * moves
		 * @returns a string that represents the move the piece just made
		 * @author Thomas Hooper
		 * @date July 2019
        */
		public abstract string GetMoveString();
	}
}