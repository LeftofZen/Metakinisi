using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Endeavour
{
	[DataContract]
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	public struct Point3 : IEquatable<Point3>
	{
		private static readonly Point3 zeroPoint;

		[DataMember]
		public int X;

		[DataMember]
		public int Y;

		[DataMember]
		public int Z;

		public static Point3 Zero => zeroPoint;

		internal string DebugDisplayString => X + "  " + Y + "  " + Z;

		public Point3(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Point3(int value)
		{
			X = value;
			Y = value;
			Z = value;
		}

		public static Point3 operator +(Point3 value1, Point3 value2) => new Point3(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);

		public static Point3 operator -(Point3 value1, Point3 value2) => new Point3(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);

		public static Point3 operator *(Point3 value1, Point3 value2) => new Point3(value1.X * value2.X, value1.Y * value2.Y, value1.Z * value2.Z);

		public static Point3 operator /(Point3 source, Point3 divisor) => new Point3(source.X / divisor.X, source.Y / divisor.Y, source.Z / divisor.Z);

		public static bool operator ==(Point3 a, Point3 b) => a.Equals(b);

		public static bool operator !=(Point3 a, Point3 b) => !a.Equals(b);

		public override bool Equals(object obj)
		{
			if (obj is Point3)
			{
				return Equals((Point3)obj);
			}

			return false;
		}

		public bool Equals(Point3 other) => X == other.X && Y == other.Y && Z == other.Z;

		public override int GetHashCode() => HashCode.Combine(X, Y, Z);

		public override string ToString() => $"{{X:{X} Y:{Y} Z:{Z}}}";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 ToVector3() => new Vector3(X, Y, Z);

		public void Deconstruct(out int x, out int y, out int z)
		{
			x = X;
			y = Y;
			z = Z;
		}
	}
}
