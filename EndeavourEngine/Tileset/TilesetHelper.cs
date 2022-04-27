using Microsoft.Xna.Framework;

namespace Endeavour.Tileset
{
	public enum TilesetIds
	{
		Straight = 0,
		Crossroads = 1,
		Corner = 2,
	}
	public static class TilesetHelper
	{
		public static Rectangle RectangleFromTilesetId(TilesetIds id, int widthInTiles, int heightInTiles, int tileSize)
		{
			var nid = (int)id;
			var x = nid % widthInTiles;
			var y = nid / heightInTiles;
			return RectangleFromTilesetXY(x, y, tileSize);
		}
		public static Rectangle RectangleFromTilesetPoint(Point point, int tileSize)
			=> RectangleFromTilesetXY(point.X, point.Y, tileSize);

		public static Rectangle RectangleFromTilesetXY(int x, int y, int tileSize)
			=> new(
				x * tileSize,
				y * tileSize,
				tileSize,
				tileSize);
	}
}
