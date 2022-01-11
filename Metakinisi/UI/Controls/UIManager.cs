using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Metakinisi.UI
{
	public class UIManager : IDrawable, IUpdateable
	{
		public Control TopLevelControl;
		public Control? FocusedControl;

		public bool HighlightFocusedControl = true;

		IEnumerable<Control> AllControls => GetControls(TopLevelControl);

		static IEnumerable<Control> GetControls(Control parent)
		{
			foreach (var c in parent.Controls)
			{
				yield return c;
				GetControls(c);
			}
		}

		public void Draw(SpriteBatch sb)
		{
			TopLevelControl.Draw(sb);

			if (HighlightFocusedControl)
			{
				var fc = FocusedControl;
				if (fc != null)
				{
					sb.DrawRectangle(fc.AbsoluteBounds, Color.Red, 5);
				}
			}
		}

		public void ShowPopupMessage(string message)
		{
			var window = new Window(new Rectangle(400, 400, 400, 200), $"Error: {message}");
			window.ShouldDisposeOnClose = true;
			window.ZIndex = 100;
			FocusedControl = window;
			TopLevelControl.AddControl(window);
		}

		public void Update(GameTime gameTime)
		{
			if (FocusedControl == null)
			{
				SetFocusedControl();
			}

			// FocusedControl can still be null if you drag off the edge
			// of the entire game window 
			if (FocusedControl != null && !FocusedControl.IsDragging)
			{
				SetFocusedControl();
			}

			FocusedControl?.HandleInput(gameTime);
			TopLevelControl.Update(gameTime);
		}

		private void SetFocusedControl()
		{
			FocusedControl = AllControls
				//.OfType<Window>()
				.Where(c => c.AbsoluteBounds.Contains(GameServices.InputManager.CurrentMouse.Position))
				.OrderByDescending(c => c.ZIndex)
				.FirstOrDefault();
		}
	}
}
