using Endeavour.Services;
using Endeavour.Tileset;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endeavour.UI
{
	public interface IImage
	{
		void Draw(SpriteBatch sb, Rectangle bounds);
	}

	public struct TilesetImage : IImage
	{
		public TilesetImage(TilesetReference? tilesetReference) => this.tilesetReference = tilesetReference;

		public TilesetReference? tilesetReference { get; set; } = null;

		public void Draw(SpriteBatch sb, Rectangle bounds)
		{
			if (tilesetReference is null)
			{
				return;
			}

			var centre = new Vector2(tilesetReference.Value.SourceRectangle.Width / 2f, tilesetReference.Value.SourceRectangle.Height / 2f);
			var renderRect = bounds;
			renderRect.Offset(bounds.Width / 2f, bounds.Height / 2f);

			// image
			sb.Draw(
				GameServices.Textures[tilesetReference.Value.TilesetName],
				renderRect,
				tilesetReference.Value.SourceRectangle,
				Color.White,
				RotationHelpers.RotationAnglesForDrawing[tilesetReference.Value.Rotation],
				centre,
				SpriteEffects.None,
				0f);
		}
	}
}
