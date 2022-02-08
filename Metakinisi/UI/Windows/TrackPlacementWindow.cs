using Metakinisi.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Metakinisi.UI
{
	public class TrackPlacementWindow : Window
	{
		public TrackPlacementTool tool = new();
		Panel currentTrackImage;

		public TrackPlacementWindow(Rectangle bounds, string text) : base(bounds, text)
		{
			var squareButton = new Rectangle(4, titleBar.Height + 4, 32, 32);

			// straight track
			var straightTrack = new Button(squareButton, "Straight", () => tool.cursorType = TrackType.Straight)
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
				BackgroundImage = new TilesetReference { TilesetName = "tileset", SourceRectangle = TilesetHelper.RectangleFromTilesetId(TilesetIds.Straight, 16, 16, 32) },
				DrawText = false,
			};
			AddControl(straightTrack);

			squareButton.Offset(32, 0);
			// corner track
			var cornerTrack = new Button(squareButton, "Corner", () => tool.cursorType = TrackType.Curve)
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
				BackgroundImage = new TilesetReference { TilesetName = "tileset", SourceRectangle = TilesetHelper.RectangleFromTilesetId(TilesetIds.Corner, 16, 16, 32), },
				DrawText = false,
			};
			AddControl(cornerTrack);

			// rotate left track
			squareButton.Offset(-32, 32);
			var rotateLeft = new Button(squareButton,
				"CCW",
				() => tool.cursorRotation = RotationHelpers.NextRotation[tool.cursorRotation])
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
				BackgroundImage = new TilesetReference { TilesetName = "ui", SourceRectangle = TilesetHelper.RectangleFromTilesetXY(0, 3, 24) },
				DrawText = false,
			};
			AddControl(rotateLeft);

			// rotate right
			squareButton.Offset(32, 0);
			var rotateRight = new Button(
				squareButton,
				"CW",
				() => tool.cursorRotation = RotationHelpers.PreviousRotation[tool.cursorRotation])
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
				BackgroundImage = new TilesetReference { TilesetName = "ui", SourceRectangle = TilesetHelper.RectangleFromTilesetXY(0, 10, 24) },
				DrawText = false,
			};
			AddControl(rotateRight);

			currentTrackImage = new Panel(new Rectangle(16, squareButton.Y + squareButton.Height + 16, 32, 32));
			currentTrackImage.BorderStyle = new BorderStyle { Color = Color.Black, Thickness = 2 };
			AddControl(currentTrackImage);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			var ctrlS = Controls.OfType<Button>().First(c => c.Text == "Straight");
			var ctrlC = Controls.OfType<Button>().First(c => c.Text == "Corner");

			if (tool.cursorType == TrackType.Straight)
			{
				ctrlS.BorderStyle.Color = Color.Red;
				ctrlC.BorderStyle.Color = Color.DarkGray;
				currentTrackImage.BackgroundImage = new TilesetReference
				{
					SourceRectangle = ctrlS.BackgroundImage.Value.SourceRectangle,
					TilesetName = ctrlS.BackgroundImage.Value.TilesetName,
					Rotation = tool.cursorRotation,
				};
			}
			else if (tool.cursorType == TrackType.Curve)
			{
				ctrlS.BorderStyle.Color = Color.DarkGray;
				ctrlC.BorderStyle.Color = Color.Red;
				currentTrackImage.BackgroundImage = new TilesetReference
				{
					SourceRectangle = ctrlC.BackgroundImage.Value.SourceRectangle,
					TilesetName = ctrlC.BackgroundImage.Value.TilesetName,
					Rotation = tool.cursorRotation,
				};
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);
		}
	}
}
