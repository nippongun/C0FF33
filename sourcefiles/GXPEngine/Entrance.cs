using System.Collections.Generic;
namespace GXPEngine
{
	public class Entrance : AnimationSprite
	{
		EntranceTrigger _entranceTrigger;
		Room _enteringRoom;
		private int animCount;
		public Entrance(EntranceTrigger entranceTrigger, Room enteringRoom ) : base( "Entrance.png", 1, 1 )
		{
			_entranceTrigger = entranceTrigger;
			enteringRoom = _enteringRoom;
			animCount = 0;
		}

		private void Update()
		{
			if (_entranceTrigger.GetTriggerStatus)
			{
				handleAnimation();
				Destroy();
			}
		}

		private void handleAnimation()
		{
			if (_entranceTrigger.GetTriggerStatus && _entranceTrigger != null)
			{
				_entranceTrigger.SetFrame( 0 );
			}
			else if (_entranceTrigger.GetTriggerStatus)
			{
				for (int i = 0; i < animCount; i++)
				{
					_entranceTrigger.NextFrame();
				}
			}
		}
	}
}
