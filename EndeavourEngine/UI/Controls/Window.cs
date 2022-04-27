using Endeavour.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endeavour.UI
{
	public class Window : Control
	{
		public string Title { get; set; }

		protected Panel titleBar;
		private int titleBarThickness = 24;

		public Window(Rectangle bounds, string title) : base(bounds)
		{
			Title = title;
			RelativeBounds = bounds;

			titleBar = new Panel(new Rectangle(0, 0, RelativeBounds.Width, titleBarThickness))
			{
				BackColor = Color.Red,
				Name = "TitleBar",
			};

			titleBar.MouseDownEH += (obj, sender) => GameServices.UIManager.SetFocusedWindow(this);

			// rough approx. of 10 pixels per char, can use MeasureString later
			var lbl = new Label(new Rectangle(0, 0, Title.Length * 10, titleBarThickness))
			{
				Text = Title,
				Font = GameServices.Fonts["Calibri"],
				ForeColor = Color.Blue,
				BackColor = Color.Yellow,
				Name = "CloseLabel",
			};
			titleBar.AddControl(lbl);

			var closeBtn = new Button(new Rectangle(RelativeBounds.Width - titleBarThickness, 0, titleBarThickness, titleBarThickness), "X", CloseControl)
			{
				BorderStyle = new BorderStyle { Color = Color.DarkGray, Thickness = 2 },
				BackColor = Color.Gray,
				Name = "CloseButton",
			};

			titleBar.AddControl(closeBtn);
			titleBar.DragEH += (obj, sender) => DragControl();

			AddControl(titleBar);
		}

		public void CloseControl()
		{
			Visible = false;
			Enabled = false;
			ShouldDispose = ShouldDisposeOnClose;
		}

		public void DragControl()
		{
			var mouseTravel = GameServices.InputManager.MouseTravelDistance();
			RelativeBounds.X += mouseTravel.X;
			RelativeBounds.Y += mouseTravel.Y;
			GameServices.UIManager.SetFocusedWindow(this);
		}

		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);

			if (UIManager.DebugRenderingEnabled)
			{
				sb.DrawString(GameServices.Fonts["Calibri"], ZIndex.ToString(), AbsoluteLocation.ToVector2(), Color.Black);
			}
			//debug
			//  var counter = 0f;
			//foreach (var c in Controls)
			//{
			//	var s = $"Name={c.Name} - ALoc={c.AbsoluteLocation} - RLoc={c.RelativeLocation} Zindex={c.ZIndex}";
			//	sb.DrawString(GameServices.Fonts["Calibri"], s, new Vector2(AbsoluteLocation.X, AbsoluteLocation.Y + counter), Color.White);
			//	counter += 20f;
			//}
		}
	}
}
