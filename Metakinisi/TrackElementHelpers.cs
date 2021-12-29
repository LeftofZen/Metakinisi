using Microsoft.Xna.Framework;

namespace Metakinisi
{
	public static class TrackElementHelpers
	{
		public static readonly Point ConnectorTop = new(16, 0);
		public static readonly Point ConnectorBottom = new(16, 32);
		public static readonly Point ConnectorLeft = new(0, 16);
		public static readonly Point ConnectorRight = new(32, 16);

		public static Dictionary<Point, List<Point>> StraightHorizontalInternalConnections = new()
		{
			{ ConnectorLeft, new List<Point>{ ConnectorRight } },
			{ ConnectorRight, new List<Point> { ConnectorLeft } }
		};

		public static Dictionary<Point, List<Point>> StraightVerticalInternalConnections = new()
		{
			{ ConnectorTop, new List<Point>{ ConnectorBottom } },
			{ ConnectorBottom, new List<Point> { ConnectorTop } }
		};

		public static HashSet<Point> ExternalConnections = MakeExternalConnections();

		public static HashSet<Point> MakeExternalConnections() => new()
		{ ConnectorLeft, ConnectorRight, ConnectorTop, ConnectorBottom };

		public static Dictionary<Point, Point> AdjacentConnector = new()
		{
			{ ConnectorTop, ConnectorBottom },
			{ ConnectorBottom, ConnectorTop },
			{ ConnectorLeft, ConnectorRight },
			{ ConnectorRight, ConnectorLeft },
		};

	}
}
