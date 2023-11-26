using System;

namespace Sample.Pathfinding
{
    [Serializable]
    public readonly struct WaypointConnection : IEquatable<WaypointConnection>, IComparable<WaypointConnection>
    {
        public static WaypointConnection Empty = new(-1, WaypointConnectionType.Walk);

        public readonly int TargetWaypointIndex;
        public readonly WaypointConnectionType Type;

        public WaypointConnection(int targetWaypointIndex, WaypointConnectionType type)
        {
            TargetWaypointIndex = targetWaypointIndex;
            Type = type;
        }

        public bool IsEmpty()
        {
            return TargetWaypointIndex == Empty.TargetWaypointIndex;
        }

        public bool Equals(WaypointConnection other)
        {
            return TargetWaypointIndex == other.TargetWaypointIndex;
        }

        public int CompareTo(WaypointConnection other)
        {
            return ((int) Type).CompareTo((int) other.Type);
        }
    }
}