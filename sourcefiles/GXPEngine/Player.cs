using System;
namespace GXPEngine
{
	public class Player : Entity
	{
		private float _playerSpeed;
		private int _playerLives;
		private readonly int _maximumLives;
		private int _score = 0;
		private bool hasKey = false;
		private bool _isDead;

		// Describes the speed of changing a frame
		private float _frame = 1;
		private bool _firstFrame = true;
		private float _groundHitSpeed;

		private int _airFrame;
		// The frames 5 up to 8 are resposible for the wall jump animation
		private int _wallJumpFrame = 5;
		private float _wallJumpDirection;
		private float _previousWallJumpDirection;
		private bool _isDebugMode = false;

		private Time _time;
		private enum playerState
		{
			Idle,
			Sliding,
			InAir,
			Running,
			Dying
		}

		private playerState _playerState;
		public Player( EntityType type ) : base( "skeleton_full.png", 10, 3 )
		{
			_playerSpeed = 4f;
			_isDead = false;
			_type = type;
			_maximumLives = 5;
			_playerLives = _maximumLives;
			SetFrame( 1 );
			_frame = 0.0f;

			_time = new Time();
		}

		private new void Update()
		{

			if (parent != null && !parent.visible)
				return;

			if (!_isDead)
			{
				_frame += 0.555f;

				_playerState = playerState.InAir;
				applyGravity();
				movement();
				checkOtherCollisions();


				switch (_playerState)
				{
					case playerState.Running:
						Running();
						break;
					case playerState.Sliding:
						Sliding();
						break;
					case playerState.Idle:
						Idle();
						break;
					case playerState.InAir:
						InAir();
						break;
				}
				wallJump();
			}
			checkDamage();
		}
		/// <summary>
		/// Moves the player by the set input
		/// </summary>
		private void movement()
		{

			bool moving = true;
			//float tempPlayerSpeed;
			if (moving)
			{

				if (_isGrounded && (Input.GetKey( Key.W ) || Input.GetKey( Key.UP )))
				{
					_speedY -= 16.0f;
				}


				//Toggle dedugmode;
				if (Input.GetKey( Key.T ) && !_isDebugMode) _isDebugMode = true;
				if (Input.GetKey( Key.T ) && _isDebugMode) _isDebugMode = false;

				//Only if debufmode is toggled
				if (_isDebugMode && Input.GetKey( Key.W ))
				{
					TryMove( _playerSpeed, 0 );
				}
				if (_isDebugMode && Input.GetKey( Key.S ))
				{
					TryMove( -_playerSpeed, 0 );
				}

				if (Input.GetKey( Key.LEFT ) || Input.GetKey( Key.A ))
				{
					if (!Input.GetKey( Key.RIGHT ) && TryMove( -_playerSpeed, 0 ) == true)
					{
						if (_isGrounded)
						{
							_playerState = playerState.Running;
						}
						//Flip/mirror the player
						scaleX = -1;
						//Set the origin to the right edge of the player
						SetOrigin( width, 0 );

					}
					else if (!_isGrounded && (_speedY > 2.0f || Input.GetKeyDown( Key.SPACE ) || Input.GetKeyDown( Key.UP )))
					{
						_playerState = playerState.Sliding;
					}
				}

				if (Input.GetKey( Key.RIGHT ) || Input.GetKey( Key.D ))
				{
					if (!Input.GetKey( Key.LEFT ) && TryMove( _playerSpeed, 0 ) == true)
					{
						if (_isGrounded)
						{
							_playerState = playerState.Running;
						}
						scaleX = 1;
						SetOrigin( 0, 0 );
					}
					else if (!_isGrounded && (_speedY > 2.0f || Input.GetKeyDown( Key.SPACE ) || Input.GetKeyDown( Key.UP )))
					{
						_playerState = playerState.Sliding;
					}
				}
			}
		}

		/// <summary>
		/// Applies the gravity to the player 
		/// </summary>
		protected new void applyGravity()
		{
			if (!_isDebugMode)
			{
				if (Math.Abs( _speedY - 1.5f ) < 0.05)
				{
					_speedY += 1.5f;
				}
				else
				{
					_speedY += 1.0f;
				}
				// the value shouldn't be greater than the amount of pixel of a tile
				if (_speedY > 15.5f)
				{
					_speedY = 15.5f;
				}

				_isGrounded = false;
				if (!TryMove( 0, _speedY ))
				{
					//The same as: if(speedY > 0) isGrounded=true;
					//called pipe equals
					_isGrounded |= _speedY > 0;

					_wallJumpDirection = 0.0f;
					_groundHitSpeed = _speedY;
					_speedY = 0;
					_playerState = playerState.Idle;
				}
			}
		}
		/// <summary>
		/// Walls the jump.
		/// </summary>
		private void wallJump()
		{
			// if the frame is less than 8 and wallJumpDirection equals to previousWallDirection
			if (_wallJumpFrame < 8 && Math.Abs( _wallJumpDirection - _previousWallJumpDirection ) > 0.005)
			{

				_speedY -= 10.0f / _wallJumpFrame;
				if (_speedY < -6.0f)
				{
					_speedY = -6.0f;
				}

				_wallJumpFrame++;
			}
			else
			{
				_previousWallJumpDirection = _wallJumpDirection;
				_wallJumpFrame = 8;
			}
		}

		private void checkDamage()
		{
			if (_isGrounded && _groundHitSpeed >= 16 || _isDead)
			{
				_isDead = true;

				Console.WriteLine( "ded, D E D" );

				if (_firstFrame)
				{
					currentFrame = 22;
					_firstFrame = false;
				}
				else
				{
					_frame = 0.0f;
					NextFrame();
					//After the last frame is played, the level will be reseted
					if (currentFrame > 25)
					{
						currentFrame = 25;
						_playerLives -= 1;
						Level level = parent as Level;
						level.ResetLevel();
					}
				}
			}
		}
		/// <summary>
		/// Checks other collisions like the pumpkins, keys and spikes
		/// </summary>
		private void checkOtherCollisions()
		{
			foreach (var other in GetCollisions())
			{
				if (other is Pickup)
				{
					Pickup pickup = other as Pickup;
					switch (pickup.GetPickupType)
					{
						case PickupType.Pumpkin:
							_score++;
							pickup.Destroy();
							break;
						case PickupType.Key:
							hasKey = true;
							pickup.Destroy();
							break;
					}
				}
				if (other is Tile)
				{
					Tile tile = other as Tile;
					if (tile.GetTileType == TileType.Deadly)
					{
						Level level = parent as Level;
						level.ResetLevel();
					}
					if (tile.GetTileType == TileType.Exit && hasKey)
					{
						Level level = parent as Level;
						level.NextLevel();
					}
				}
			}
		}
		/// <summary>
		/// Resets the player. (Public since the level will be reseted in the level class)
		/// </summary>
		public void resetPlayer()
		{
			_isDead = false;
			_playerState = playerState.Idle;
			_score = 0;
			_frame = 0.0f;
			SetFrame( 1 );
		}

		private void Running()
		{
			if (_frame > 1)
			{
				_frame = 0.0f;
				NextFrame();
				if (currentFrame > 6)
				{
					currentFrame = 1;
				}
			}
		}

		private void Sliding()
		{
			_speedY -= 2.0f;

			if (_speedY < 4.0f)
			{
				_speedY = 4.0f;
			}
			currentFrame = 20;

			if (Input.GetKey( Key.SPACE ))
			{
				_wallJumpFrame = 1;
				_wallJumpDirection = -scaleX;
			}
		}

		private void InAir()
		{
			_playerSpeed = 4;
			//if the player moves downwards...
			if (_speedY < 0)
			{
				currentFrame = 17;

				//set the speed to:
				_playerSpeed = 6;
				if (_airFrame >= 2)
				{
					currentFrame = 18;
				}
				_airFrame++;
			}

			if (_speedY > 1.0f && !_isGrounded)
			{
				currentFrame = 19;
				_airFrame = 0;
			}
		}

		private void Idle()
		{
			currentFrame = 0;
		}

		public int GetPlayerLives { get { return _playerLives; } }
		public int GetMaximumLives { get { return _maximumLives; } }
		public int GetScore { get { return _score; } }
	}
}
