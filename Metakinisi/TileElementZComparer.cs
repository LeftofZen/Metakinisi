namespace Metakinisi
{
	public class TileElementZComparer : Comparer<ITileElement>
	{
		// sort by Z first then by ZIndex
		public override int Compare(ITileElement a, ITileElement b)
			=> a.Coordinates.Z == b.Coordinates.Z
				? a.ZIndex.CompareTo(b.ZIndex)
				: a.Coordinates.Z.CompareTo(b.Coordinates.Z);
	}
}
