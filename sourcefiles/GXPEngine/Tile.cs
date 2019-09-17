using System;
using System.Drawing;
namespace GXPEngine
{
	public class Tile : AnimationSprite
	{
		private TileType _tileType;
		public Tile( TileType tileType ) : base( "CaveTileset.png", 16, 10 )
		{
			_tileType = tileType;
		}
		public TileType GetTileType { get { return _tileType; } }
	}
}
