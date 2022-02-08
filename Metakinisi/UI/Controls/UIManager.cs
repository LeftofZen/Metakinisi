using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Metakinisi.UI
{
	public class UIManager : IDrawable, IUpdateable
	{
		public List<Window> Windows = new(); // ordered by zorder
		public Control? FocusedWindow;

		public bool HighlightFocusedControl = true;

		public int MaxZIndex = int.MaxValue;

		public UIManager()
		{

		}

		//IEnumerable<Control> GetControls() => GetControls(Windows);
		//IEnumerable<Control> GetAllControls() => GetControls(Windows);

		//IEnumerable<Control> GetAllControls()
		//{
		//	foreach (var v in Windows)
		//	{
		//		foreach (Control c in v.Controls)
		//		{
		//			yield return c;
		//			GetControls(c);
		//		}
		//	}
		//}

		//static IEnumerable<Control> GetControls(Control parent)
		//{
		//	foreach (var c in parent.Controls)
		//	{
		//		yield return c;
		//		GetControls(c);
		//	}
		//}

		public void Draw(SpriteBatch sb)
		{
			foreach (var window in Windows.Where(w => w.Visible).OrderBy(w => w.ZIndex))
			{
				window.Draw(sb);
			}

			if (HighlightFocusedControl)
			{
				var fc = FocusedWindow;
				if (fc != null)
				{
					var rect = fc.AbsoluteBounds;
					rect.Inflate(3, 3);
					sb.DrawRectangle(rect, Color.DarkRed, 5);
				}
			}
		}

		public void ShowPopupMessage(string message)
		{
			var window = new Window(new Rectangle(400, 400, 400, 200), $"Error: {message}");
			window.ShouldDisposeOnClose = true;
			window.ZIndex = 100;
			FocusedWindow = window;

			//TopLevelControl.AddControl(window);
		}

		public void Update(GameTime gameTime)
		{
			foreach (var w in Windows)
			{
				w.Update(gameTime);
			}

			if (FocusedWindow == null)
			{
				SetFocusedWindow();
			}

			// FocusedControl can still be null if you drag off the edge
			// of the entire game window 
			if (FocusedWindow != null && !FocusedWindow.IsDragging)
			{
				SetFocusedWindow();
			}

			FocusedWindow?.HandleInput(gameTime);
		}

		public void SetFocusedWindow(Window w = null)
		{
			if (w is null)
			{
				w = Windows
				.Where(c => c.AbsoluteBounds.Contains(GameServices.InputManager.CurrentMouse.Position))
				.OrderByDescending(c => c.ZIndex)
				.FirstOrDefault();

				if (w is not null)
				{
					foreach (var ww in Windows.Where(ww => ww.Title != "Main Game"))
					{
						ww.ZIndex = 10;
					}
					Windows.Where(ww => ww.Title == "Main Game").Single().ZIndex = 0;

					w.ZIndex = 20;
					FocusedWindow = w;
				}
				return;
			}
			else
			{
				FocusedWindow.ZIndex = 10;
				FocusedWindow = w;
				FocusedWindow.ZIndex = 20;
				//var oldWindow = FocusedWindow;
				//var FocusedZIndex = FocusedWindow.ZIndex;
				//FocusedWindow = w;

				//oldWindow.ZIndex = w.ZIndex;
				//w.ZIndex = FocusedZIndex;


				//var currZIndex = FocusedWindow.ZIndex;
				//foreach (var ww in Windows.Where(ww => ww.Title != "Main Game"))
				//{
				//	ww.ZIndex = 10;
				//}
				//FocusedWindow.ZIndex = MaxZIndex;
			}
		}
	}
}
