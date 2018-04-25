using System;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

namespace March.Scene
{
    /// <summary>
    /// Spawn as a child of a sprite.
    /// </summary>
    /// <remarks>Pay attention to itemTransform, which is parent we reference to.</remarks>
    public class ItemActionController : MonoBehaviour
    {
        public SceneEditor SceneEditor;

        public string Path;
        public int Index;

        public bool IsModifiable { get; set; }

        public float MoveSpeed = 1f;

        private PressGesture ClosePressGesture;
        private TransformGesture MoveGesture;

        private SpriteRenderer spriteRenderer;
        private Transform itemTransform;
        private Collider2D itemCollider;
        private Transform deleteButtonTrans;

        private BoxCollider boxCollider;

        private void Awake()
        {
            MoveGesture = GetComponent<TransformGesture>();

            deleteButtonTrans = transform.Find("Delete");
            ClosePressGesture = deleteButtonTrans.GetComponent<PressGesture>();

            boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = IsModifiable;
        }

        private void Start()
        {
            itemTransform = transform.parent;
            itemCollider = itemTransform.GetComponent<Collider2D>();
            if (itemCollider != null)
                itemCollider.enabled = false;
            spriteRenderer = itemTransform.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = true;

            AdjustDeleteButton();
        }

        private void OnEnable()
        {
            ClosePressGesture.Pressed += OnClosePressHandler;
            MoveGesture.Transformed += OnMoveHandler;
        }

        private void OnDisable()
        {
            ClosePressGesture.Pressed -= OnClosePressHandler;
            MoveGesture.Transformed -= OnMoveHandler;
        }

        private void OnClosePressHandler(object sender, EventArgs e)
        {
            if (!SceneEditor.IsModifyMode)
                return;

            SceneEditor.RemoveItem(Path, itemTransform.gameObject);
        }

        private void OnMoveHandler(object sender, EventArgs e)
        {
            if (!SceneEditor.IsModifyMode)
                return;

            itemTransform.localPosition +=
                new Vector3(MoveGesture.DeltaPosition.x, MoveGesture.DeltaPosition.y, MoveGesture.DeltaPosition.z) * MoveSpeed;
        }

        private void AdjustDeleteButton()
        {
            var y = boxCollider.size.y / 2;
            deleteButtonTrans.position += new Vector3(-y, y, 0);

            var render = deleteButtonTrans.GetComponent<SpriteRenderer>();
            render.sortingOrder = spriteRenderer.sortingOrder + 1;
        }

        private void ActiveDeleteButton(bool flag)
        {
            deleteButtonTrans.gameObject.SetActive(flag);
        }

        private void Update()
        {
            ActiveDeleteButton(IsModifiable && SceneEditor.IsModifyMode);
            boxCollider.enabled = IsModifiable && SceneEditor.IsModifyMode;
        }
    }
}