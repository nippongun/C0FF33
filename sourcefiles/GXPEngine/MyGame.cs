using GXPEngine;                                // GXPEngine contains the engine
public class MyGame : Game
{
	Level _level;
	public MyGame( int screenWidth, int screenHeight ) : base( screenWidth, screenHeight, false )       // Creates a window that's 800x600 and NOT fullscreen
	{
		_level = new Level( 1 );
        AddChild( _level );
	}

	void Update()
	{

	}

	static void Main()                          // Main() is the first method that's called when the program is running
	{
		new MyGame( 1600, 900 ).Start();                   // Creates a "MyGame" and starts it
	}

	public void CreateLevel()
	{
		_level = new Level( 1 );
		AddChild( _level );
	}

	public Level Level
	{
		get
		{
			return _level;
		}

		set
		{
			_level = value;
		}
	}

}