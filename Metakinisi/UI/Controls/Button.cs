using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Metakinisi.UI
{
	public class Button : Control
	{
		Color DefaultBackColor = Color.Gray;
		Color DefaultForeColor = Color.Gainsboro;
		Color HoverBackColor = Color.Gainsboro;
		Color HoverForeColor = Color.Gray;
		Color PressedBackColor = Color.White;
		Color PressedForeColor = Color.Black;

		Label lblText;
		//public bool IsToggle = false;
		//public bool ToggleValue = false;

		public Button(Rectangle bounds, string text, Action action) : base(bounds)
		{
			OnClick = action;
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
