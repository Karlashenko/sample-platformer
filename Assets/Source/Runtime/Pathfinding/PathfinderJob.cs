using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Sample.Pathfinding
{
    [BurstCompile]
    public struct PathfinderJob : IJob
    {
        [ReadOnly] public Waypoint Origin;
        [ReadOnly] public Waypoint Destination;
        [ReadOnly] public NativeArray<Waypoint> Waypoints;
        [ReadOnly] public NativeParallelMultiHashMap<int, WaypointConnection> Connections;

        public NativeList<PathfinderNode> Result;

        public void Execute()
        {
            var visited = new NativeList<int>(16, Allocator.Temp);
            var pending = new NativeQueue<PathfinderNode>(Allocator.Temp);
            var nodes = new NativeParallelHashMap<int, PathfinderNode>(16, Allocator.Temp);

            pending.Enqueue(CreateNode(Origin, WaypointConnectionType.Walk, null));

            while (pending.Count > 0)
            {
                var current = pending.Dequeue();

                if (!nodes.ContainsKey(current.WaypointIndex))
                {
                    nodes.Add(current.WaypointIndex, current);
                }

                foreach (var connection in Connections.GetValuesForKey(current.WaypointIndex))
                {
                    if (visited.Contains(connection.TargetWaypointIndex))
                    {
                        continue;
                    }

                    visited.Add(connection.TargetWaypointIndex);

                    pending.Enqueue(CreateNode(Waypoints[connection.TargetWaypointIndex], connection.Type, current));
                }

                if (current.WaypointIndex == Destination.Index)
                {
                    BuildPath(Origin.Index, Destination.Index, nodes);
                    break;
                }
            }

            visited.Dispose();
            pending.Dispose();
            nodes.Dispose();
        }

        private void BuildPath(int originWaypointIndex, int targetWaypointIndex, NativeParallelHashMap<int, PathfinderNode> nodes)
        {
            if (nodes.Count() == 0 || !nodes.ContainsKey(targetWaypointIndex))
            {
                return;
            }

            var current = nodes[targetWaypointIndex];

            while (current.WaypointIndex != originWaypointIndex)
            {
                Result.Add(current);
                current = nodes[current.ParentWaypointIndex];
            }

            Result.Add(nodes[originWaypointIndex].WithConnectionType(Result[^1].WaypointConnectionType));

            Reverse(Result);
        }

        private static void Reverse(NativeList<PathfinderNode> list)
        {
            var a = 0;
            var b = list.Length - 1;

            while (a < b)
            {
                (list[a], list[b]) = (list[b], list[a]);

                a += 1;
                b -= 1;
            }
        }

        private static PathfinderNode CreateNode(in Waypoint waypoint, WaypointConnectionType connectionType, in PathfinderNode? parent)
        {
            return new PathfinderNode(waypoint.Position, waypoint.Index, parent?.WaypointIndex ?? -1, connectionType);
        }
    }
}
