using Graph;
using Microsoft.Xna.Framework.Graphics;
using Metakinisi.Tools;

namespace Metakinisi
{
	public interface IGridWorld : IUpdateable, IDrawable
	{
		Map Map { get; }

		Graph2D RailGraph { get; }

		RenderTarget2D RenderTarget { get; set; }

		void SetCurrentTool(ITool tool);
	}
}
