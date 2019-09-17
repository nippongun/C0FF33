using System;
namespace GXPEngine
{
	public class Corridor : GameObject
	{
		private Random r;
		private int _corridorIndex;
		private int _startXPosition;
		private int _startYPosition;
		private int _corridorLength;
		private Direction _direction;

		private Entrance _entrance;
		private EntranceTrigger _entranceTrigger;
		private Room _enteringRoom;
		private int entranceXPosition;
		private int entranceYPosition;
		private IntRange _triggerXPosition;
		public Corridor()
		{
			r = new Random();
		}

		public int EndPositionX
		{
			get
			{
				int endPosition = 0;

				if (_direction == Direction.North || _direction == Direction.South)
				{
					endPosition = _startXPosition;
				}
				else if (_direction == Direction.East)
				{
					endPosition = _startXPosition + _corridorLength - 1;
				}
				else if (_direction == Direction.West)
				{
					endPosition = _startXPosition - _corridorLength + 1;
				}

				return endPosition;
			}
		}

		public int EndPositionY
		{
			get
			{
				int endPosition = 0;

				if (_direction == Direction.East || _direction == Direction.West)
				{
					endPosition = _startYPosition;
				}
				else if (_direction == Direction.North)
				{
					endPosition = _startYPosition + _corridorLength - 1;
				}
				else if (_direction == Direction.South)
				{
					endPosition = _startYPosition - _corridorLength + 1;
				}

				return endPosition;
			}
		}

		public void CorridorSetup( Room room, IntRange corridorLength, IntRange roomWidth, IntRange roomHeight, int columns, int rows, bool firstCorridor, int corridorIndex, Room enteringRoom )
		{
			_enteringRoom = enteringRoom;

			_direction = (Direction)r.Next( 0, 4 );

			Direction oppositeDirection = (Direction)(((int)room.GetEnteringDirection + 2) % 4);

			//The entering corridor mustn't overlap with the previous corridor
			if (!firstCorridor && _direction == oppositeDirection)
			{
				// a random direction must be chosen
				int i = (int)_direction;
				i++;
				i = i % 4;
				_direction = (Direction)i;

			}
			if (_direction == Direction.West)
			{
				_direction = Direction.South;
			}
			_corridorLength = corridorLength.Random;

			int maxLength = corridorLength._max;

			switch (_direction)
			{
				case Direction.North:
					_startXPosition = r.Next( room.GetXPosition + 1, room.GetXPosition + room.GetRoomWidth - 1 );
					_startYPosition = room.GetYPosition + room.GetRoomHeight - 1;
					maxLength = rows - _startYPosition - roomHeight._min;
					break;
				case Direction.East:
					_startXPosition = room.GetXPosition + room.GetRoomWidth;
					_startYPosition = r.Next( room.GetYPosition, room.GetYPosition + room.GetRoomHeight - 1 );
					maxLength = columns - _startXPosition - roomWidth._min;
					break;
				case Direction.South:
					_startXPosition = r.Next( room.GetXPosition + 1, room.GetXPosition + room.GetRoomWidth );
					_startYPosition = room.GetYPosition + 1;
					maxLength = _startYPosition - roomHeight._min;
					break;
				case Direction.West:
					_startXPosition = room.GetXPosition;
					_startYPosition = r.Next( room.GetYPosition, room.GetYPosition + room.GetRoomHeight );
					maxLength = _startXPosition - roomWidth._min;
					break;
			}
			_corridorLength = Mathf.Clamp( _corridorLength, 1, maxLength );
		}

		public void setEntrance()
		{
			_entranceTrigger = new EntranceTrigger( "Entrance.png", 1, 1 );
			_entrance = new Entrance( _entranceTrigger, _enteringRoom );
			_triggerXPosition = new IntRange( _enteringRoom.GetXPosition, _enteringRoom.GetXPosition + _enteringRoom.GetRoomWidth );
			entranceXPosition = (_startXPosition + 64) * 64;
			entranceYPosition = (_startYPosition + 64) * 64;
			_entrance.SetXY( entranceXPosition, entranceYPosition );
			_entranceTrigger.SetXY( _triggerXPosition.Random, _enteringRoom.GetYPosition );

			AddChild( _entrance );
			AddChild( _entranceTrigger );
		}

		/// <summary>
		/// Gets the direction
		/// </summary>
		/// <value>_direction</value>
		public Direction GetDirection { get { return _direction; } }
		/// <summary>
		/// Gets the length of the corrdior
		/// </summary>
		/// <value>_corridorLength</value>
		public int GetCorridorLength { get { return _corridorLength; } }
		/// <summary>
		/// Gets the start x position
		/// </summary>
		/// <value>_startXPosition</value>
		public int GetStartX { get { return _startXPosition; } }
		/// <summary>
		/// Gets the the start y positon
		/// </summary>
		/// <value>_startYPosition</value>
		public int GetStartY { get { return _startYPosition; } }
	}
}
