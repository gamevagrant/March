using UnityEngine;

namespace PathFinding
{
    /// <summary>
    /// Connection between two nodes.
    /// </summary>
    public class NodeConnection
    {
        public Node Parent;
        public Node Node;

        /// <summary>
        /// Vector which can indicate 8 directions.
        /// </summary>
        /// <remarks>Like (-2, 0) means left, (-1, -1) means bottom left, refers to diamond pattern.</remarks>
        public Vector2 Direction;

        /// <summary>
        /// Flag indicates if the connection is valid.
        /// </summary>
        public bool Valid;

        public NodeConnection(Node parent, Node node, bool valid)
        {
            Valid = valid;
            Node = node;
            Parent = parent;

            Valid = Valid && (Node != null && Node.IsAvailable) && (Parent != null && Parent.IsAvailable);
        }

        /// <summary>
        /// Draw a line with current node connecton.
        /// </summary>
        /// <param name="parent">Parent tranform.</param>
        /// <remarks>Debug only.</remarks>
        public void DrawLine(Transform parent)
        {
            if (Parent == null || Node == null)
                return;

            var root = parent.Find("Lines") ?? new GameObject("Lines").transform;
            root.transform.parent = parent;
            var go = Object.Instantiate(Resources.Load("Line")) as GameObject;
            go.layer = parent.gameObject.layer;
            go.transform.parent = root.transform;
            var line = go.GetComponent<LineRenderer>();
            line.SetPosition(0, Parent.Position);
            line.SetPosition(1, Node.Position);
            line.startColor = line.endColor = (Valid) ? Color.green : Color.red;
            line.startWidth = 0.06f;
            line.endWidth = 0f;
        }
    }
}