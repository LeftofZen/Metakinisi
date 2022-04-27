namespace Metakinisi
{
	public interface ITileElement : Endeavour.Interfaces.IDrawable
	{
		public Point3 Coordinates { get; }

		// used to sort elements on the same Z level
		public int ZIndex { get; }
	}
}
