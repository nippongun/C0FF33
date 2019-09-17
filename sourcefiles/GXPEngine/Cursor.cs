namespace GXPEngine
{
	public class Cursor : Sprite
	{
		public Cursor(string sprite) : base(sprite)
		{
			SetOrigin(width / 2, height / 2);
		}

		void Update() 
		{
            x = Input.mouseX;
			y = Input.mouseY;
		}
	}
}
