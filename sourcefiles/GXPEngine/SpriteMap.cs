using System;
using System.Drawing;
using GXPEngine.Core;
using System.IO;
namespace GXPEngine
{
	public class SpriteMap
	{
		private Bitmap[] images;
		public SpriteMap(string filename, int columns, int rows)
		{
			try
			{
				images = new Bitmap[columns * rows];
				Image image = Image.FromFile(filename);
				int frameSize = image.Width / columns;
				int index = 0;

				for (int x = 0; x < columns; x++)
				{
					for (int y = 0; y < rows; y++)
					{
						images[index] = new Bitmap(frameSize, frameSize);
						Graphics graphics = Graphics.FromImage(images[index]);
						graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, frameSize, frameSize), new System.Drawing.Rectangle(x * frameSize, y * frameSize, frameSize, frameSize), GraphicsUnit.Pixel);
						graphics.Dispose();
						index++;
					}

				}
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine(e);
			}
		}

		public Bitmap GetFrame(int index) 
		{
			return images[index];
		}

		public int GetLength() 
		{
			return images.Length;
		}
	}
}