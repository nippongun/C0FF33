using System;
namespace GXPEngine
{
	public class Room : GameObject
	{
		private Random r;

		private int _xPosition;
		private int _yPosition;
		private int _roomWidth;
		private int _roomHeight;
		private Direction _enteringDirection;
		public Room()
		{
			r = new Random();
		}
		/// <summary>
		/// Setups the very first room and place it roughly to the middle of the board
		/// </summary>
		/// <param name="roomWidth">Room width.</param>
		/// <param name="roomHeight">Room height.</param>
		/// <param name="columns">Columns.</param>
		/// <param name="rows">Rows.</param>
		public void RoomSetup( IntRange roomWidth, IntRange roomHeight, int columns, int rows )
		{
			_roomWidth = roomWidth.Random;
			_roomHeight = roomHeight.Random;

			_xPosition = Mathf.Round( columns / 2f - _roomWidth / 2f );
			_yPosition = Mathf.Round( rows / 2f - _roomHeight / 2f );
		}
		/// <summary>
		/// Setups the room by the specifed values
		/// </summary>
		/// <param name="roomWidth">Room width.</param>
		/// <param name="roomHeight">Room height.</param>
		/// <param name="columns">Columns.</param>
		/// <param name="rows">Rows.</param>
		/// <param name="corridor">Corridor.</param>
		public void RoomSetup( IntRange roomWidth, IntRange roomHeight, int columns, int rows, Corridor corridor )
		{
			_enteringDirection = corridor.GetDirection;

			_roomWidth = roomWidth.Random;
			_roomHeight = roomHeight.Random;

			switch (corridor.GetDirection)
			{
				//Depending on the direction...
				case Direction.North:
					//the room height is between 1 and rows - the y value of end position, since the length mustn't go over the total size of the board
					_roomHeight = Mathf.Clamp( _roomHeight, 1, rows - corridor.EndPositionY );
					//Due north is 'working' on the y-axis, the room just have to put at the end of the corridor
					_yPosition = corridor.EndPositionY;
					//The x position of the corridor should be somewhere at the bottom edge of the room
					_xPosition = r.Next( corridor.EndPositionX - _roomWidth + 1, corridor.EndPositionX );
					//Clamping for preventing overshooting
					_xPosition = Mathf.Clamp( _xPosition, 0, columns - _roomWidth - 1 );
					break;
					//Same fot the other directions, but with specified algortihm, (mostly some changing on the y- and x-axis)
				case Direction.East:
					_roomWidth = Mathf.Clamp( _roomWidth, 1, columns - corridor.EndPositionX );
					_xPosition = corridor.EndPositionX;
					_yPosition = r.Next( corridor.EndPositionY - _roomHeight + 1, corridor.EndPositionY );
					_yPosition = Mathf.Clamp( _yPosition, 0, rows - _roomHeight - 1 );
					break;
				case Direction.South:
					_roomHeight = Mathf.Clamp( _roomHeight, 1, corridor.EndPositionY - 1 );
					_yPosition = corridor.EndPositionY - _roomHeight + 1;
					_xPosition = r.Next( corridor.EndPositionX - _roomWidth + 1, corridor.EndPositionX );
					_xPosition = Mathf.Clamp( _xPosition, 0, columns - _roomWidth - 1 );
					break;
				case Direction.West:
					_roomWidth = Mathf.Clamp( _roomWidth, 1, corridor.EndPositionX - 1 );
					_xPosition = corridor.EndPositionX - _roomWidth + 1;
					_yPosition = r.Next( corridor.EndPositionY - _roomHeight + 1, corridor.EndPositionY );
					_yPosition = Mathf.Clamp( _yPosition, 0, rows - _roomHeight - 1 );
					break;
			}
		}
		public int GetXPosition { get { return _xPosition; } }
		public int GetYPosition { get { return _yPosition; } }
		public int GetRoomHeight { get { return _roomHeight; } }
		public int GetRoomWidth { get { return _roomWidth; } }
		public Direction GetEnteringDirection { get { return _enteringDirection; } }
	}
}