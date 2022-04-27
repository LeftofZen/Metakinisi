using Endeavour.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endeavour.UI
{
	public class Label : Control
	{
		public string Text;
		public SpriteFont Font;

		public Label(Rectangle bounds) : base(bounds) => DrawBackground = false;

		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);

			if (!string.IsNullOrEmpty(Text))
			{
				sb.DrawString(Font, Text, AbsoluteBounds, TextAlignment.Center, ForeColor);
			}
		}
	}
}
