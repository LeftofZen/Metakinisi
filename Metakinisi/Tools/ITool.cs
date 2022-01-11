using Graph;
using Microsoft.Xna.Framework;

namespace Metakinisi.Tools
{
	public interface ITool : IDrawable
	{
		void Update(GameTime gameTime, TrackElement[,] track, Graph2D railGraph);
	}
}
