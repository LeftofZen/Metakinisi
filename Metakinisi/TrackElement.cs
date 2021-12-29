using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace Metakinisi
{
	public class TrackElement
	{
		public static TrackElement None = new TrackElement(new Point(-1, -1), TrackType.None);

		public TrackType type = TrackType.None;
		public Rotation rotation;
		public Point cellCoordinates;
		public Point worldCoordinates => new Point(cellCoordinates.X * 32, cellCoordinates.Y * 32);

		public TrackElement(Point cellCoordinates, TrackType type, Rotation rotation = Rotation.Zero)
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

		public Point GetConnectedTrackElement(Point exitPoint, bool reversed)
		{
			var direction = (reversed ? -1 : 1);
			if (exitPoint == TrackElementHelpers.ConnectorTop)
			{
				return new Point(cellCoordinates.X, cellCoordinates.Y - 1 * direction);
			}
			if (exitPoint == TrackElementHelpers.ConnectorBottom)
			{
				return new Point(cellCoordinates.X, cellCoordinates.Y + 1 * direction);
			}
			if (exitPoint == TrackElementHelpers.ConnectorLeft)
			{
				return new Point(cellCoordinates.X - 1 * direction, cellCoordinates.Y);
			}
			else //if (exitPoint == TrackElementHelpers.ConnectorRight)
			{
				return new Point(cellCoordinates.X + 1 * direction, cellCoordinates.Y);
			}
		}

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

		public void Draw(SpriteBatch sb, int x, int y, int cellSize)
		{
			if (type == TrackType.None)
				return;

			var thickness = cellSize / 4;
			var straightRectHoro = new Rectangle(x * cellSize, (y * cellSize) + (cellSize / 2) - (thickness / 2), cellSize, thickness);
			var straightRectVert = new Rectangle((x * cellSize) + (cellSize / 2) - (thickness / 2), y * cellSize, thickness, cellSize);

			switch (type)
			{
				case TrackType.None:
					break;
				case TrackType.Straight:
					switch (rotation)
					{
						case Rotation.Zero:
						case Rotation.OneEighty:
							sb.FillRectangle(straightRectHoro, Color.LightSlateGray);
							break;
						case Rotation.Ninety:
						case Rotation.TwoSeventy:
							sb.FillRectangle(straightRectVert, Color.LightSlateGray);
							break;
					}
					//DrawArrow(sb, x, y, cellSize, rotation);
					break;
				case TrackType.Curve:
					//
					break;
			}

			var offset = new Vector2(x * cellSize, y * cellSize);

			// draw paths
			foreach (var (entry, v) in InternalConnections)
			{
				foreach (var exit in v)
				{
					sb.DrawLine(entry.ToVector2() + offset, exit.ToVector2() + offset, Color.Purple, 2);
				}
			}

			//foreach (var path in Paths)
			//{
			//	sb.DrawCircle(path.Key.ToVector2() + offset, 4, 8, Color.Blue, 3);
			//	foreach (var endpoint in path.Value)
			//	{
			//		sb.DrawLine(path.Key.ToVector2() + offset, endpoint.ToVector2() + offset, Color.Purple, 1);
			//		sb.DrawCircle(endpoint.ToVector2() + offset, 4, 8, Color.Blue, 3);
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
