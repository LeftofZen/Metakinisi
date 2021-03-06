using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endeavour.UI
{
	public class Panel : Control
	{
		public RenderTarget2D renderTarget;

		public Panel(Rectangle bounds) : base(bounds)
		{

		}

		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);
			if (renderTarget != null)
			{
				sb.Draw(renderTarget, AbsoluteBounds, Color.White);
			}
		}
	}
}
