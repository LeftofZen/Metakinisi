using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Metakinisi.UI
{
	public class UIManager : IDrawable, IUpdateable
	{
		public Control TopLevelControl;
		public Control? FocusedControl;

		public bool HighlightFocusedControl = false;

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

		public void Update(GameTime gameTime)
		{
			if (FocusedControl == null)
			{
				UpdateFocusedControl();
			}

			// FocusedControl can still be null if you drag off the edge
			// of the entire game window 
			if (FocusedControl != null && !FocusedControl.IsDragging)
			{
				UpdateFocusedControl();
			}

			FocusedControl?.HandleInput(gameTime);
			TopLevelControl.Update(gameTime);
		}

		private void UpdateFocusedControl()
		{
			FocusedControl = AllControls
				//.OfType<Window>()
				.Where(c => c.AbsoluteBounds.Contains(GameServices.InputManager.CurrentMouse.Position))
				.OrderByDescending(c => c.ZIndex)
				.FirstOrDefault();
		}
	}
}
