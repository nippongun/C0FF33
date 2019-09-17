using System.Drawing;
namespace GXPEngine
{
	public class Button : AnimationSprite
	{
		private TextBoard _textBoard;
		private string _text;
		public Button( string text ) : base( "button.png", 2, 1 )
		{
			SetFrame( 0 );
			_text = text;
			//_textBoard = new TextBoard( 128, 64, Color.Transparent );
			//_textBoard.width = this.width;
			//_textBoard.height = this.height;
			//_textBoard.SetText( _text );
			//_textBoard.SetXY( x, y );
			//AddChild( _textBoard );
		}
	}
}
