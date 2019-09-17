namespace GXPEngine
{
	public class Level : GameObject
	{
		private LevelGenerator _levelGenerator;
		private readonly Player _player;
		private HUD _hud;
		private GameObject _focus;
		private int _stage;
		private int _lives;
		private int _score;
		private int _currentLevel;
		private Menu _menu;
		public Level( int stage ) : base()
		{
			_player = new Player( EntityType.Player );
			_stage = stage;
			_levelGenerator = new LevelGenerator( _player, _stage );

			AddChild( _levelGenerator );
			AddChild( _player );
			_focus = _player;

			_hud = new HUD( _player );
			game.AddChild( _hud );

			_menu = new Menu();
			AddChild( _menu );
			_menu.visible = false;
			_menu.SetXY( this.x, this.y );
			// Calls the updateCamera method after all Update-methods are called to prevent a delayed camera
			game.OnAfterStep += updateCamera;
		}

		void Update()
		{
			if (!visible)
			{
				return;
			}
			// Pauses the game (in that case the level) if pressed 'M'
			if (Input.GetKey( Key.M ))
			{
				visible = false;
				foreach (GameObject child in parent.GetChildren())
				{
					if (child is Menu)
					{
						Menu menu = child as Menu;
						AddChild( menu );
						menu.visible = true; 
					}
				}
			}

		}

		private void updateCamera()
		{
			if (_focus != null)
			{
				x = (game.width / 2 - _focus.x);
				y = (game.height / 2 - _focus.y);
			}
		}

		private void gameOver()
		{
			Destroy();
		}

		public void ResetLevel()
		{
			_lives = _player.GetMaximumLives;

			Level level = new Level( _currentLevel );
			game.AddChild( level );
			Destroy();
		}

		public void NextLevel()
		{

		}

		public Player GetPlayer()
		{
			return _player;
		}
	}
}
