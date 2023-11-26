using System;
using Sample.Extensions;
using Unity.Collections;
using UnityEngine;

namespace Sample.Pathfinding
{
    [ExecuteInEditMode]
    public class Waypoints : MonoBehaviour
    {
        [SerializeField] private Vector2 _origin;
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _cellSize;
        [SerializeField] private int _jumpRange;
        [SerializeField] private LayerMask _collisionMask;

        private Vector2 _size;
        private NativeArray<Waypoint> _waypoints;
        private NativeParallelMultiHashMap<int, WaypointConnection> _connections;

        private void Awake()
        {
            Generate();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public int GetCellSize()
        {
            return _cellSize;
        }

        public void Generate(Vector2 origin, int width, int height)
        {
            _origin = origin;
            _width = width;
            _height = height;

            Generate();
        }

        public void Generate()
        {
            Dispose();
            CreateWaypoints();
            CreateWaypointConnections();
        }

        public Waypoint GetWaypointAt(int index)
        {
            if (_waypoints.IndexInBounds(index))
            {
                return _waypoints[index];
            }

            Debug.LogError($"No waypoint at index {index.ToString()}");
            return Waypoint.Empty;
        }

        public NativeArray<Waypoint> GetWaypoints()
        {
            return _waypoints;
        }

        public NativeParallelMultiHashMap<int, WaypointConnection> GetConnections()
        {
            return _connections;
        }

        public Waypoint FindClosestWalkableToWorldPoint(Vector2 point, int range = 5)
        {
            var relative = point - _origin;

            var x = Math.Min((int) (relative.x / _cellSize), _width - 1);
            var y = Math.Min((int) (relative.y / _cellSize), _height - 1);

            var distanceMin = Mathf.Infinity;
            var closestIndex = -1;

            for (var yOffset = -range; yOffset < range; yOffset++)
            {
                for (var xOffset = -range; xOffset < range; xOffset++)
                {
                    var index = IndexAt(x + xOffset, y + yOffset);

                    if (!ValidateWaypointType(index, WaypointType.Walkable))
                    {
                        continue;
                    }

                    var distance = (point - _waypoints[index].Position).sqrMagnitude;

                    if (distance > distanceMin)
                    {
                        continue;
                    }

                    distanceMin = distance;
                    closestIndex = index;
                }
            }

            return closestIndex == -1 ? Waypoint.Empty : _waypoints[closestIndex];
        }

        private void Dispose()
        {
            if (_waypoints.IsCreated)
            {
                _waypoints.Dispose();
            }

            if (_connections.IsCreated)
            {
                _connections.Dispose();
            }
        }

        private void CreateWaypoints()
        {
            _size = new Vector2(_width * _cellSize, _height * _cellSize);
            _waypoints = new NativeArray<Waypoint>(_width * _height, Allocator.Persistent);

            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    var index = x + y * _width;
                    var type = WaypointType.Empty;
                    var position = new Vector2(x * _cellSize + _cellSize / 2f, y * _cellSize + _cellSize / 2f) + _origin;
                    var isEdge = false;
                    var isSlope = false;

                    if (Physics2D.Raycast(position, Vector2.down, 0, _collisionMask))
                    {
                        type = WaypointType.Solid;
                    }
                    else
                    {
                        var raycastHit2D = Physics2D.Raycast(position, Vector2.down, _cellSize, _collisionMask);

                        if (raycastHit2D)
                        {
                            position = raycastHit2D.point;
                            type = WaypointType.Walkable;
                            isSlope = Mathf.Abs(raycastHit2D.normal.x) > 0.01f;
                        }
                    }

                    if (x > 0)
                    {
                        var left = x - 1 + y * _width;

                        isEdge = type == WaypointType.Walkable && _waypoints[left].Type == WaypointType.Empty;

                        if (!_waypoints[left].IsEdge && _waypoints[left].Type == WaypointType.Walkable && type == WaypointType.Empty)
                        {
                            _waypoints[left] = _waypoints[left].WithIsEdge(true);
                        }
                    }

                    _waypoints[index] = new Waypoint(index, position, type, isEdge, isSlope);
                }
            }
        }

        private void CreateWaypointConnections()
        {
            _connections = new NativeParallelMultiHashMap<int, WaypointConnection>(4, Allocator.Persistent);

            foreach (var waypoint in _waypoints)
            {
                if (waypoint.Type != WaypointType.Walkable)
                {
                    continue;
                }

                CreateWalkConnections(waypoint);
                CreateFallConnections(waypoint);
                CreateJumpConnections(waypoint);
            }
        }

        private void CreateJumpConnections(Waypoint waypoint)
        {
            if (FirstAbove(WaypointType.Solid, IndexX(waypoint.Index), IndexY(waypoint.Index), _jumpRange) > 0)
            {
                return;
            }

            for (var yOffset = -_jumpRange; yOffset <= _jumpRange; yOffset++)
            {
                for (var xOffset = -_jumpRange; xOffset <= _jumpRange; xOffset++)
                {
                    var x = IndexX(waypoint.Index) + xOffset;
                    var y = IndexY(waypoint.Index) + yOffset;
                    var i = IndexAt(x, y);

                    if (!ValidateWaypointType(i, WaypointType.Walkable) || _waypoints[i].IsSlope)
                    {
                        continue;
                    }

                    if (Math.Abs(waypoint.Position.x - _waypoints[i].Position.x) < _cellSize || waypoint.Position.y - _waypoints[i].Position.y > _cellSize * 2)
                    {
                        continue;
                    }

                    if (Mathf.Abs(IndexX(waypoint.Index) - x) < 2)
                    {
                        continue;
                    }

                    if (IndexX(waypoint.Index) > x && ValidateWaypointType(IndexAt(x + 1, y - 1), WaypointType.Solid) ||
                        IndexX(waypoint.Index) < x && ValidateWaypointType(IndexAt(x - 1, y - 1), WaypointType.Solid))
                    {
                        continue;
                    }

                    var sqrMagnitude = (_waypoints[i].Position - waypoint.Position).sqrMagnitude;

                    if (sqrMagnitude > _jumpRange * _jumpRange)
                    {
                        continue;
                    }

                    AddConnection(waypoint.Index, new WaypointConnection(i, WaypointConnectionType.Jump));
                    AddConnection(i, new WaypointConnection(waypoint.Index, WaypointConnectionType.Jump));
                }
            }
        }

        private void CreateFallConnections(Waypoint waypoint)
        {
            if (!_waypoints[waypoint.Index].IsEdge)
            {
                return;
            }

            var x = IndexX(waypoint.Index);
            var y = IndexY(waypoint.Index);

            var rightNeighbourIndex = IndexAt(x + 1, y);
            var leftNeighbourIndex = IndexAt(x - 1, y);

            if (ValidateWaypointType(leftNeighbourIndex, WaypointType.Empty))
            {
                var index = FirstBelow(WaypointType.Walkable, x - 1, y, 0);

                if (index >= 0 && !(waypoint.IsSlope && _waypoints[index].IsSlope))
                {
                    AddConnection(waypoint.Index, new WaypointConnection(index, WaypointConnectionType.Fall));
                }
            }

            if (ValidateWaypointType(rightNeighbourIndex, WaypointType.Empty))
            {
                var index = FirstBelow(WaypointType.Walkable, x + 1, y, 0);

                if (index >= 0 && !(waypoint.IsSlope && _waypoints[index].IsSlope))
                {
                    AddConnection(waypoint.Index, new WaypointConnection(index, WaypointConnectionType.Fall));
                }
            }
        }

        private void CreateWalkConnections(Waypoint waypoint)
        {
            var x = IndexX(waypoint.Index);
            var y = IndexY(waypoint.Index);

            var rightNeighbourIndex = IndexAt(x + 1, y);

            if (ValidateWaypointType(rightNeighbourIndex, WaypointType.Walkable))
            {
                AddConnection(waypoint.Index, new WaypointConnection(rightNeighbourIndex, WaypointConnectionType.Walk));
                AddConnection(rightNeighbourIndex, new WaypointConnection(waypoint.Index, WaypointConnectionType.Walk));
                return;
            }

            var topRightNeighbour = IndexAt(x + 1, y + 1);

            if (ValidateWaypointType(topRightNeighbour, WaypointType.Walkable))
            {
                if (Mathf.Abs(_waypoints[topRightNeighbour].Position.y - waypoint.Position.y) <= _cellSize / 2f || waypoint.IsSlope && _waypoints[topRightNeighbour].IsSlope)
                {
                    AddConnection(waypoint.Index, new WaypointConnection(topRightNeighbour, WaypointConnectionType.Walk));
                    AddConnection(topRightNeighbour, new WaypointConnection(waypoint.Index, WaypointConnectionType.Walk));
                }
            }

            var bottomRightNeighbour = IndexAt(x + 1, y - 1);

            if (ValidateWaypointType(bottomRightNeighbour, WaypointType.Walkable))
            {
                if (Mathf.Abs(_waypoints[bottomRightNeighbour].Position.y - waypoint.Position.y) <= _cellSize / 2f || waypoint.IsSlope && _waypoints[bottomRightNeighbour].IsSlope)
                {
                    AddConnection(waypoint.Index, new WaypointConnection(bottomRightNeighbour, WaypointConnectionType.Walk));
                    AddConnection(bottomRightNeighbour, new WaypointConnection(waypoint.Index, WaypointConnectionType.Walk));
                }
            }
        }

        private void AddConnection(int index, WaypointConnection connection)
        {
            _connections.Add(index, connection);
        }

        private bool ValidateWaypointType(int index, WaypointType type)
        {
            if (!_waypoints.IndexInBounds(index))
            {
                return false;
            }

            return _waypoints[index].Type == type;
        }

        private int IndexAt(int x, int y)
        {
            return x + y * _width;
        }

        private int IndexX(int index)
        {
            return index % _width;
        }

        private int IndexY(int index)
        {
            return index / _width;
        }

        private int FirstBelow(WaypointType type, int cellX, int cellY, int minY)
        {
            for (var y = cellY; y > minY; y--)
            {
                var index = cellX + y * _width;

                if (!_waypoints.IndexInBounds(index))
                {
                    break;
                }

                if (_waypoints[index].Type == type)
                {
                    return index;
                }
            }

            return -1;
        }

        private int FirstAbove(WaypointType type, int cellX, int cellY, int maxY)
        {
            for (var yOffset = 0; yOffset < maxY; yOffset++)
            {
                var index = cellX + (cellY + yOffset) * _width;

                if (!_waypoints.IndexInBounds(index))
                {
                    break;
                }

                if (_waypoints[index].Type == type)
                {
                    return index;
                }
            }

            return -1;
        }

        private void OnDrawGizmosSelected()
        {
            if (!_waypoints.IsCreated || !_connections.IsCreated)
            {
                return;
            }

            foreach (var waypoint in _waypoints)
            {
                if (waypoint.Type != WaypointType.Walkable)
                {
                    continue;
                }

                Gizmos.DrawIcon(waypoint.Position, "winbtn_mac_max@2x");

                foreach (var connection in _connections.GetValuesForKey(waypoint.Index))
                {
                    Gizmos.color = connection.Type switch
                    {
                        WaypointConnectionType.Fall => Color.yellow,
                        WaypointConnectionType.Walk => Color.green,
                        WaypointConnectionType.Jump => Color.blue,
                        _ => Gizmos.color
                    };

                    Gizmos.DrawLine(waypoint.Position, _waypoints[connection.TargetWaypointIndex].Position);
                }
            }

            var topLeft = new Vector3(_origin.x, _size.y + _origin.y);
            var topRight = new Vector3(_size.x + _origin.x, _size.y + _origin.y);
            var bottomLeft = new Vector3(_origin.x, _origin.y);
            var bottomRight = new Vector3(_size.x + _origin.x, _origin.y);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);

            Gizmos.color = Color.cyan.With(a: 0.1f);

            for (var i = 1; i < _width; i++)
            {
                Gizmos.DrawLine(topLeft + new Vector3(i * _cellSize, 0), bottomLeft + new Vector3(i * _cellSize, 0));
            }

            for (var i = 1; i < _height; i++)
            {
                Gizmos.DrawLine(bottomLeft + new Vector3(0, i * _cellSize), bottomRight + new Vector3(0, i * _cellSize));
            }
        }
    }
}
