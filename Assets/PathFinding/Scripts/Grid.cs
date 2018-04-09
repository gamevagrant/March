using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    /// <summary>
    /// The area that operate all nodes and node connections.
    /// </summary>
    /// <remarks>
    /// All grids are in the diamond pattern, open debug to see what exactly it is.
    /// Width and height will be set refers to Grid's local scale.x, scale.y, keep in mind.
    /// </remarks>
    public class Grid : MonoBehaviour
    {
        public int Width;
        public int Height;

        [Range(0, 2)]
        public float WidthScale = 1f;
        [Range(0, 2)]
        public float HeightScale = 1f;

        public bool AutoGenerate;

        public bool ShowDebug;
        public bool ShowDebugNode;
        public bool ShowDebugNodeConnection;

        public Node[,] Nodes;

        /// <summary>
        /// An vector to indicate width and height of a unit size.
        /// </summary>
        public Vector2 UnitSize;

        /// <summary>
        /// Range of the grid.
        /// </summary>
        public Vector4 Range;

        /// <summary>
        /// Zero transform of start node.
        /// </summary>
        public Transform Zero;

        public int Left
        {
            get { return 0; }
        }

        public int Right
        {
            get { return Width; }
        }

        public int Bottom
        {
            get { return 0; }
        }

        public int Top
        {
            get { return Height; }
        }

        private void Awake()
        {
            if (AutoGenerate)
                Generate();
        }

        private void DrawAll()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Nodes[x, y] == null)
                        continue;

                    if (ShowDebugNode)
                        Nodes[x, y].DrawNode(transform);

                    if (ShowDebugNodeConnection)
                        Nodes[x, y].DrawConnections(transform);
                }
            }

            DrawRange(transform);
        }

        private void DrawRange(Transform parent)
        {
            var root = parent.Find("Bounds") ?? new GameObject("Bounds").transform;
            root.parent = parent;

            const int count = 4;
            for (var i = 0; i < count; ++i)
            {
                var go = Instantiate(Resources.Load("Line")) as GameObject;
                go.layer = parent.gameObject.layer;
                go.transform.parent = root;
                var lineRenderer = go.GetComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
                lineRenderer.startColor = lineRenderer.endColor = Color.red;
                lineRenderer.sortingOrder = gameObject.GetComponent<Renderer>().sortingOrder;

                Vector3 begin = Vector3.zero;
                Vector3 end = Vector3.zero;
                if (i == 0)
                {
                    begin = new Vector3(Range.x, Range.y, 0);
                    end = new Vector3(Range.x, Range.w, 0);
                }
                else if (i == 1)
                {
                    begin = new Vector3(Range.x, Range.y, 0);
                    end = new Vector3(Range.z, Range.y, 0);
                }
                else if (i == 2)
                {
                    begin = new Vector3(Range.z, Range.y, 0);
                    end = new Vector3(Range.z, Range.w, 0);
                }
                else if (i == 3)
                {
                    begin = new Vector3(Range.x, Range.w, 0);
                    end = new Vector3(Range.z, Range.w, 0);
                }
                lineRenderer.SetPosition(0, begin);
                lineRenderer.SetPosition(1, end);
            }
        }

        public Point WorldToGrid(Vector2 worldPosition)
        {
            worldPosition = worldPosition - (Vector2)Zero.position;
            var gridPosition = new Vector2((worldPosition.x / WidthScale * 2f), -(worldPosition.y / HeightScale * 2f) + 1);

            //adjust to our nearest integer
            var rx = gridPosition.x % 1;
            if (rx < 0.5f)
                gridPosition.x = gridPosition.x - rx;
            else
                gridPosition.x = gridPosition.x + (1 - rx);

            var ry = gridPosition.y % 1;
            if (ry < 0.5f)
                gridPosition.y = gridPosition.y - ry;
            else
                gridPosition.y = gridPosition.y + (1 - ry);

            var x = (int)gridPosition.x;
            var y = (int)gridPosition.y;

            if (x < 0 || y < 0 || x > Width - 1 || y > Height - 1)
                return null;

            var node = Nodes[x, y];
            if (node != null)
                return new Point(node.X, node.Y);

            //We calculated a spot between nodes', to find nearest neighbor
            var mag = float.MaxValue;
            if (x < Width - 1 && Nodes[x + 1, y].IsAvailable)
            {
                var mag1 = (Nodes[x + 1, y].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x + 1, y];
                }
            }

            if (y < Height - 1 && Nodes[x, y + 1].IsAvailable)
            {
                var mag1 = (Nodes[x, y + 1].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x, y + 1];
                }
            }

            if (x > 0 && Nodes[x - 1, y].IsAvailable)
            {
                var mag1 = (Nodes[x - 1, y].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x - 1, y];
                }
            }

            if (y > 0 && Nodes[x, y - 1].IsAvailable)
            {
                var mag1 = (Nodes[x, y - 1].Position - worldPosition).magnitude;
                if (mag1 < mag)
                {
                    mag = mag1;
                    node = Nodes[x, y - 1];
                }
            }

            var point = (node == null) ? null : new Point(node.X, node.Y);
            return point;
        }

        public Vector2 GridToWorld(Point gridPosition)
        {
            var world = new Vector2(gridPosition.X * WidthScale / 2f, -(gridPosition.Y - 1) * HeightScale / 2f) + new Vector2(Zero.position.x, Zero.position.y);
            return world;
        }

        public bool ConnectionIsValid(Point point1, Point point2)
        {
            // same point case.
            if (point1.X == point2.X && point1.Y == point2.Y)
                return false;

            if (Nodes[point1.X, point1.Y] == null)
                return false;

            var direction = new Vector2(point2.X - point1.X, point2.Y - point1.Y);
            var connection = Nodes[point1.X, point1.Y].ConnectionDict[direction];
            return connection != null && connection.Valid;
        }

        /// <summary>
        /// Get node list in world position.
        /// </summary>
        /// <param name="node">Node to tranverse through.</param>
        /// <returns>The list of nodes in world position.</returns>
        private List<Vector3> GetNodeList(BreadCrumb node)
        {
            var nodeList = new List<Vector3>();
            while (node != null)
            {
                nodeList.Add(GridToWorld(node.Position));
                node = node.Next;
            }
            return nodeList;
        }

        private bool IsValidPoint(Point p)
        {
            return !(p.X < 0 || p.Y < 0 || p.X > Width - 1 || p.Y > Height - 1);
        }

        public List<Vector3> GetNodeList(Vector3 startPos, Vector3 endPos)
        {
            var nodeList = new List<Vector3>();

            var startGridPos = WorldToGrid(startPos);
            var endGridPos = WorldToGrid(endPos);
            if (startGridPos == null || endGridPos == null || !IsValidPoint(startGridPos) || !IsValidPoint(endGridPos))
                return nodeList;

            var bc = PathFinder.FindPath(this, startGridPos, endGridPos);
            return GetNodeList(bc);
        }

        /// <summary>
        /// Generate nodes according to grid settings.
        /// </summary>
        [ContextMenu("Generate Nodes")]
        public void Generate()
        {
            Width = ((int)transform.localScale.x) * 2 + 2;
            Height = ((int)transform.localScale.y) * 2 + 2;

            UnitSize = new Vector2(WidthScale, HeightScale);
            var topLeft = new Vector2(Zero.position.x, Zero.position.y);
            var bottomRight = new Vector2(transform.position.x + transform.localScale.x * WidthScale,
                transform.position.y - transform.localScale.y * HeightScale) + new Vector2(Zero.position.x, Zero.position.y) - new Vector2(transform.position.x, transform.position.y);
            Range = new Vector4(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);

            Nodes = new Node[Width, Height];

            //Initialize the grid nodes - 1 grid unit between each node
            for (var x = 0; x < Width / 2; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    float ptx = x;
                    var pty = -(y / 2) + 0.5f;
                    var offsetx = 0;

                    if (y % 2 == 0)
                    {
                        ptx = x + 0.5f;
                        offsetx = 1;
                    }
                    else
                    {
                        pty = -(y / 2);
                    }

                    var pos = new Vector2(ptx * WidthScale, pty * HeightScale) + (Vector2)Zero.transform.position;
                    var node = new Node(x * 2 + offsetx, y, pos, this);
                    Nodes[x * 2 + offsetx, y] = node;
                }
            }

            //Create connections between each node
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Nodes[x, y] == null) continue;
                    Nodes[x, y].InitializeConnections(this);
                }
            }

            //Pass 1, we removed the bad nodes, based on valid connections
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Nodes[x, y] == null)
                        continue;

                    Nodes[x, y].CheckNodeValidation(this);
                }
            }

            //Pass 2, remove bad connections based on bad nodes
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Nodes[x, y] == null)
                        continue;

                    Nodes[x, y].CheckConnectionsValidation();
                }
            }

            if (ShowDebug)
                DrawAll();
        }

        /// <summary>
        /// Clear up.
        /// </summary>
        /// <remarks>Edit debug use only, cause GC if in running mode.</remarks>
        [ContextMenu("Cleanup Nodes")]
        public void Cleanup()
        {
            var count = transform.childCount;
            for (var i = count - 1; i >= 0; --i)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            Nodes = null;
        }
    }
}