using System.Collections.Generic;
using System.Drawing;
namespace GXPEngine
{
	public class HUD : GameObject
	{
		private readonly Player _player;
		private readonly List<Heart> _listHearts;
		private TextBoard _textBoardHearts;
		private readonly TextBoard _textBoardCoordinates;
		public HUD( Player pPlayer )
		{
			_player = pPlayer;

			_textBoardHearts = new TextBoard( _player.GetMaximumLives * 64, 64, Color.Black );
			AddChild( _textBoardHearts );

			_textBoardCoordinates = new TextBoard( 128, 64, Color.Black );
			_textBoardCoordinates.x = game.width - _textBoardCoordinates.width;
			AddChild( _textBoardCoordinates );
			_listHearts = new List<Heart>();

			for (int i = 0; i < _player.GetMaximumLives; i++)
			{
				Heart heart = new Heart();
				heart.x = i * heart.width + 32;
				heart.y = 32;
				_listHearts.Add( heart );
				AddChild( heart );
			}
		}

		public void Update()
		{
			_textBoardCoordinates.SetText( "X: " + Mathf.Round(_player.x / 16) + ", Y: " + Mathf.Round(_player.y / 16 ) + "\nScore: "+_player.GetScore);

			for (int i = 0; i < _player.GetMaximumLives; i++)
			{
				if (i < _player.GetPlayerLives)
				{
					_listHearts[i].visible = true;
				}
				else
				{
					_listHearts[i].visible = false;
				}
			}
		}
	}
}
