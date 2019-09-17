using System;
namespace GXPEngine
{
	public class Enemy : Entity
	{
		private readonly Player _player;
		private float _enemySpeed;

		public Enemy( string filename, int columns, int rows, EntityType type, Player player ) : base( filename, columns, rows )
		{
			_type = type;
			_enemySpeed = 5f;
			SetOrigin( width / 2, height / 2 );
			this._player = player;
		}

		private new void Update()
		{
			applyGravity();
			movement();
		}

		private void movement()
		{
			if (DistanceTo( _player ) <= 100)
			{
				float distance = DistanceTo( _player );
				if (x < _player.x)
				{
					TryMove( _enemySpeed, 0 );
				}
				else
				{
					TryMove( - _enemySpeed, 0 );
				}
			}
		}
	}
}
