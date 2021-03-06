using Endeavour.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endeavour.UI
{
	public class Button : Control
	{
		private Color DefaultBackColor = Color.Gray;
		private Color DefaultForeColor = Color.Gainsboro;
		private Color HoverBackColor = Color.Gainsboro;
		private Color HoverForeColor = Color.Gray;
		private Color PressedBackColor = Color.White;
		private Color PressedForeColor = Color.Black;
		private Label lblText;

		public string Text
		{
			get => lblText.Text;
			set => lblText.Text = value;
		}

		//public bool IsToggle = false;
		//public bool ToggleValue = false;

		public bool DrawText
		{
			get => lblText.Visible;
			set => lblText.Visible = value;
		}

		public Button(Rectangle bounds, string text, Action action) : base(bounds)
		{
			MouseClickEH += (obj, sender) => action();
			//IsToggle = toggle;

			var lblBounds = new Rectangle(0, 0, bounds.Width, bounds.Height);
			lblBounds.Inflate(-2, -2);
			lblText = new Label(lblBounds)
			{
				BackColor = DefaultBackColor,
				ForeColor = DefaultForeColor,
				Text = text,
				Font = GameServices.Fonts["Calibri"],
			};

			AddControl(lblText);
		}

		//public override void HandleInput()
		//{
		//	//base.HandleInput();

		//	IsPressed = ContainsMouse && GameServices.InputManager.IsMouseButtonPressed(Input.MouseButtons.LeftButton);
		//}

		//public override void Update(GameTime gameTime)
		//{
		//	base.Update(gameTime);
		//}

		public override void Draw(SpriteBatch sb)
		{
			if (IsPressed)
			{
				BackColor = PressedBackColor;
				ForeColor = PressedForeColor;
			}
			else if (IsHovering)
			{
				BackColor = HoverBackColor;
				ForeColor = HoverForeColor;
			}
			else
			{
				BackColor = DefaultBackColor;
				ForeColor = DefaultForeColor;
			}

			lblText.BackColor = BackColor;
			lblText.ForeColor = ForeColor;

			base.Draw(sb);
		}
	}

	//public class RadioButton : Control
	//{
	//	public Button(Rectangle bounds, string text, Action action) : base(bounds)
	//	{
	//		ClickAction = action;

	//		var lblBounds = new Rectangle(0, 0, bounds.Width, bounds.Height);
	//		lblBounds.Inflate(-2, -2);
	//		var lblText = new Label(lblBounds)
	//		{
	//			BackColor = this.BackColor,
	//			ForeColor = this.ForeColor,
	//			Text = text,
	//			Font = GameServices.Fonts["Calibri"],
	//		};

	//		AddControl(lblText);
	//	}

	//	public override void Update(GameTime gameTime)
	//	{
	//		base.Update(gameTime);
	//	}
	//}
}
