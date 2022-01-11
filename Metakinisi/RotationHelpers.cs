using Microsoft.Xna.Framework;

namespace Metakinisi
{
	public static class RotationHelpers
	{
		public static readonly Dictionary<Rotation, Rotation> NextRotation = new()
		{
			{ Rotation.Zero, Rotation.Ninety },
			{ Rotation.Ninety, Rotation.OneEighty },
			{ Rotation.OneEighty, Rotation.TwoSeventy },
			{ Rotation.TwoSeventy, Rotation.Zero },
		};

		public static readonly Dictionary<Rotation, Rotation> PreviousRotation = new()
		{
			{ Rotation.Ninety, Rotation.Zero },
			{ Rotation.OneEighty, Rotation.Ninety },
			{ Rotation.TwoSeventy, Rotation.OneEighty },
			{ Rotation.Zero, Rotation.TwoSeventy },
		};

		// '0' degrees is to the 'right', or +ve x-axis as in 2pi on cartesian plane
		public static readonly Dictionary<Rotation, float> RotationAngles = new()
		{
			{ Rotation.Zero, (float)(1.0 * Math.PI / 2.0) },
			{ Rotation.Ninety, (float)(0.0 * Math.PI / 2.0) },
			{ Rotation.OneEighty, (float)(3.0 * Math.PI / 2.0) },
			{ Rotation.TwoSeventy, (float)(2.0 * Math.PI / 2.0) },
		};

		public static readonly Dictionary<Rotation, Vector2> RotationVectors = new()
		{
			{ Rotation.Zero, new Vector2(1, 0) },
			{ Rotation.Ninety, new Vector2(0, -1) },
			{ Rotation.OneEighty, new Vector2(-1, 0) },
			{ Rotation.TwoSeventy, new Vector2(0, 1) },
		};
	}
}
