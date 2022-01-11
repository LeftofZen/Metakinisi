using Metakinisi.Tools;
using Microsoft.Xna.Framework;

namespace Metakinisi.UI
{
	public class TrackPlacementWindow : Window
	{
		public TrackPlacementTool tool = new();

		public TrackPlacementWindow(Rectangle bounds, string text) : base(bounds, text)
		{
			// straight track
			var straightTrack = new Button(new Rectangle(4, 28, GameServices.GridSize * 2, GameServices.GridSize), "Straight", () => tool.cursorType = TrackType.Straight)
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
			};
			AddControl(straightTrack);

			// corner track
			var cornerTrack = new Button(new Rectangle(4 + (GameServices.GridSize * 2), 28, GameServices.GridSize * 2, GameServices.GridSize), "Corner", () => tool.cursorType = TrackType.Curve)
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
			};
			AddControl(cornerTrack);

			// rotate left track
			var rotateLeft = new Button(
				new Rectangle(4, 28 + GameServices.GridSize, GameServices.GridSize * 2, GameServices.GridSize),
				"CCW",
				() => tool.cursorRotation = RotationHelpers.NextRotation[tool.cursorRotation])
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
			};
			AddControl(rotateLeft);

			// rotate right
			var rotateRight = new Button(
				new Rectangle(4 + (GameServices.GridSize * 2), 28 + GameServices.GridSize, GameServices.GridSize * 2, GameServices.GridSize),
				"CW",
				() => tool.cursorRotation = RotationHelpers.PreviousRotation[tool.cursorRotation])
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
			};
			AddControl(rotateRight);
		}
	}
}
