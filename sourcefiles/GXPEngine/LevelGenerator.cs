using System;
using System.Collections.Generic;
namespace GXPEngine
{
	public class LevelGenerator : GameObject
	{
		private readonly Random r;

		private int _stage;

		private int _columns = 200;
		private int _rows = 200;
		private IntRange _numRooms;
		private IntRange _roomWidth = new IntRange( 6, 10 );
		private IntRange _roomHeight = new IntRange( 6, 10 );
		private IntRange _corridorLength = new IntRange( 5, 7 );

		private readonly SpriteMap floor = new SpriteMap( "CaveTileset.png", 20, 10 );
		private readonly SpriteMap wall = new SpriteMap( "spritesheet-wall.png", 1, 1 );

		/// <summary>
		/// https://opengameart.org/content/16x16-cave-tileset
		/// 16 by 10 tiles
		/// </summary>
		private const string MAPTILESET = "CaveTileset.png";

		private Player _player;

		private bool[,] _boolTiles;
		private Tile[,] _tiles;
		private Room[] _rooms;
		private Corridor[] _corridors;
		private int[,] _frameValue;

		private List<Pickup> _pickupList;
		private List<Tile> _deadlyList;
		public LevelGenerator( Player player, int stage )
		{
			x = 0;
			y = 0;
			_player = player;
			_stage = stage;
			_pickupList = new List<Pickup>();
			_deadlyList = new List<Tile>();
			r = new Random();

			_numRooms = new IntRange( 4 + _stage, 9 + _stage );

			setupTilesArray();
			createLevel();

			tileSetRooms();
			tileSetCorridor();

			createTiles();
			createWall();

			placeObstacles();
		}
		/// <summary>
		/// Creates a bool array by the size of the actual board size
		/// </summary>
		private void setupTilesArray()
		{
			_frameValue = new int[_columns, _rows];
			_tiles = new Tile[_columns, _rows];
			_boolTiles = new bool[_columns, _rows];
			for (int i = 0; i < _tiles.GetLength( 0 ); i++)
			{
				for (int j = 0; j < _tiles.GetLength( 1 ); j++)
				{
					_boolTiles[i, j] = true;
				}
			}
		}
		/// <summary>
		/// Creates the board and sets the player in the middle of the rooms
		/// </summary>
		private void createLevel()
		{
			_rooms = new Room[_numRooms.Random];

			_corridors = new Corridor[_rooms.Length - 1];

			_rooms[0] = new Room();
			_corridors[0] = new Corridor();
			_rooms[0].RoomSetup( _roomWidth, _roomHeight, _columns, _rows );
			_corridors[0].CorridorSetup( _rooms[0], _corridorLength, _roomWidth, _roomHeight, _columns, _rows, true, 0, _rooms[0] );

			for (int i = 1; i < _rooms.Length; i++)
			{
				_rooms[i] = new Room();

				_rooms[i].RoomSetup( _roomWidth, _roomHeight, _columns, _rows, _corridors[i - 1] );

				if (i < _corridors.Length)
				{
					_corridors[i] = new Corridor();
					_corridors[i].CorridorSetup( _rooms[i], _corridorLength, _roomWidth, _roomHeight, _columns, _rows, false, i, _rooms[i] );
					//_corridors[i].setEntrance();
				}
				if (i == 1)
				{
					_player.x = (_rooms[i].GetXPosition + _rooms[i].GetRoomWidth / 2f) * 16;
					_player.y = (_rooms[i].GetYPosition + _rooms[i].GetRoomHeight) * 16 - 16;
				}
			}
		}
		/// <summary>
		/// Creates a wall around each room and corridor
		/// </summary>
		private void createWall()
		{
			for (int i = 1; i < _boolTiles.GetLength( 0 ) - 1; i++)
			{
				for (int j = 1; j < _boolTiles.GetLength( 1 ) - 1; j++)
				{
					//Create the upper walls
					if (_boolTiles[i, j] && (!_boolTiles[i, j + 1]))
					{
						_tiles[i, j] = new Tile( TileType.Border );
						//_tiles[i, j].SetXY( i * _tiles[i, j].width, j * _tiles[i, j].height );
						_tiles[i, j].SetXY( i * 16, j * 16 );
						_tiles[i, j].SetFrame( 24 );
						_frameValue[i, j] = 24;
						AddChild( _tiles[i, j] );
					}
					//Create the bottom walls
					else if (_boolTiles[i, j] && (!_boolTiles[i, j - 1]))
					{
						_tiles[i, j] = new Tile( TileType.Border );
						//_tiles[i, j].SetXY( i * _tiles[i, j].width, j * _tiles[i, j].height );
						_tiles[i, j].SetXY( i * 16, j * 16 );
						_tiles[i, j].SetFrame( 56 );
						_frameValue[i, j] = 56;
						AddChild( _tiles[i, j] );
					}
					//Create the left walls
					else if (_boolTiles[i, j] && (!_boolTiles[i + 1, j]))
					{
						_tiles[i, j] = new Tile( TileType.Border );
						_tiles[i, j].SetXY( i * 16, j * 16 );
						_tiles[i, j].SetFrame( 39 );
						_frameValue[i, j] = 39;
						AddChild( _tiles[i, j] );
					}
					//Create the right walls
					else if (_boolTiles[i, j] && (!_boolTiles[i - 1, j]))
					{
						_tiles[i, j] = new Tile( TileType.Border );
						_tiles[i, j].SetXY( i * 16, j * 16 );
						_tiles[i, j].SetFrame( 41 );
						_frameValue[i, j] = 41;
						AddChild( _tiles[i, j] );
					}
					//Create the left-upper corner
					//if beneath the current tile a part of a left-wall is and the right tile a part of the upper-wall
					else if (_boolTiles[i - 1 , j] && _frameValue[i, j + 1] == 39 && _frameValue[i + 1, j] == 24)
					{
						_tiles[i, j] = new Tile( TileType.Border );
						_tiles[i, j].SetXY( i * 16, j * 16 );
						_tiles[i, j].SetFrame( 23 );
						_frameValue[i, j] = 23;
						AddChild( _tiles[i, j] );
					}
					else if (_boolTiles[i - 1, j] && _frameValue[i - 1, j] == 24 && _frameValue[i, j + 1] == 41)
					{
						_tiles[i, j] = new Tile( TileType.Border );
						_tiles[i, j].SetXY( i * 16, j * 16 );
						_tiles[i, j].SetFrame( 25 );
						_frameValue[i, j] = 25;
						AddChild( _tiles[i, j] );
					}
				}
			}
		}

		/// <summary>
		/// Sets the boolean of boolTile to false wherever a room is
		/// </summary>
		private void tileSetRooms()
		{
			for (int i = 0; i < _rooms.Length; i++)
			{
				Room currentRoom = _rooms[i];
				if (i != 0)
				{
					switch (currentRoom.GetEnteringDirection)
					{
						case Direction.North:
							_roomWidth = new IntRange( 6, 8 );
							_roomHeight = new IntRange( 6, 10 );
							break;
						case Direction.West:
							_roomWidth = new IntRange( 6, 10 );
							_roomHeight = new IntRange( 6, 8 );
							break;
						case Direction.South:
							_roomWidth = new IntRange( 6, 8 );
							_roomHeight = new IntRange( 6, 10 );
							break;
						case Direction.East:
							_roomWidth = new IntRange( 6, 10 );
							_roomHeight = new IntRange( 6, 8 );
							break;
					}
				}
				for (int j = 0; j < currentRoom.GetRoomWidth; j++)
				{
					int xCoordinate = currentRoom.GetXPosition + j;
					xCoordinate = Mathf.Clamp( xCoordinate, xCoordinate, _columns );
					for (int k = 0; k < currentRoom.GetRoomHeight; k++)
					{
						int yCoordinate = currentRoom.GetYPosition + k;
						yCoordinate = Mathf.Clamp( yCoordinate, yCoordinate, _rows );
						_boolTiles[xCoordinate, yCoordinate] = false;
					}
				}
			}
		}

		/// <summary>
		/// Sets the boolean to false where a corridor is
		/// </summary>
		private void tileSetCorridor()
		{
			for (int i = 0; i < _corridors.Length; i++)
			{
				Corridor currentCorridor = _corridors[i];
				for (int j = 0; j < currentCorridor.GetCorridorLength; j++)
				{
					int xCoordinate = currentCorridor.GetStartX;
					int yCoordinate = currentCorridor.GetStartY;

					switch (currentCorridor.GetDirection)
					{
						case Direction.North:
							yCoordinate += j;
							_boolTiles[xCoordinate - 1, yCoordinate] = false;
							break;
						case Direction.East:
							xCoordinate += j;
							_boolTiles[xCoordinate, yCoordinate - 1] = false;
							break;
						case Direction.South:
							yCoordinate -= j;
							_boolTiles[xCoordinate - 1, yCoordinate] = false;
							break;
						case Direction.West:
							_boolTiles[xCoordinate, yCoordinate + 1] = false;
							xCoordinate -= j;
							break;
					}
					//tiles[xCoordinate, yCoordinate].Solid = false;
					_boolTiles[xCoordinate, yCoordinate] = false;
				}
			}
		}
		/// <summary>
		/// Creates the actual tiles and adds the sprites for a designated spritesheet to the root of the game.
		/// </summary>
		private void createTiles()
		{
			int randomWall;
			int randomFloor;

			for (int i = 0; i < _tiles.GetLength( 0 ); i++)
			{
				for (int j = 0; j < _tiles.GetLength( 1 ); j++)
				{
					randomWall = wall.GetLength();
					randomFloor = floor.GetLength();
					if (!_boolTiles[i, j])
					{
						_tiles[i, j] = new Tile( TileType.Background );
						_tiles[i, j].SetFrame( 17 );
						_frameValue[i, j] = 17;
						//_tiles[i, j].SetXY( i * _tiles[i, j].width, j * _tiles[i, j].height );
						_tiles[i, j].SetXY( i * 16, j * 16 );
						AddChild( _tiles[i, j] );
					}
				}

			}
		}

		private void placeObstacles()
		{
			for (int i = 0; i < _rooms.Length; i++)
			{
				Pickup pumpkin = new Pickup( PickupType.Pumpkin );
				pumpkin.SetFrame( 102 );
				pumpkin.SetXY( (r.Next( _rooms[i].GetXPosition, (_rooms[i].GetXPosition + _rooms[i].GetRoomWidth) )) * 16, r.Next( (_rooms[i].GetYPosition), (_rooms[i].GetYPosition + _rooms[i].GetRoomHeight) ) * 16 );
				_pickupList.Add( pumpkin );
				AddChild( pumpkin );

				if (i == _rooms.Length - 1)
				{
					Tile _exit = new Tile( TileType.Exit );
					_exit.SetXY( (r.Next( _rooms[i].GetXPosition, (_rooms[i].GetXPosition + _rooms[i].GetRoomWidth) )) * 16, r.Next( (_rooms[i].GetYPosition), (_rooms[i].GetYPosition + _rooms[i].GetRoomHeight) ) * 16 );
					AddChild( _exit );

					Tile deadly = new Tile( TileType.Deadly );
					deadly.SetFrame( 51 );
					for (int j = 0; j < _rooms[i].GetRoomWidth; j++)
					{
						switch (_corridors[_corridors.Length - 1].GetDirection)
						{
							case Direction.North:
								deadly.SetXY( (_corridors[i - 1].GetStartX) * 16, (_corridors[i - 1].GetStartY + _corridors[i - 1].GetCorridorLength * 16) * j );
								_deadlyList.Add( deadly );
								AddChild( deadly );
								break;
							case Direction.East:
								deadly.SetXY( (_corridors[i - 1].GetStartX + _corridors[i - 1].GetCorridorLength) * 16, (_rooms[i].GetYPosition + _rooms[i].GetRoomHeight) * 16 * j );
								_deadlyList.Add( deadly );
								AddChild( deadly );
								break;
							case Direction.South:
								deadly.SetXY( (_corridors[i - 1].GetStartX) * 16, (_corridors[i - 1].GetStartY + _corridors[i - 1].GetCorridorLength) * 16 );
								_deadlyList.Add( deadly );
								AddChild( deadly );
								break;
							case Direction.West:
								break;
						}
					}
				}
			}
		}

		//private void setEntrance( int corridorIndex, int roomIndex) 
		//{

		//}
	}
}


