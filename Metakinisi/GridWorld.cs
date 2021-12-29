using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Metakinisi
{
	public class GridWorld
	{
		MouseState previousMouseState;
		KeyboardState previousKeyboardState;

		GridCell[,] world;
		TrackElement[,] track;

		CellType selectedCellType = CellType.Dirt;
		MouseClickMode selectedMouseClickMode = MouseClickMode.None;
		const int gridSize = 32;
		const int worldSize = 16;
		Rotation cursorRotation;
		TrackType cursorType = TrackType.Straight;
		Vehicle train;

		public GridWorld()
		{
			world = new GridCell[worldSize, worldSize];
			track = new TrackElement[worldSize, worldSize];

			for (int y = 0; y < worldSize; ++y)
			{
				for (int x = 0; x < worldSize; ++x)
				{
					world[y, x] = new GridCell(CellType.Grass);
				}
			}

			for (int y = 0; y < worldSize; ++y)
			{
				for (int x = 0; x < worldSize; ++x)
				{
					track[y, x] = new TrackElement(new Point(x, y), TrackType.None);
				}
			}

			//track[3, 4] = new TrackElement { type = TrackType.Straight, rotation = Rotation.Zero };
			//track[0, 0] = new TrackElement { type = TrackType.Straight, rotation = Rotation.Ninety };

			train = new Vehicle(new Point(4, 4), 0.5f);
		}

		public void Update(GameTime gameTime)
		{
			var currMouseState = Mouse.GetState();
			var currKeyboardState = Keyboard.GetState();

			// update
			if (currMouseState.LeftButton == ButtonState.Pressed)
			{
				var cell = new Point(currMouseState.X / gridSize, currMouseState.Y / gridSize);
				if (cell.Y >= 0 && cell.Y < track.GetLength(1) && cell.X >= 0 && cell.X < track.GetLength(1))
				{
					track[cell.Y, cell.X] = new TrackElement(cell, cursorType, cursorRotation);
				}
			}

			if (currKeyboardState.IsKeyDown(Keys.R) && !previousKeyboardState.IsKeyDown(Keys.R))
			{
				cursorRotation = RotationHelpers.NextRotation[cursorRotation];
			}

			if (currKeyboardState.IsKeyDown(Keys.T) && !previousKeyboardState.IsKeyDown(Keys.T))
			{
				cursorType = cursorType == TrackType.Straight ? TrackType.Curve : TrackType.Straight;
			}

			if (currMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed)
			{
				// snap to track
				var cell = new Point(currMouseState.X / gridSize, currMouseState.Y / gridSize);
				if (cell.Y >= 0 && cell.Y < track.GetLength(1) && cell.X >= 0 && cell.X < track.GetLength(1))
				{
					train.PlaceInCell(cell, 0.5f); //position = track[cell.Y, cell.X].PositionFromLerpedPercent(0.5f);
				}

				//var dir = GameServices.Random.Next(0, 3);
				//train.direction = RotationHelpers.RotationAngles[(Rotation)dir];
				//train.PercentThroughTile = 0.5f;
			}

			if (currKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
			{
				train.Reverse();
			}

			train.Update(gameTime, track);

			previousMouseState = currMouseState;
			previousKeyboardState = currKeyboardState;
		}

		public void Draw(SpriteBatch sb)
		{
			// world cells
			for (int y = 0; y < worldSize; ++y)
			{
				for (int x = 0; x < worldSize; ++x)
				{
					world[y, x].Draw(sb, x, y, gridSize);
				}
			}

			// grid
			for (int y = 0; y < GameServices.Game.GraphicsDevice.PresentationParameters.BackBufferHeight; y += gridSize)
			{
				for (int x = 0; x < GameServices.Game.GraphicsDevice.PresentationParameters.BackBufferWidth; x += gridSize)
				{
					sb.DrawRectangle(x, y, gridSize, gridSize, Color.DarkKhaki, 1);
				}
			}

			// track cells
			for (int y = 0; y < worldSize; ++y)
			{
				for (int x = 0; x < worldSize; ++x)
				{
					track[y, x].Draw(sb, x, y, gridSize);
				}
			}

			train.Draw(sb);

			// current rotation
			sb.DrawString(GameServices.Fonts["Calibri"], cursorRotation.ToString() + " (R)", new Vector2(11, 11), Color.Black);
			sb.DrawString(GameServices.Fonts["Calibri"], cursorRotation.ToString() + " (R)", new Vector2(10, 10), Color.White);

			// current type
			sb.DrawString(GameServices.Fonts["Calibri"], cursorType.ToString() + " (T)", new Vector2(11, 31), Color.Black);
			sb.DrawString(GameServices.Fonts["Calibri"], cursorType.ToString() + " (T)", new Vector2(10, 30), Color.White);
		}
	}
}
