using Graph;
using Metakinisi.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Metakinisi.Tools
{
	public class TrackPlacementTool : ITool
	{
		public Rotation cursorRotation = Rotation.Zero;
		public TrackType cursorType = TrackType.Straight;

		public Edge? highlightedEdge;
		public Edge? ghostEdge;

		public void AddTrack(Point cellCoords, Rotation rotation, TrackType trackType)
		{

			//railGraph.AddEdge(ghostEdge.Value);
		}

		public void Update(GameTime gameTime, TrackElement[,] track, Graph2D railGraph)
		{
			var input = GameServices.InputManager;

			var cell = new Point(input.CurrentMouse.X / GameServices.GridSize, input.CurrentMouse.Y / GameServices.GridSize);

			highlightedEdge = null;
			// get highlighted rail cell
			if (cell.Y >= 0 && cell.Y < track.GetLength(1) && cell.X >= 0 && cell.X < track.GetLength(1))
			{
				var rect = new RectangleF(cell.X * GameServices.GridSize, cell.Y * GameServices.GridSize, GameServices.GridSize, GameServices.GridSize);
				foreach (var e in railGraph.Edges)
				{
					if (rect.Contains(e.Barycentre))
					{
						if (highlightedEdge == null)
							highlightedEdge = e;

						highlightedEdge
							= (e.Barycentre - input.CurrentMouse.Position.ToVector2()).LengthSquared()
							< (highlightedEdge?.Barycentre - input.CurrentMouse.Position.ToVector2())?.LengthSquared() ?
							e : highlightedEdge;
					}
				}
			}

			// place

			// new graph track placement
			if (cell.Y >= 0 && cell.Y < track.GetLength(1) && cell.X >= 0 && cell.X < track.GetLength(1))
			{
				var cellCoords = new Point(cell.X * GameServices.GridSize, cell.Y * GameServices.GridSize);

				if (cursorType == TrackType.Straight)
				{
					if (cursorRotation == Rotation.Zero || cursorRotation == Rotation.OneEighty)
					{
						ghostEdge = new Edge(
							//_ = railGraph.AddEdge(
							cellCoords + TrackElementHelpers.ConnectorLeft,
							cellCoords + TrackElementHelpers.ConnectorRight);
					}
					else
					{
						ghostEdge = new Edge(
							//_ = railGraph.AddEdge(
							cellCoords + TrackElementHelpers.ConnectorTop,
							cellCoords + TrackElementHelpers.ConnectorBottom);
					}
				}
				else if (cursorType == TrackType.Curve)
				{
					if (cursorRotation == Rotation.Zero)
					{
						ghostEdge = new Edge(
							//_ = railGraph.AddEdge(
							cellCoords + TrackElementHelpers.ConnectorLeft,
							cellCoords + TrackElementHelpers.ConnectorTop);
					}
					else if (cursorRotation == Rotation.Ninety)
					{
						ghostEdge = new Edge(
							//_ = railGraph.AddEdge(
							cellCoords + TrackElementHelpers.ConnectorBottom,
							cellCoords + TrackElementHelpers.ConnectorLeft);
					}
					else if (cursorRotation == Rotation.OneEighty)
					{
						ghostEdge = new Edge(
							//_ = railGraph.AddEdge(
							cellCoords + TrackElementHelpers.ConnectorRight,
							cellCoords + TrackElementHelpers.ConnectorBottom);
					}
					else
					{
						ghostEdge = new Edge(
							//_ = railGraph.AddEdge(
							cellCoords + TrackElementHelpers.ConnectorTop,
							cellCoords + TrackElementHelpers.ConnectorRight);
					}
				}

				if (input.IsMouseButtonPressed(MouseButtons.LeftButton) && ghostEdge != null)
				{
					railGraph.AddEdge(ghostEdge.Value);
					//AddTrack();
				}
			}

			if (input.IsNewKeyPress(Keys.R))
			{
				cursorRotation = RotationHelpers.NextRotation[cursorRotation];
			}

			if (input.IsNewKeyPress(Keys.T))
			{
				cursorType = cursorType == TrackType.Straight ? TrackType.Curve : TrackType.Straight;
			}

			if (input.IsNewKeyPress(Keys.D))
			{
				railGraph.Clear();
			}

			if (input.IsNewKeyPress(Keys.Delete))
			{
				if (highlightedEdge != null)
				{
					_ = railGraph.RemoveEdge(highlightedEdge.Value);
				}
			}
		}

		public void Draw(SpriteBatch sb)
		{
			// current rotation
			sb.DrawString(GameServices.Fonts["Calibri"], cursorRotation.ToString() + " (R)", new Vector2(11, 11), Color.Black);
			sb.DrawString(GameServices.Fonts["Calibri"], cursorRotation.ToString() + " (R)", new Vector2(10, 10), Color.White);

			// current type
			sb.DrawString(GameServices.Fonts["Calibri"], cursorType.ToString() + " (T)", new Vector2(11, 31), Color.Black);
			sb.DrawString(GameServices.Fonts["Calibri"], cursorType.ToString() + " (T)", new Vector2(10, 30), Color.White);

			if (highlightedEdge != null)
			{
				Graph2D.DrawDebugEdge(sb, highlightedEdge.Value, Color.Beige, 2);
			}

			if (ghostEdge != null)
			{
				Graph2D.DrawDebugEdge(sb, ghostEdge.Value, Color.PaleGoldenrod, 2);
			}
		}
	}
}
