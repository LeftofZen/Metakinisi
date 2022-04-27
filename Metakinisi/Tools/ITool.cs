using Graph;
using Microsoft.Xna.Framework;

namespace Metakinisi.Tools
{
	public interface ITool : Endeavour.Interfaces.IDrawable
	{
		void Update(GameTime gameTime, Graph2D railGraph);
	}
}
