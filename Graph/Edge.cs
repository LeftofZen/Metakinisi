using Microsoft.Xna.Framework;

namespace Graph
{
	public enum EdgeDirectionForLerp { AtoB, BtoA }

	public struct Edge : IEquatable<Edge>
	{
		public Node A { get; init; }
		public Node B { get; init; }

		public Edge(Point a, Point b) : this(new Node(a), new Node(b))
		{ }

		public Edge(Node a, Node b)
		{
			A = a;
			B = b;

			_ = A.Edges.Add(this);
			_ = B.Edges.Add(this);
		}

		public float Length
			=> (A.Position - B.Position).ToVector2().Length();

		public Vector2 PositionFromLerp(float percent, EdgeDirectionForLerp direction)
		{
			percent = direction == EdgeDirectionForLerp.AtoB ? percent : 1 - percent;
			return new Vector2(
				MathHelper.Lerp(A.Position.X, B.Position.X, percent),
				MathHelper.Lerp(A.Position.Y, B.Position.Y, percent));
		}

		public bool Equals(Edge other)
		{
			return ((A == other.A && B == other.B) || (A == other.B && B == other.A));
		}

		public static bool operator ==(Edge a, Edge b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Edge a, Edge b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return A.GetHashCode() ^ B.GetHashCode();
		}

		public Vector2 Barycentre
			=> (A.Position.ToVector2() + B.Position.ToVector2()) / 2f;
	}
}