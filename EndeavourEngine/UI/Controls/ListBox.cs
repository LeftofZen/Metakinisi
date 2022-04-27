using Endeavour.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Endeavour.UI
{
	public class ListBox<T> : Control where T : class
	{
		public List<T> Items { get; set; } = new();

		public SpriteFont Font { get; set; }

		public int ListItemHeight { get; set; } = 20;

		public int SelectedElementIndex { get; set; } = -1;

		public T SelectedElement => SelectedElementIndex == -1 ? null : Items[SelectedElementIndex];

		public int HighlightedElementIndex { get; set; } = -1;

		public ListBox(Rectangle bounds) : base(bounds)
		{
		}

		public override void HandleInput()
		{
			base.HandleInput();

			if (ContainsMouse)
			{
				var translatedMousePos = GameServices.InputManager.CurrentMouse.Position - AbsoluteLocation;
				HighlightedElementIndex = translatedMousePos.Y / ListItemHeight;

				if (GameServices.InputManager.IsNewMousePress(Input.MouseButtons.LeftButton))
				{
					SelectedElementIndex = HighlightedElementIndex;
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);
			var offset = 0;

			var itemsToRender = AbsoluteBounds.Height / ListItemHeight;
			for (var i = 0; i < itemsToRender; i++)
			{
				var backColor = i == SelectedElementIndex ? Color.Yellow : BackColor;
				sb.FillRectangle(new RectangleF(AbsoluteBounds.X, AbsoluteBounds.Y + offset, AbsoluteBounds.Width - 5, ListItemHeight), backColor);

				var borderColor = i == HighlightedElementIndex ? Color.Yellow : Color.Black;
				sb.DrawRectangle(new RectangleF(AbsoluteBounds.X, AbsoluteBounds.Y + offset, AbsoluteBounds.Width - 5, ListItemHeight), borderColor);

				if (i < Items.Count)
				{
					var item = Items[i];
					sb.DrawString(Font, item.ToString(), new Vector2(AbsoluteBounds.X, AbsoluteBounds.Y + offset), ForeColor);
				}

				offset += ListItemHeight;
			}
		}
	}
}
