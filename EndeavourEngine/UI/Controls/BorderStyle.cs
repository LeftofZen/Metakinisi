using Microsoft.Xna.Framework;

namespace Endeavour.UI
{
	public struct BorderStyle
	{
		public BorderStyle(Color color, int thickness)
		{
			Color = color;
			Thickness = thickness;
		}

		public Color Color { get; set; } = Color.Red;
		public int Thickness { get; set; } = 3;
	}
}
