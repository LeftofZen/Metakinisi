using Graph;
using Microsoft.Xna.Framework.Graphics;
using Metakinisi.Tools;

namespace Metakinisi
{
	public interface IGridWorld : Endeavour.Interfaces.IUpdateable, Endeavour.Interfaces.IDrawable
	{
		GameState GameState { get; }

		RenderTarget2D RenderTarget { get; set; }

		void SetCurrentTool(ITool tool);
	}
}
