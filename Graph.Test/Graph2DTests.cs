using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Graph.Test
{
	public class Graph2DTests
	{
		[SetUp]
		public void Setup()
		{ }

		[Test]
		public void TestNodeEquality()
		{
			var p1 = new Point(10, 20);
			var p2 = new Point(10, 20);

			var n1 = new Node(p1);
			var n2 = new Node(p2);

			Assert.IsTrue(n1 == n2);
			Assert.IsTrue(n1.Equals(n2));

			Assert.AreEqual(p1, p2);
			Assert.AreEqual(n1, n2);
		}

		[Test]
		public void TestEdgeEquality()
		{
			var p1 = new Point(10, 20);
			var p2 = new Point(30, 40);

			var e1 = new Edge(new Node(p1), new Node(p2));
			var e2 = new Edge(new Node(p1), new Node(p2));

			Assert.IsTrue(e1 == e2);
			Assert.IsTrue(e1.Equals(e2));
			Assert.IsTrue(e1.GetHashCode() == e2.GetHashCode());

			Assert.AreEqual(e1, e2);
		}

		[Test]
		public void TestCannotAddDuplicateEdges()
		{
			var g = new Graph2D();

			var p1 = new Point(10, 20);
			var p2 = new Point(30, 40);

			var n1 = new Node(p1);
			var n2 = new Node(p2);

			var e1 = new Edge(n1, n2);
			var e2 = new Edge(n1, n2);
			var e3 = new Edge(new Node(p1), new Node(p2));
			var e4 = new Edge(new Node(new Point(10, 20)), new Node(new Point(30, 40)));

			Assert.IsTrue(g.AddEdge(e1));

			Assert.IsFalse(g.AddEdge(e1));
			Assert.IsFalse(g.AddEdge(e2));
			Assert.IsFalse(g.AddEdge(e3));
			Assert.IsFalse(g.AddEdge(e4));
		}
	}
}