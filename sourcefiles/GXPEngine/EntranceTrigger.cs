using System;
namespace GXPEngine
{
	public class EntranceTrigger : AnimationSprite
	{
		private bool _isTriggered;
		public EntranceTrigger( string filename, int columns, int rows ) : base( filename, columns, rows )
		{
		}

		public void Update()
		{
			collisionWithPlayer();
		}

		private void collisionWithPlayer()
		{
			foreach (GameObject other in GetCollisions())
			{
				if (other is Player)
				{
					Player player = other as Player;
					_isTriggered = true;
					NextFrame();
				}
			}
		}

		public bool GetTriggerStatus { get { return _isTriggered; } }
	}
}
