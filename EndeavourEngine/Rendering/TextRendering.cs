using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endeavour.Rendering
{
	public static class TextRendering
	{
		public static void DrawString(this SpriteBatch sb, SpriteFont font, string text, Rectangle bounds, TextAlignment align, Color color)
		{
			var size = font.MeasureString(text);
			var pos = bounds.Center.ToVector2();
			var origin = size * 0.5f;

			if (align.HasFlag(TextAlignment.Left))
			{
				origin.X += bounds.Width / 2 - size.X / 2;
			}

			if (align.HasFlag(TextAlignment.Right))
			{
				origin.X -= bounds.Width / 2 - size.X / 2;
			}

			if (align.HasFlag(TextAlignment.Top))
			{
				origin.Y += bounds.Height / 2 - size.Y / 2;
			}

			if (align.HasFlag(TextAlignment.Bottom))
			{
				origin.Y -= bounds.Height / 2 - size.Y / 2;
			}

			sb.DrawString(font, text, pos.ToPoint().ToVector2(), color, 0, origin.ToPoint().ToVector2(), 1, SpriteEffects.None, 0);
		}
	}
}
