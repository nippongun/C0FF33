using System.Drawing;
namespace GXPEngine
{
	public class TextBoard : Canvas
	{
		Color _color;
		public TextBoard( int width, int height, Color color ) : base( width, height )
		{
			_color = color;
			SetText( "" );
		}

		public void SetText( string text )
		{
			graphics.Clear( _color );
			graphics.DrawString( text, SystemFonts.DefaultFont, Brushes.White, 0, 0 );
		}

		public Color Color
		{
			get
			{
				return _color;
			}

			set
			{
				_color = value;

			}
		}
	}
}
