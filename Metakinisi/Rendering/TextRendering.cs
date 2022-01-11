using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Metakinisi.Rendering
{
	[Flags]
	public enum Alignment
	{
		Center = 0,
		Left = 1,
		Right = 2,
		Top = 4,
		Bottom = 8,
	}

	public static class TextRendering
	{
		public static void DrawString(this SpriteBatch sb, SpriteFont font, string text, Rectangle bounds, Alignment align, Color color)
		{
			Vector2 size = font.MeasureString(text);
			Vector2 pos = bounds.Center.ToVector2();
			Vector2 origin = size * 0.5f;

			if (align.HasFlag(Alignment.Left))
				origin.X += bounds.Width / 2 - size.X / 2;

			if (align.HasFlag(Alignment.Right))
				origin.X -= bounds.Width / 2 - size.X / 2;

			if (align.HasFlag(Alignment.Top))
				origin.Y += bounds.Height / 2 - size.Y / 2;

			if (align.HasFlag(Alignment.Bottom))
				origin.Y -= bounds.Height / 2 - size.Y / 2;

			sb.DrawString(font, text, pos.ToPoint().ToVector2(), color, 0, origin.ToPoint().ToVector2(), 1, SpriteEffects.None, 0);
		}
	}
}
