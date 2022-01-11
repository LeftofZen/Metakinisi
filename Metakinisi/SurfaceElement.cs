using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Metakinisi
{
	public enum SurfaceElementType
	{
		Grass,
		Sand,
		Water,
	}

	public class SurfaceElement : ITileElement
	{
		public int ZIndex => 0;

		public SurfaceElement(Point3 coordinates, SurfaceElementType surfaceType)
		{
			Coordinates = coordinates;
			SurfaceType = surfaceType;
		}

		public static Dictionary<SurfaceElementType, Color> SurfaceColors = new Dictionary<SurfaceElementType, Color>()
		{
			{ SurfaceElementType.Grass, Color.Green },
			{ SurfaceElementType.Sand, Color.SandyBrown },
			{ SurfaceElementType.Water, Color.CadetBlue },
		};

		public Point3 Coordinates { get; set; }

		public SurfaceElementType SurfaceType { get; set; }

		public void Draw(SpriteBatch sb)
		{
			sb.FillRectangle(
				new Rectangle(Coordinates.X * GameServices.GridSize, Coordinates.Y * GameServices.GridSize, GameServices.GridSize, GameServices.GridSize),
				SurfaceColors[SurfaceType]);
		}
	}
}
