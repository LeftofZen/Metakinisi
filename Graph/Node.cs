using Microsoft.Xna.Framework;

namespace Graph
{
	public struct Node : IEquatable<Node>
	{
		public Point Position { get; init; }
		public readonly HashSet<Edge> Edges = new();

		public Node(Point position)
		{
			Position = position;
		}

		public bool RemoveEdge(Edge e)
		{
			return Edges.Remove(e);
		}

		public bool Equals(Node other)
		{
			return Position.Equals(other.Position);
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode();
		}

		public static bool operator ==(Node a, Node b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Node a, Node b)
		{
			return !(a == b);
		}

	}
}
