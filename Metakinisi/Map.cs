using Endeavour.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Metakinisi
{
	public class Map : Endeavour.Interfaces.IDrawable
	{
		public Map(int width, int height)
		{
			int z = 0;
			Tiles = new List<ITileElement>[height, width];
			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					Tiles[y, x] = new List<ITileElement>();

					// add a base layer
					var surfaceElement = new SurfaceElement(new Endeavour.Point3(x, y, z), SurfaceElementType.Grass);
					AddElement(surfaceElement);
				}
			}
		}
		public void AddElement(ITileElement element)
		{
			var listForCell = Tiles[element.Coordinates.Y, element.Coordinates.X];
			if (element is SurfaceElement && listForCell.OfType<SurfaceElement>().Any())
			{
				//throw new Exception("Cannot add a second surface element to this tile");
				GameServices.UIManager.ShowPopupMessage("Cannot add a second surface element to this tile");
				return;
			}
			if (element is TrackElement trackElement)
			{
				var list = listForCell.OfType<TrackElement>().Where(te => te.Coordinates.Z == element.Coordinates.Z);
				if (list.Any(te => te.rotation == trackElement.rotation && trackElement.type == te.type))
				{
					//GameServices.UIManager.ShowPopupMessage("Cannot add a second identical element to this tile");
					return;
				}
			}

			listForCell.Add(element);
			listForCell.Sort(zComparer);
		}

		TileElementZComparer zComparer = new();

		// should remain sorted on ITileElement.Z
		public List<ITileElement>[,] Tiles { get; }

		public int Width => Tiles.GetLength(1);
		public int Height => Tiles.GetLength(0);

		public bool DrawGrid = true;
		public Color GridColor = Color.DarkGreen;

		public SurfaceElement GetSurfaceElement(int x, int y)
			=> Tiles[y, x].OfType<SurfaceElement>().First();

		public void Draw(SpriteBatch sb)
		{
			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					foreach (var te in Tiles[y, x])
					{
						te.Draw(sb);
					}

					if (DrawGrid)
					{
						sb.DrawRectangle(
							x * GameServices.GridSize,
							y * GameServices.GridSize,
							GameServices.GridSize,
							GameServices.GridSize,
							GridColor,
							1);
					}
				}
			}
		}
	}
}
