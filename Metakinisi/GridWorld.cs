using Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Metakinisi.Input;
using Metakinisi.Tools;

namespace Metakinisi
{
	public interface IGridWorld : IUpdateable, IDrawable
	{
		TrackElement[,] Track { get; }
		GridCell[,] World { get; }
		Graph2D RailGraph { get; }

		RenderTarget2D RenderTarget { get; set; }

		void SetCurrentTool(ITool tool);
	}

	public class GridWorld : IGridWorld
	{
		public RenderTarget2D RenderTarget { get; set; }

		GridCell[,] world;
		TrackElement[,] track;
		Graph2D railGraph;

		#region IGridWorld

		public TrackElement[,] Track => track;
		public GridCell[,] World => world;
		public Graph2D RailGraph => railGraph;

		#endregion

		CellType selectedCellType = CellType.Dirt;
		MouseClickMode selectedMouseClickMode = MouseClickMode.None;
		public ITool CurrentTool;
		Vehicle train;

		public int Height { get; init; }
		public int Width { get; init; }

		public GridWorld(int width, int height)
		{
			Width = width;
			Height = height;

			GenWorld();
			railGraph = new Graph2D();

			train = new Vehicle(new Point(4, 4), 0.5f);
		}

		public void SetCurrentTool(ITool tool)
		{
			this.CurrentTool = tool;
		}

		void GenWorld()
		{
			world = new GridCell[Height, Width];
			track = new TrackElement[Height, Width];

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					world[y, x] = new GridCell(CellType.Grass);
				}
			}

			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					track[y, x] = new TrackElement(new Point(x, y), TrackType.None);
				}
			}
		}

		public void Update(GameTime gameTime)
		{
			var input = GameServices.InputManager;
			input.Update(gameTime);

			if (CurrentTool != null)
			{
				CurrentTool.Update(gameTime, track, railGraph);
			}

			if (input.IsNewMousePress(MouseButtons.RightButton))
			{
				var cell = new Point(input.CurrentMouse.X / GameServices.GridSize, input.CurrentMouse.Y / GameServices.GridSize);
				if (cell.Y >= 0 && cell.Y < track.GetLength(1) && cell.X >= 0 && cell.X < track.GetLength(1))
				{
					// snap to track
					if (cell.Y >= 0 && cell.Y < track.GetLength(1) && cell.X >= 0 && cell.X < track.GetLength(1))
					{
						train.PlaceInCell(cell, 0.5f); //position = track[cell.Y, cell.X].PositionFromLerpedPercent(0.5f);
					}
				}

				//var dir = GameServices.Random.Next(0, 3);
				//train.direction = RotationHelpers.RotationAngles[(Rotation)dir];
				//train.PercentThroughTile = 0.5f;
			}

			if (input.IsNewKeyPress(Keys.E))
			{
				train.Reverse();
			}

			train.Update(gameTime, track);
		}

		public void Draw(SpriteBatch sb)
		{
			// Set the render target
			sb.GraphicsDevice.SetRenderTarget(RenderTarget);

			// Draw the scene
			sb.GraphicsDevice.Clear(Color.CornflowerBlue);
			sb.Begin();
			DrawReal(sb);
			sb.End();

			// Drop the render target
			sb.GraphicsDevice.SetRenderTarget(null);
		}

		void DrawReal(SpriteBatch sb)
		{
			// world cells
			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					world[y, x].Draw(sb, x, y, GameServices.GridSize);
				}
			}

			// grid
			for (int y = 0; y < GameServices.Game.GraphicsDevice.PresentationParameters.BackBufferHeight; y += GameServices.GridSize)
			{
				for (int x = 0; x < GameServices.Game.GraphicsDevice.PresentationParameters.BackBufferWidth; x += GameServices.GridSize)
				{
					sb.DrawRectangle(x, y, GameServices.GridSize, GameServices.GridSize, Color.DarkKhaki, 1);
				}
			}

			// track cells
			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					track[y, x].Draw(sb, x, y, GameServices.GridSize);
				}
			}

			train.Draw(sb);

			CurrentTool?.Draw(sb);

			// reverse train
			sb.DrawString(GameServices.Fonts["Calibri"], train.reversed.ToString() + " (E)", new Vector2(11, 51), Color.Black);
			sb.DrawString(GameServices.Fonts["Calibri"], train.reversed.ToString() + " (E)", new Vector2(10, 50), Color.White);

			// clear graph
			sb.DrawString(GameServices.Fonts["Calibri"], "Clear Graph" + " (D)", new Vector2(11, 71), Color.Black);
			sb.DrawString(GameServices.Fonts["Calibri"], "Clear Graph" + " (D)", new Vector2(10, 70), Color.White);

			railGraph.Draw(sb);
		}
	}
}
