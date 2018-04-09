/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using UnityEngine;
using TouchScript.Gestures.TransformGestures;

namespace TouchScript.Examples.CameraControl
{
    /// <exclude />
    public class CameraController : MonoBehaviour
    {
        public ScreenTransformGesture TwoFingerMoveGesture;
        public ScreenTransformGesture ManipulationGesture;
        public float PanSpeed = 200f;
        public float RotationSpeed = 200f;
        public float ZoomSpeed = 10f;
        public Vector2 ScaleRange = new Vector2(0.8f, 1.2f);

        private Transform pivot;
        private Transform cam;
        private Camera camera2D;

        private void Awake()
        {
            pivot = transform.Find("Pivot");
            cam = transform.Find("Pivot/Camera");
            camera2D = cam.GetComponent<Camera>();
            ScaleRange *= camera2D.orthographicSize;
        }

        private void OnEnable()
        {
            TwoFingerMoveGesture.Transformed += twoFingerTransformHandler;
            ManipulationGesture.Transformed += manipulationTransformedHandler;
        }

        private void OnDisable()
        {
            TwoFingerMoveGesture.Transformed -= twoFingerTransformHandler;
            ManipulationGesture.Transformed -= manipulationTransformedHandler;
        }

        private void manipulationTransformedHandler(object sender, System.EventArgs e)
        {
            //var rotation = Quaternion.Euler(ManipulationGesture.DeltaPosition.y/Screen.height*RotationSpeed,
            //    -ManipulationGesture.DeltaPosition.x/Screen.width*RotationSpeed,
            //    ManipulationGesture.DeltaRotation);
            //pivot.localRotation *= rotation;
            //cam.transform.localPosition += Vector3.forward*(ManipulationGesture.DeltaScale - 1f)*ZoomSpeed;
            var scale = Mathf.Clamp(ManipulationGesture.DeltaScale, ScaleRange.x, ScaleRange.y);
            Debug.LogWarning(scale);
            camera2D.orthographicSize *= ManipulationGesture.DeltaScale;
            camera2D.orthographicSize = Mathf.Clamp(camera2D.orthographicSize, ScaleRange.x, ScaleRange.y);
        }

        private void twoFingerTransformHandler(object sender, System.EventArgs e)
        {
            //pivot.localPosition += pivot.rotation*TwoFingerMoveGesture.DeltaPosition*PanSpeed;
            cam.localPosition -= new Vector3(TwoFingerMoveGesture.DeltaPosition.x, 0, TwoFingerMoveGesture.DeltaPosition.z) * PanSpeed;
        }
    }
}