using System;
using UnityEngine;

namespace Sample.Pathfinding
{
    public readonly struct Waypoint : IEquatable<Waypoint>
    {
        public static readonly Waypoint Empty = new(-1, default(Vector2), WaypointType.Empty);

        public readonly int Index;
        public readonly Vector2 Position;
        public readonly bool IsEdge;
        public readonly bool IsSlope;
        public readonly WaypointType Type;

        public Waypoint(int index, Vector2 position, WaypointType type, bool isEdge = false, bool isSlope = false)
        {
            Index = index;
            Position = position;
            Type = type;
            IsEdge = isEdge;
            IsSlope = isSlope;
        }

        public bool IsEmpty()
        {
            return Index == Empty.Index;
        }

        public Waypoint WithIsEdge(bool isEdge)
        {
            return new Waypoint(Index, Position, Type, isEdge, IsSlope);
        }

        public Waypoint WithIsSlope(bool isSlope)
        {
            return new Waypoint(Index, Position, Type, IsEdge, isSlope);
        }

        public static bool operator ==(Waypoint a, Waypoint b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Waypoint a, Waypoint b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object other)
        {
            return other is Waypoint waypoint && waypoint.Equals(this);
        }

        public bool Equals(Waypoint other)
        {
            return other.Index == Index;
        }

        public override int GetHashCode()
        {
            return Index;
        }
    }
}