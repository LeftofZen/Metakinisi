using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Graph
{
	public enum GraphDrawMode { Normal, Debug }

	public class Graph2D
	{
		public readonly HashSet<Node> Nodes = new();
		public readonly HashSet<Edge> Edges = new();
		public GraphDrawMode DrawMode { get; set; } = GraphDrawMode.Normal;

		public void Clear()
		{
			Nodes.Clear();
			Edges.Clear();
		}

		public bool AddEdge(Edge e)
		{
			return Edges.Add(e);
		}

		public bool AddEdge(Node a, Node b)
		{
			return Edges.Add(new Edge(a, b));
		}

		public bool AddEdge(Point a, Point b)
		{
			return Edges.Add(new Edge(new Node(a), new Node(b)));
		}

		public bool RemoveEdge(Edge e)
		{
			_ = e.A.RemoveEdge(e);
			_ = e.B.RemoveEdge(e);
			return Edges.Remove(e);
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (var e in Edges)
			{
				if (DrawMode == GraphDrawMode.Debug)
					DrawDebugEdge(sb, e, Color.White, 2);
				else
					DrawRail(sb, e, Color.White, 3);
			}
		}

		public static void DrawDebugEdge(SpriteBatch sb, Edge e, Color color, int thickness)
		{
			sb.DrawLine(e.A.Position.ToVector2(), e.B.Position.ToVector2(), color, thickness);
			sb.DrawPoint(e.A.Position.ToVector2(), Color.Blue, thickness * 2);
			sb.DrawPoint(e.B.Position.ToVector2(), Color.Blue, thickness * 2);
		}

		public static void DrawRail(SpriteBatch sb, Edge e, Color color, int thickness)
		{
			// draw rail bed
			sb.DrawLine(e.A.Position.ToVector2(), e.B.Position.ToVector2(), Color.Tan, thickness * 8);

			var length = (int)(e.A.Position.ToVector2() - e.B.Position.ToVector2()).Length();
			var railGap = thickness * 1.5f;
			var sleeperCount = 4;
			var spacing = length / sleeperCount;
			var spacingOffset = spacing / 2;

			var a = e.A.Position.ToVector2();
			var b = e.B.Position.ToVector2();

			// vertical
			if (e.A.Position.X == e.B.Position.X)
			{
				// draw sleepers
				for (var i = 0; i < sleeperCount; ++i)
				{
					sb.DrawLine(a + new Vector2(-thickness * 3, spacing * i + spacingOffset), a + new Vector2(thickness * 3, spacing * i + spacingOffset), Color.SaddleBrown, thickness);
				}

				// draw rails
				sb.DrawLine(a + new Vector2(railGap, 0), b + new Vector2(railGap, 0), Color.SlateGray, thickness);
				sb.DrawLine(a - new Vector2(railGap, 0), b - new Vector2(railGap, 0), Color.SlateGray, thickness);
			}
			// horizontal
			else
			{
				// draw sleepers
				for (var i = 0; i < sleeperCount; ++i)
				{
					sb.DrawLine(a + new Vector2(spacing * i + spacingOffset, -thickness * 3), a + new Vector2(spacing * i + spacingOffset, thickness * 3), Color.SaddleBrown, thickness);
				}

				// draw rails
				sb.DrawLine(a + new Vector2(0, railGap), b + new Vector2(0, railGap), Color.SlateGray, thickness);
				sb.DrawLine(a - new Vector2(0, railGap), b - new Vector2(0, railGap), Color.SlateGray, thickness);
			}
		}
	}
}
