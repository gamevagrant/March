using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PathFinding
{
    /// <summary>
    /// Node the basic unit used by a star.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Flag indicates if the node is avaliable or not.
        /// </summary>
        public bool IsAvailable;

        /// <summary>
        /// Node coordinates.
        /// </summary>
        public int X;
        public int Y;

        /// <summary>
        /// world position
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Dictionary of node connections according to 8 directions.
        /// </summary>
        /// <remarks>Direction please refers to PathFinder.Surrounding, which follows diamond pattern.</remarks>
        public Dictionary<Vector2, NodeConnection> ConnectionDict = new Dictionary<Vector2, NodeConnection>();

        public const int ConnectionCount = 2;

        private const int NodeClearCount = 3;
        private const string WallTag = "Wall";

        public Node(float x, float y, Vector2 position, Grid grid)
        {
            Initialize(x, y, position, grid);
        }

        public void Initialize(float x, float y, Vector2 position, Grid grid)
        {
            X = (int)x;
            Y = (int)y;

            Position = position;

            for (var i = -ConnectionCount / 2; i <= ConnectionCount / 2; ++i)
            {
                for (var j = -ConnectionCount / 2; j <= ConnectionCount / 2; ++j)
                {
                    if (i == 0 && j == 0)
                        continue;
                    var direction = Mathf.Abs(i) == Mathf.Abs(j) ? new Vector2(i, j) : new Vector2(i * 2, j * 2);
                    ConnectionDict.Add(direction, null);
                }
            }

            // Nodes reside along the bounds defined as invalid.
            IsAvailable = Position.x > grid.Range.x &&
                          Position.y < grid.Range.y &&
                          Position.x < grid.Range.z &&
                          Position.y > grid.Range.w;
            if (!IsAvailable)
            {
                DisableConnections();
            }
        }

        /// <summary>
        /// Check the validation of current node according to valid connection points.
        /// </summary>
        /// <param name="grid">The grid we are using.</param>
        /// <remarks>The current algorithm checked by valid connection counts, which is 3.</remarks>
        public void CheckNodeValidation(Grid grid)
        {
            if (IsAvailable)
            {
                var clearCount = ConnectionDict.Values.Count(connection => connection != null && connection.Valid);
                //If not at least 3 valid connection points - disable node
                if (clearCount < NodeClearCount)
                {
                    IsAvailable = false;
                    DisableConnections();
                }
            }
        }

        /// <summary>
        /// Remove connections that connect to bad nodes
        /// </summary>
        public void CheckConnectionsValidation()
        {
            ConnectionDict.Values.ToList().ForEach(connection =>
            {
                if (connection != null && connection.Node != null && !connection.Node.IsAvailable)
                {
                    connection.Valid = false;
                }
            });

        }

        /// <summary>
        /// Disable all connections going from this
        /// </summary>
        public void DisableConnections()
        {
            ConnectionDict.Values.ToList().ForEach(connection =>
            {
                if (connection != null)
                {
                    connection.Valid = false;
                }
            });
        }

        /// <summary>
        /// Debug draw for connection lines.
        /// </summary>
        /// <remarks>For debugging purpose only.</remarks>
        public void DrawConnections(Transform parent)
        {
            ConnectionDict.Values.ToList().ForEach(connection =>
            {
                if (connection != null)
                {
                    connection.DrawLine(parent);
                }
            });
        }

        /// <summary>
        /// Debug draw for node itself.
        /// </summary>
        public void DrawNode(Transform parent)
        {
            var root = parent.Find("Nodes") ?? new GameObject("Nodes").transform;
            root.parent = parent;
            var node = Object.Instantiate(Resources.Load("Node")) as GameObject;
            node.layer = parent.gameObject.layer;
            node.transform.parent = root;
            node.transform.position = Position;
            node.transform.GetComponent<SpriteRenderer>().color = IsAvailable ? Color.yellow : Color.red;
        }

        /// <summary>
        /// Raycast in all 8 directions to determine valid routes
        /// </summary>
        /// <param name="grid">The whole grid.</param>
        public void InitializeConnections(Grid grid)
        {
            bool valid;
            RaycastHit2D hit;
            var diagonalDistance = Mathf.Sqrt(Mathf.Pow(grid.UnitSize.x / 2, 2) + Mathf.Pow(grid.UnitSize.y / 2, 2));

            for (var i = -ConnectionCount / 2; i <= ConnectionCount / 2; ++i)
            {
                for (var j = -ConnectionCount / 2; j <= ConnectionCount / 2; ++j)
                {
                    if (i == 0 && j == 0)
                        continue;

                    var direction = Mathf.Abs(i) == Mathf.Abs(j) ? new Vector2(i, j) : new Vector2(i * 2, j * 2);
                    if (X + direction.x >= 0 && Y + direction.y >= 0 && X + direction.x < grid.Width && Y + direction.y < grid.Height)
                    {
                        hit = Physics2D.Raycast(Position, direction, grid.UnitSize.x);
                        valid = !(hit.collider != null && hit.collider.tag == WallTag);
                        ConnectionDict[direction] = new NodeConnection(this, grid.Nodes[X + (int)direction.x, Y + (int)direction.y], valid);
                    }
                }
            }
        }
    }
}
