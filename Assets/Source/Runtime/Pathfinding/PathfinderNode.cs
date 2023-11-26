using System;
using UnityEngine;

namespace Sample.Pathfinding
{
    public readonly struct PathfinderNode : IEquatable<PathfinderNode>
    {
        public readonly Vector2 Position;
        public readonly int WaypointIndex;
        public readonly int ParentWaypointIndex;
        public readonly WaypointConnectionType WaypointConnectionType;

        public PathfinderNode(Vector2 position, int waypointIndex, int parentWaypointIndex, WaypointConnectionType waypointConnectionType)
        {
            Position = position;
            WaypointIndex = waypointIndex;
            ParentWaypointIndex = parentWaypointIndex;
            WaypointConnectionType = waypointConnectionType;
        }

        public PathfinderNode WithConnectionType(WaypointConnectionType waypointConnectionType)
        {
            return new PathfinderNode(Position, WaypointIndex, ParentWaypointIndex, waypointConnectionType);
        }

        public static bool operator ==(PathfinderNode a, PathfinderNode b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PathfinderNode a, PathfinderNode b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object other)
        {
            return other is PathfinderNode node && node.Equals(this);
        }

        public bool Equals(PathfinderNode other)
        {
            return other.ParentWaypointIndex == ParentWaypointIndex;
        }

        public override int GetHashCode()
        {
            return ParentWaypointIndex;
        }
    }
}
