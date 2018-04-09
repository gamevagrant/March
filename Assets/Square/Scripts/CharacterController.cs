using System.Collections.Generic;
using UnityEngine;

namespace March.Scene
{
    public class CharacterController : MonoBehaviour
    {
        public int DebugPositionCount = 1000;
        [Range(0, 1)]
        public float Speed = 1f;

        public bool ShowDebugPath;

        public bool MoveForward
        {
            set
            {
                render.flipX = value;
            }
        }

        private SpriteRenderer render;

        private void Awake()
        {
            render = transform.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Draw path according to list of nodes.
        /// </summary>
        /// <param name="nodeList">The node list.</param>
        /// <remarks>For debug purpose only.</remarks>
        public void DrawPath(List<Vector3> nodeList)
        {
            if (!ShowDebugPath)
                return;

            var lineRenderer = transform.GetComponent<LineRenderer>();
            lineRenderer.positionCount = DebugPositionCount;
            lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
            lineRenderer.startColor = lineRenderer.endColor = Color.yellow;
            lineRenderer.sortingOrder = transform.GetComponent<Renderer>().sortingOrder;

            var count = 0;
            foreach (var node in nodeList)
            {
                lineRenderer.SetPosition(count++, node);
            }

            lineRenderer.positionCount = nodeList.Count;
        }
    }
}