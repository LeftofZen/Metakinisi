using Microsoft.Xna.Framework;

namespace Metakinisi.UI
{
	public class Window : Control
	{
		public string Title { get; set; }

		Panel titleBar;
		int titleBarThickness = 24;

		public Window(Rectangle bounds, string title) : base(bounds)
		{
			Title = title;
			RelativeBounds = bounds;

			titleBar = new Panel(new Rectangle(0, 0, RelativeBounds.Width, titleBarThickness))
			{
				BackColor = Color.Red,
			};

			// rough approx. of 10 pixels per char, can use MeasureString later
			var lbl = new Label(new Rectangle(0, 0, Title.Length * 10, titleBarThickness))
			{
				Text = Title,
				Font = GameServices.Fonts["Calibri"],
				ForeColor = Color.Blue,
				BackColor = Color.Yellow,
			};
			titleBar.AddControl(lbl);

			var closeBtn = new Button(new Rectangle(RelativeBounds.Width - titleBarThickness, 0, titleBarThickness, titleBarThickness), "X", CloseControl)
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
			};
			titleBar.AddControl(closeBtn);
			titleBar.OnDrag = DragControl;

			AddControl(titleBar);
		}

		public void CloseControl()
		{
			Visible = false;
			Enabled = false;
		}

		public void DragControl()
		{
			var mouseTravel = GameServices.InputManager.MouseTravelDistance();
			RelativeBounds.X += mouseTravel.X;
			RelativeBounds.Y += mouseTravel.Y;
		}
	}
}
