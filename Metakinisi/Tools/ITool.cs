using Graph;
using Microsoft.Xna.Framework;

namespace Metakinisi.Tools
{
	public interface ITool : IDrawable
	{
		void Update(GameTime gameTime, Graph2D railGraph);
	}
}
