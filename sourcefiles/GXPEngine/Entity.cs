using System;
namespace GXPEngine
{
	public abstract class Entity : AnimationSprite
	{
		protected Entity( string filename, int columns, int rows ) : base( filename, columns, rows )
		{
		}

		protected EntityType _type;
		protected float _speedX;
		protected float _speedY;
		protected bool _isGrounded;

		protected Entity( string filename, int columns, int rows, EntityType type ) : base( filename, columns, rows )
		{
			_type = type;
			_isGrounded = false;
		}

		public void Update()
		{
			applyGravity();
		}

		protected bool TryMove( float moveX, float moveY )
		{
			if (Math.Abs( moveX ) < 0.005 && Math.Abs( moveY ) < 0.005)
				return false;
			x += moveX;
			y += moveY;


			bool canMove = true;
			foreach (GameObject other in GetCollisions())
			{

				if (other is Tile)
				{
					Tile tile = other as Tile;
					if (tile.GetTileType == TileType.Border)
					{
						canMove = canMove && manageCollision( tile, moveX, moveY );
					}
				}
			}
			return canMove;
		}

		protected bool manageCollision( GameObject other, float moveX, float moveY )
		{
			if (other is Tile)
			{
				Tile tile = other as Tile;

				if (tile.GetTileType == TileType.Border)
				{
					return resolveCollision( other as Sprite, moveX, moveY );
				}
			}
			return true;
		}

		protected bool resolveCollision( Sprite collisionObject, float moveX, float moveY )
		{
			if (moveX > 0)
			{
				x = collisionObject.x - width;
				return false;
			}
			if (moveX < 0)
			{
				x = collisionObject.x + collisionObject.width;
				return false;
			}
			if (moveY > 0)
			{
				y = collisionObject.y - height;
				return false;
			}
			if (moveY < 0)
			{
				y = collisionObject.y + collisionObject.height;
			}
			return true;
		}

		protected void applyGravity()
		{
			if (_speedY >= 1.0f && _speedY < 1.0f)
			{
				_speedY += 1.0f;
			}
			else
			{
				_speedY += 0.5f;
			}

			if (_speedY > 32.0f)
			{
				_speedY = 32.0f;
			}

			_isGrounded = false;
			if (!TryMove( 0, _speedY ))
			{
				if (_speedY > 0) _isGrounded = true;
				_speedY = 0;
			}
		}
	}
}
