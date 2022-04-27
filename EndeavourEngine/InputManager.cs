using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Endeavour.Input
{
	public class InputManager : Interfaces.IUpdateable
	{
		public MouseState CurrentMouse => currentMouseState;
		private MouseState currentMouseState;
		private MouseState previousMouseState;

		public MouseState CurrentKeyboard => currentMouseState;
		private KeyboardState previousKeyboardState;
		private KeyboardState currentKeyboardState;

		#region IUpdateable

		public void Update(GameTime gameTime)
		{
			previousMouseState = currentMouseState;
			previousKeyboardState = currentKeyboardState;

			currentMouseState = Mouse.GetState();
			currentKeyboardState = Keyboard.GetState();
		}

		#endregion

		public bool IsMouseButtonPressed(MouseButtons mouseButtons)
			=> mouseButtons switch
			{
				MouseButtons.LeftButton => currentMouseState.LeftButton == ButtonState.Pressed,
				MouseButtons.RightButton => currentMouseState.RightButton == ButtonState.Pressed,
				MouseButtons.MiddleButton => currentMouseState.MiddleButton == ButtonState.Pressed,
				_ => throw new NotImplementedException($"[IsMouseButtonPressed] Handler for {mouseButtons} not implemented"),
			};

		public bool IsNewKeyPress(Keys key)
			=> previousKeyboardState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key);

		public Point MouseTravelDistance()
		{
			return new Point(
				currentMouseState.Position.X - previousMouseState.Position.X,
				currentMouseState.Position.Y - previousMouseState.Position.Y);
		}

		public bool IsNewMousePress(MouseButtons mouseButtons)
			=> mouseButtons switch
			{
				MouseButtons.LeftButton => IsNewMousePress(previousMouseState.LeftButton, currentMouseState.LeftButton),
				MouseButtons.RightButton => IsNewMousePress(previousMouseState.RightButton, currentMouseState.RightButton),
				MouseButtons.MiddleButton => IsNewMousePress(previousMouseState.MiddleButton, currentMouseState.MiddleButton),
				_ => throw new NotImplementedException($"[IsNewMousePress] Handler for {mouseButtons} not implemented"),
			};

		private static bool IsNewMousePress(ButtonState previous, ButtonState current)
			=> previous == ButtonState.Released && current == ButtonState.Pressed;

		public bool IsNewMouseRelease(MouseButtons mouseButtons)
			=> mouseButtons switch
			{
				MouseButtons.LeftButton => IsNewMouseRelease(previousMouseState.LeftButton, currentMouseState.LeftButton),
				MouseButtons.RightButton => IsNewMouseRelease(previousMouseState.RightButton, currentMouseState.RightButton),
				MouseButtons.MiddleButton => IsNewMouseRelease(previousMouseState.MiddleButton, currentMouseState.MiddleButton),
				_ => throw new NotImplementedException($"[IsNewMouseRelease] Handler for {mouseButtons} not implemented"),
			};

		private static bool IsNewMouseRelease(ButtonState previous, ButtonState current)
			=> previous == ButtonState.Pressed && current == ButtonState.Released;
	}
}
