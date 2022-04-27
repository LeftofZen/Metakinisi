using Microsoft.Xna.Framework;

namespace Endeavour.Tileset
{
	public struct TilesetReference
	{
		public string TilesetName;
		public Rectangle SourceRectangle;
		public Rotation Rotation = Rotation.Zero;

		public TilesetReference(string tilesetName, Rectangle sourceRectangle, Rotation rotation)
		{
			TilesetName = tilesetName;
			SourceRectangle = sourceRectangle;
			Rotation = rotation;
		}
	}
}
