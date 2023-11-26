using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Sample.Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        [SerializeField] private Waypoints _waypoints = null!;

        public async UniTask<NativeList<PathfinderNode>?> FindPath(Vector2 origin, Vector2 target, CancellationToken cancellationToken)
        {
            var originWaypoint = _waypoints.FindClosestWalkableToWorldPoint(origin);
            var targetWaypoint = _waypoints.FindClosestWalkableToWorldPoint(target);

            if (originWaypoint.IsEmpty() || targetWaypoint.IsEmpty() || originWaypoint == targetWaypoint)
            {
                return null;
            }

            var job = new PathfinderJob();
            job.Origin = originWaypoint;
            job.Destination = targetWaypoint;
            job.Waypoints = _waypoints.GetWaypoints();
            job.Connections = _waypoints.GetConnections();
            job.Result = new NativeList<PathfinderNode>(16, Allocator.Persistent);
            await job.Schedule().WaitAsync(PlayerLoopTiming.PostLateUpdate, cancellationToken);

            if (job.Result.Length > 1)
            {
                return job.Result;
            }

            job.Result.Dispose();
            return null;

        }
    }
}
