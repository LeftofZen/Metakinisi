using Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Metakinisi
{
	public class Vehicle
	{
		public Vector2 Position
		{
			get => TrackWorld[Cell.Y, Cell.X].PositionFromLerpedPercent(PercentThroughTile);
		}

		// define world location
		Point Cell;
		public float PercentThroughTile = 0f;

		// define movement
		float speed = 1.13f;
		public bool reversed = false;

		//public float direction = RotationHelpers.RotationAngles[Rotation.Zero]; // in radians
		//public Rotation Direction = Rotation.Zero;

		// new graph code
		Node Destination;
		Edge Location;
		float edgePosition;
		//Vector2 position;

		public Vehicle(Point cell, float percent)
		{
			PlaceInCell(cell, percent);
		}

		public void PlaceInCell(Point cell, float percent)
		{
			Cell = cell;
			PercentThroughTile = percent;
		}

		public void PlaceOnEdge(Edge e, float percent)
		{

		}

		public void Reverse()
		{
			reversed = !reversed;
		}

		public void Update(GameTime gameTime, TrackElement[,] trackWorld)
		{
			TrackWorld = trackWorld;
			var track = GetCurrentTrackElement(Cell, trackWorld);

			if (track.type == TrackType.None)
			{ return; }

			var remainingSpeedToUse = speed;  //* (reversed ? -1 : 1);

			do
			{
				var percentDirectionLeftOnTrack = reversed ? PercentThroughTile : 1 - PercentThroughTile;
				var distanceLeftOnTrack = track.LengthInWorld * percentDirectionLeftOnTrack;

				if (remainingSpeedToUse < distanceLeftOnTrack)
				{
					PercentThroughTile += remainingSpeedToUse / track.LengthInWorld * (reversed ? -1 : 1);
					remainingSpeedToUse = 0;
				}
				else
				{
					remainingSpeedToUse -= percentDirectionLeftOnTrack * track.LengthInWorld * (reversed ? -1 : 1);

					if (track.Paths.Count == 0)
					{
						PercentThroughTile = (reversed ? 0f : 1f);
						break;
					}

					var nextCoords = track.GetConnectedTrackElement(track.Paths.First().Value.First(), reversed);
					track = GetCurrentTrackElement(nextCoords, trackWorld);

					if (track.type != TrackType.None)
					{
						Cell = nextCoords;
						PercentThroughTile = (reversed ? 1f : 0f);
					}
				}
			}
			while (remainingSpeedToUse > 0);
		}

		TrackElement[,] TrackWorld;

		static TrackElement GetCurrentTrackElement(Point coords, TrackElement[,] trackWorld)
			=> coords.Y >= 0 && coords.X >= 0 && coords.Y < trackWorld.GetLength(0) && coords.X < trackWorld.GetLength(1)
		? trackWorld[coords.Y, coords.X]
		: TrackElement.None;

		static Point GetCurrentCoordinates(Vector2 position)
			=> new((int)position.X / 32, (int)position.Y / 32);

		public void Draw(SpriteBatch sb)
		{
			sb.DrawCircle(Position, 16, 16, Color.Red, 8);
			//TrackElement.DrawArrow(sb, Position, 32, Direction);
		}
	}
}
