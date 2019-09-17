using System;
namespace GXPEngine
{
	public class Menu : GameObject
	{
		Button _startButton;
		Button _exitButton;
		private bool _isPaused;
		public Menu() : base()
		{
			_startButton = new Button( "Continue!" );
			_startButton.SetXY( game.width / 2f, game.height / 2 - _startButton.width + 8 );
			_exitButton = new Button( "Exit!" );
			_exitButton.SetXY( game.width / 2f, game.height / 2 - _exitButton.width - 8 );

			AddChild( _startButton );
			AddChild( _exitButton );
		}

		public void Update()
		{
			if (!_isPaused)
			{
				visible = false;
				return;
			}

			if (_startButton.HitTestPoint( Input.mouseX, Input.mouseY ))
			{
				_startButton.NextFrame();
				if (Input.GetMouseButtonDown( 0 ))
				{
					startGame();
				}
			}
			if (_exitButton.HitTestPoint( Input.mouseX, Input.mouseY ))
			{
				_exitButton.NextFrame();
				if (Input.GetMouseButtonDown( 0 ))
				{
					exitGame();
				}
			}
		}

		private void startGame()
		{
			_isPaused = false;

			foreach (GameObject child in game.GetChildren())
			{
				if (child is Menu)
				{
					child.visible = false;
				}
				if (child is Level)
				{
					child.visible = true;
					this.visible = false;
					return;
				}
			}
		}

		private void exitGame()
		{
			game.Destroy();
		}
	}
}
