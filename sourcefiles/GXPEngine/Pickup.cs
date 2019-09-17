using System;
namespace GXPEngine
{
	public class Pickup : AnimationSprite
	{
		PickupType _pickupType;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:GXPEngine.Pickup"/> class.
		/// </summary>
		/// <param name="pickupType">Pickup type.</param>
		public Pickup( PickupType pickupType ) : base( "CaveTileset.png", 16, 10 )
		{
			_pickupType = pickupType;
		}

		public PickupType GetPickupType { get { return _pickupType; } }
	}
}
