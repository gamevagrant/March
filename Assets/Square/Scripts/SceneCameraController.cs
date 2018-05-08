using DG.Tweening;
using System;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

namespace March.Scene
{
    public class SceneCameraController : MonoBehaviour
    {
        public float PanSpeed = 200f;
        public float ZoomSpeed = 10f;
        [Range(0f, 10f)]
        public float FlipFactor = 1f;
        [Range(0f, 1f)]
        public float FlipDuration = 0.5f;
        public Ease FlipEase = Ease.Linear;
        [Range(0f, 1f)]
        public float BackDuration = 0.2f;
        public Ease BackEase = Ease.Linear;
        public Vector2 ScaleRange = new Vector2(0.8f, 1.2f);
        public Vector2 MoveRange = new Vector2(-10f, 10f);
        public Vector2 OutMoveRange = new Vector2(-12f, 12f);

        public SceneSerializer Serilizer;
        public PathFinding.Grid GridManager;

        private ScreenTransformGesture HorizontalMoveGesture;
        private TransformGesture ScaleGesture;
        private PressGesture PressGesture;
        private ReleaseGesture ReleaseGesture;
        private FlickGesture FlickGesture;

        private CharacterController player;
        private Tween playerTweener;
        private Tween flipTweener;
        private Sequence sequence;

        private void Awake()
        {
            ScaleRange *= Camera.main.orthographicSize;

            HorizontalMoveGesture = GetComponent<ScreenTransformGesture>();
            ScaleGesture = GetComponent<TransformGesture>();
            PressGesture = GetComponent<PressGesture>();
            ReleaseGesture = GetComponent<ReleaseGesture>();
            FlickGesture = GetComponent<FlickGesture>();
        }

        private void Start()
        {
            Camera.main.transform.position = new Vector3(MoveRange.x, Camera.main.transform.position.y, Camera.main.transform.position.z);

            Serilizer.SceneIsReady += OnSceneIsReady;
        }

        private void OnSceneIsReady(object sender, EventArgs args)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
            {
                player = go.GetComponent<CharacterController>();
            }

            if (GridManager != null)
            {
                GridManager.Generate();
            }
        }

        private void OnEnable()
        {
            HorizontalMoveGesture.Transformed += HorizontalTransformHandler;
            ScaleGesture.Transformed += ScaleTransformedHandler;
            PressGesture.Pressed += OnPressHandler;
            ReleaseGesture.Released += OnReleaseHandler;
            FlickGesture.Flicked += OnFlickHandler;
        }

        private void OnDisable()
        {
            HorizontalMoveGesture.Transformed -= HorizontalTransformHandler;
            ScaleGesture.Transformed -= ScaleTransformedHandler;
            PressGesture.Pressed -= OnPressHandler;
            ReleaseGesture.Released -= OnReleaseHandler;
            FlickGesture.Flicked -= OnFlickHandler;
        }

        private void ScaleTransformedHandler(object sender, EventArgs e)
        {
            Camera.main.orthographicSize *= ScaleGesture.DeltaScale;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, ScaleRange.x, ScaleRange.y);
        }

        private void HorizontalTransformHandler(object sender, EventArgs e)
        {
            Camera.main.transform.localPosition -=
                new Vector3(HorizontalMoveGesture.DeltaPosition.x, 0, HorizontalMoveGesture.DeltaPosition.z) * PanSpeed;
            Camera.main.transform.localPosition = new Vector3(Mathf.Clamp(Camera.main.transform.localPosition.x, OutMoveRange.x, OutMoveRange.y),
                Camera.main.transform.localPosition.y,
                Camera.main.transform.localPosition.z);
        }

        private void OnReleaseHandler(object sender, EventArgs e)
        {
            var step = Mathf.Abs(Camera.main.transform.position.x) - Mathf.Abs(MoveRange.x);
            if (step > 0)
            {
                var direction = (Camera.main.transform.position.x > 0) ? -1 : 1;
                step = direction * step;
                Camera.main.transform.DOLocalMoveX(Camera.main.transform.localPosition.x + step, BackDuration).SetEase(BackEase);
            }

            MovePlayer();
        }

        private void OnFlickHandler(object sender, EventArgs e)
        {
            var direction = (FlickGesture.NormalizedScreenPosition.x - FlickGesture.PreviousNormalizedScreenPosition.x) > 0 ? -1 : 1;
            var step = Camera.main.orthographicSize * FlipFactor * direction;

            if (flipTweener != null)
                flipTweener.Kill();

            var x = Mathf.Clamp(Camera.main.transform.position.x + step, MoveRange.x, MoveRange.y);
            flipTweener = Camera.main.transform.DOLocalMoveX(x, FlipDuration).SetEase(FlipEase);
        }

        private void OnPressHandler(object sender, EventArgs e)
        {
            //MovePlayer();
        }

        private void MovePlayer()
        {
            if (player == null)
                return;

            //Convert mouse click point to grid coordinates
            Vector2 endWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var nodeList = GridManager.GetNodeList(player.transform.position, endWorldPos);

            if (nodeList.Count > 0)
            {
                if (playerTweener != null)
                {
                    playerTweener.Kill();
                }
                var duration = nodeList.Count * player.Speed;
                playerTweener = player.transform.DOPath(nodeList.ToArray(), duration).OnWaypointChange(wayIndex =>
                {
                    if (wayIndex < nodeList.Count - 1)
                    {
                        player.MoveForward = nodeList[wayIndex + 1].x - nodeList[wayIndex].x >= 0;
                    }
                });
            }

            player.DrawPath(nodeList);
        }
    }
}