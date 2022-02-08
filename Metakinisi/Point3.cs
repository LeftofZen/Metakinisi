using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Metakinisi
{
	//
	// Summary:
	//     Describes a 2D-point.
	[DataContract]
	[DebuggerDisplay("{DebugDisplayString,nq}")]
	public struct Point3 : IEquatable<Point3>
	{
		private static readonly Point3 zeroPoint;

		//
		// Summary:
		//     The x coordinate of this Microsoft.Xna.Framework.Point.
		[DataMember]
		public int X;

		//
		// Summary:
		//     The y coordinate of this Microsoft.Xna.Framework.Point.
		[DataMember]
		public int Y;

		//
		// Summary:
		//     The y coordinate of this Microsoft.Xna.Framework.Point.
		[DataMember]
		public int Z;

		//
		// Summary:
		//     Returns a Microsoft.Xna.Framework.Point with coordinates 0, 0.
		public static Point3 Zero => zeroPoint;

		internal string DebugDisplayString => X + "  " + Y + "  " + Z;

		//
		// Summary:
		//     Constructs a point with X,Y,Z from threee values.
		//
		// Parameters:
		//   x:
		//     The x coordinate in 3d-space.
		//
		//   y:
		//     The y coordinate in 3d-space.
		//
		//   z:
		//     The y coordinate in 3d-space.
		public Point3(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		//
		// Summary:
		//     Constructs a point with X,Y,Z set to the same value.
		//
		// Parameters:
		//   value:
		//     The x, y and z coordinates in 3d-space.
		public Point3(int value)
		{
			X = value;
			Y = value;
			Z = value;
		}

		//
		// Summary:
		//     Adds two points.
		//
		// Parameters:
		//   value1:
		//     Source Microsoft.Xna.Framework.Point on the left of the add sign.
		//
		//   value2:
		//     Source Microsoft.Xna.Framework.Point on the right of the add sign.
		//
		// Returns:
		//     Sum of the points.
		public static Point3 operator +(Point3 value1, Point3 value2)
		{
			return new Point3(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);
		}

		//
		// Summary:
		//     Subtracts a Microsoft.Xna.Framework.Point from a Microsoft.Xna.Framework.Point.
		//
		// Parameters:
		//   value1:
		//     Source Microsoft.Xna.Framework.Point on the left of the sub sign.
		//
		//   value2:
		//     Source Microsoft.Xna.Framework.Point on the right of the sub sign.
		//
		// Returns:
		//     Result of the subtraction.
		public static Point3 operator -(Point3 value1, Point3 value2)
		{
			return new Point3(value1.X - value2.X, value1.Y - value2.Y, value1.Z - value2.Z);
		}

		//
		// Summary:
		//     Multiplies the components of two points by each other.
		//
		// Parameters:
		//   value1:
		//     Source Microsoft.Xna.Framework.Point on the left of the mul sign.
		//
		//   value2:
		//     Source Microsoft.Xna.Framework.Point on the right of the mul sign.
		//
		// Returns:
		//     Result of the multiplication.
		public static Point3 operator *(Point3 value1, Point3 value2)
		{
			return new Point3(value1.X * value2.X, value1.Y * value2.Y, value1.Z * value2.Z);
		}

		//
		// Summary:
		//     Divides the components of a Microsoft.Xna.Framework.Point by the components of
		//     another Microsoft.Xna.Framework.Point.
		//
		// Parameters:
		//   source:
		//     Source Microsoft.Xna.Framework.Point on the left of the div sign.
		//
		//   divisor:
		//     Divisor Microsoft.Xna.Framework.Point on the right of the div sign.
		//
		// Returns:
		//     The result of dividing the points.
		public static Point3 operator /(Point3 source, Point3 divisor)
		{
			return new Point3(source.X / divisor.X, source.Y / divisor.Y, source.Z / divisor.Z);
		}

		//
		// Summary:
		//     Compares whether two Microsoft.Xna.Framework.Point instances are equal.
		//
		// Parameters:
		//   a:
		//     Microsoft.Xna.Framework.Point instance on the left of the equal sign.
		//
		//   b:
		//     Microsoft.Xna.Framework.Point instance on the right of the equal sign.
		//
		// Returns:
		//     true if the instances are equal; false otherwise.
		public static bool operator ==(Point3 a, Point3 b)
		{
			return a.Equals(b);
		}

		//
		// Summary:
		//     Compares whether two Microsoft.Xna.Framework.Point instances are not equal.
		//
		// Parameters:
		//   a:
		//     Microsoft.Xna.Framework.Point instance on the left of the not equal sign.
		//
		//   b:
		//     Microsoft.Xna.Framework.Point instance on the right of the not equal sign.
		//
		// Returns:
		//     true if the instances are not equal; false otherwise.
		public static bool operator !=(Point3 a, Point3 b)
		{
			return !a.Equals(b);
		}

		//
		// Summary:
		//     Compares whether current instance is equal to specified System.Object.
		//
		// Parameters:
		//   obj:
		//     The System.Object to compare.
		//
		// Returns:
		//     true if the instances are equal; false otherwise.
		public override bool Equals(object obj)
		{
			if (obj is Point3)
			{
				return Equals((Point3)obj);
			}

			return false;
		}

		//
		// Summary:
		//     Compares whether current instance is equal to specified Microsoft.Xna.Framework.Point.
		//
		// Parameters:
		//   other:
		//     The Microsoft.Xna.Framework.Point to compare.
		//
		// Returns:
		//     true if the instances are equal; false otherwise.
		public bool Equals(Point3 other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		//
		// Summary:
		//     Gets the hash code of this Microsoft.Xna.Framework.Point.
		//
		// Returns:
		//     Hash code of this Microsoft.Xna.Framework.Point.
		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y, Z);
		}

		//
		// Summary:
		//     Returns a System.String representation of this Microsoft.Xna.Framework.Point
		//     in the format: {X:[Microsoft.Xna.Framework.Point.X] Y:[Microsoft.Xna.Framework.Point.Y]}
		//
		// Returns:
		//     System.String representation of this Microsoft.Xna.Framework.Point.
		public override string ToString()
		{
			return $"{{X:{X} Y:{Y} Z:{Z}}}";
		}

		//
		// Summary:
		//     Gets a Microsoft.Xna.Framework.Vector3 representation for this object.
		//
		// Returns:
		//     A Microsoft.Xna.Framework.Vector3 representation for this object.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 ToVector3()
		{
			return new Vector3(X, Y, Z);
		}

		//
		// Summary:
		//     Deconstruction method for Microsoft.Xna.Framework.Point.
		//
		// Parameters:
		//   x:
		//
		//   y:
		public void Deconstruct(out int x, out int y, out int z)
		{
			x = X;
			y = Y;
			z = Z;
		}
	}
}
