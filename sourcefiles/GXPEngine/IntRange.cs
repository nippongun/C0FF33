using System;
namespace GXPEngine
{
	[Serializable]
	public class IntRange
	{
		private readonly Random r;
		public int _min;
		public int _max;
		public IntRange(int min, int max)
		{
			_min = min;
			_max = max;
			r = new Random();
		}

		public int Random 
		{
			get{ return r.Next(_min, _max); }
		}
	}
}
