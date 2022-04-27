using Endeavour.Services;
using Endeavour.Tileset;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace Metakinisi
{
	public class TrackElement : ITileElement
	{
		public int ZIndex => 1;

		public static TrackElement None = new TrackElement(new Point3(-1, -1, -1), TrackType.None);

		public TrackType type = TrackType.None;
		public Rotation rotation;
		Point3 cellCoordinates;

		public Point3 Coordinates { get => cellCoordinates; }
		public Point worldCoordinates => new Point(cellCoordinates.X * 32, cellCoordinates.Y * 32);

		public TrackElement(Point3 cellCoordinates, TrackType type, Rotation rotation = Rotation.Zero)
		{
			this.cellCoordinates = cellCoordinates;
			this.type = type;
			this.rotation = rotation;

			var con = TrackElementHelpers.ConnectorBottom;

			if (type == TrackType.Straight)
			{
				if (rotation == Rotation.Zero || rotation == Rotation.OneEighty)
				{
					InternalConnections = TrackElementHelpers.StraightHorizontalInternalConnections;
				}
				else
				{
					InternalConnections = TrackElementHelpers.StraightVerticalInternalConnections;
				}
			}
		}

		public float Length =>
			type switch
			{
				TrackType.Straight => 1f,
				TrackType.None => 0f,
				//TrackType.Curve => MathHelper.Pi / 4f, // based on circle
				TrackType.Curve => MathF.Sqrt(0.5f), // based on straight diagonal
				_ => throw new NotSupportedException(""),
			};

		public int LengthInWorld => (int)(Length * 32);

		// old
		public Dictionary<Point, List<Point>> Paths => paths ??= GetPaths();
		Dictionary<Point, List<Point>> paths;

		// new
		public Dictionary<Point, List<Point>> InternalConnections;

		internal Dictionary<Point, List<Point>> GetPaths()
		{
			Dictionary<Point, List<Point>> output = new();

			switch (type)
			{
				case TrackType.Straight:
					{
						switch (rotation)
						{
							case Rotation.Zero:
								output.Add(TrackElementHelpers.ConnectorLeft, new List<Point>() { TrackElementHelpers.ConnectorRight });
								break;
							case Rotation.Ninety:
								output.Add(TrackElementHelpers.ConnectorBottom, new List<Point>() { TrackElementHelpers.ConnectorTop });
								break;
							case Rotation.OneEighty:
								output.Add(TrackElementHelpers.ConnectorRight, new List<Point>() { TrackElementHelpers.ConnectorLeft });
								break;
							case Rotation.TwoSeventy:
								output.Add(TrackElementHelpers.ConnectorTop, new List<Point>() { TrackElementHelpers.ConnectorBottom });
								break;
						}
						break;
					}
				case TrackType.Curve:
					{
						switch (rotation)
						{
							case Rotation.Zero:
								output.Add(TrackElementHelpers.ConnectorLeft, new List<Point>() { TrackElementHelpers.ConnectorTop });
								break;
							case Rotation.Ninety:
								output.Add(TrackElementHelpers.ConnectorBottom, new List<Point>() { TrackElementHelpers.ConnectorLeft });
								break;
							case Rotation.OneEighty:
								output.Add(TrackElementHelpers.ConnectorRight, new List<Point>() { TrackElementHelpers.ConnectorBottom });
								break;
							case Rotation.TwoSeventy:
								output.Add(TrackElementHelpers.ConnectorTop, new List<Point>() { TrackElementHelpers.ConnectorRight });
								break;
						}
						break;
					}
			}

			return output;
		}

		//public Point3 GetConnectedTrackElement(Point3 exitPoint, bool reversed)
		//{
		//	var direction = (reversed ? -1 : 1);
		//	if (exitPoint == TrackElementHelpers.ConnectorTop)
		//	{
		//		return new Point(cellCoordinates.X, cellCoordinates.Y - 1 * direction);
		//	}
		//	if (exitPoint == TrackElementHelpers.ConnectorBottom)
		//	{
		//		return new Point(cellCoordinates.X, cellCoordinates.Y + 1 * direction);
		//	}
		//	if (exitPoint == TrackElementHelpers.ConnectorLeft)
		//	{
		//		return new Point(cellCoordinates.X - 1 * direction, cellCoordinates.Y);
		//	}
		//	else //if (exitPoint == TrackElementHelpers.ConnectorRight)
		//	{
		//		return new Point(cellCoordinates.X + 1 * direction, cellCoordinates.Y);
		//	}
		//}

		public Vector2 PositionFromLerpedPercent(float percent)
		{
			if (!Paths.Any())
			{
				return Vector2.Zero;
			}

			return PositionFromLerpedPercent(percent, Paths.First().Key, Paths.First().Value.First());
		}

		public Vector2 PositionFromLerpedPercent(float percent, Point entry, Point exit)
		{
			//if (type == TrackType.Straight)
			//{
			return new Vector2(
				MathHelper.Lerp(entry.X, exit.X, percent) + worldCoordinates.X,
				MathHelper.Lerp(entry.Y, exit.Y, percent) + worldCoordinates.Y);
			//}

			//if (type == TrackType.Curve)
			//{

			//}

		}

		public void Draw(SpriteBatch sb)
		{
			if (type == TrackType.None)
				return;

			const int cellSize = Constants.GridSize;
			var x = Coordinates.X;
			var y = Coordinates.Y;

			switch (type)
			{
				case TrackType.None:
					break;
				case TrackType.Straight:
				case TrackType.Curve:

					var bounds = new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize);
					var tilesetReference = new TilesetReference
					{
						TilesetName = "tileset",
						SourceRectangle = TilesetHelper.RectangleFromTilesetId(type == TrackType.Straight ? TilesetIds.Straight : TilesetIds.Corner, 16, 16, 32),
						Rotation = rotation,
					};
					var centre = new Vector2(tilesetReference.SourceRectangle.Width / 2f, tilesetReference.SourceRectangle.Height / 2f);
					var renderRect = bounds;
					renderRect.Offset(bounds.Width / 2f, bounds.Height / 2f);

					var angle = RotationHelpers.RotationAnglesForDrawing[tilesetReference.Rotation];
					sb.Draw(
						GameServices.Textures[tilesetReference.TilesetName],
						renderRect,
						tilesetReference.SourceRectangle,
						Color.White,
						angle,
						centre,
						SpriteEffects.None,
						0f);
					//DrawArrow(sb, x, y, cellSize, rotation);
					break;
			}

			var offset = new Vector2(x * cellSize, y * cellSize);

			// draw paths
			//foreach (var (entry, v) in InternalConnections)
			//{
			//	foreach (var exit in v)
			//	{
			//		sb.DrawLine(entry.ToVector2() + offset, exit.ToVector2() + offset, Color.Purple, 2);
			//	}
			//}
		}

		static readonly List<Vector2> ArrowVertices = new()
		{
			new Vector2(-4, 8),
			new Vector2(-4, -2),
			new Vector2(-10, -2),
			new Vector2(-1, -12),
			new Vector2(1, -12),
			new Vector2(10, -2),
			new Vector2(4, -2),
			new Vector2(4, 8),
			new Vector2(-4, 8),
		};

		public static void DrawArrow(SpriteBatch sb, int x, int y, int cellSize, Rotation rotation)
		{
			var poly = new Polygon(ArrowVertices);

			poly.Rotate((float)RotationHelpers.RotationAngles[rotation]);

			// move to centre of cell/grid
			poly.Offset(new Vector2(cellSize / 2, cellSize / 2));

			sb.DrawPolygon(new Vector2(x * cellSize, y * cellSize), poly, Color.Black);
		}

		public static void DrawArrow(SpriteBatch sb, Vector2 position, int cellSize, Rotation rotation)
		{
			var poly = new Polygon(ArrowVertices);

			poly.Rotate((float)RotationHelpers.RotationAngles[rotation]);

			// move to centre of cell/grid
			//poly.Offset(new Vector2(cellSize / 2, cellSize / 2));

			sb.DrawPolygon(position, poly, Color.Black, 2);
		}
	}
}
