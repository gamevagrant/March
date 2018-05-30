using DG.Tweening;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toolkit;
using TouchScript.Gestures;
using UnityEngine;

namespace March.Scene
{
    public class SceneEditor : MonoBehaviour
    {
        public SceneSerializer Serializer;
        public ScenePanelController PanelController;

        public float ShakeDuration = 0.5f;
        [Range(0, 1f)]
        public float ShakePower = 0.5f;

        public bool IsModifyMode { get; private set; }

        private List<Tweener> shakeTweenerList = new List<Tweener>();
        private bool dirty;

        private LongPressGesture LongGesture;
        private TapGesture DoubleTapGesture;

        private const string RelativePath = "./Assets/Square/Data/LevelEditor/Resources";

        private void Awake()
        {
            LongGesture = GetComponent<LongPressGesture>();
            DoubleTapGesture = GetComponent<TapGesture>();
        }

        private void Start()
        {
            Serializer.SceneIsReady += OnSceneIsReady;
        }

        private void OnSceneIsReady(object sender, EventArgs args)
        {
            PanelController.DataMap = Serializer.SceneLoader.SceneMap;
            PanelController.FlushUI();
        }

        private void OnEnable()
        {
            LongGesture.LongPressed += OnLongPressHandler;
            DoubleTapGesture.Tapped += OnDoublePressHandler;
        }

        private void OnDisable()
        {
            LongGesture.LongPressed -= OnLongPressHandler;
            DoubleTapGesture.Tapped -= OnDoublePressHandler;
        }

        public void OnInstanceClicked(InstanceController sender)
        {
            dirty = true;

            var key = sender.ItemText;
            var index = sender.CurrentItemIndex;
            var isModifiable = Serializer.SceneLayerMap[key].Modifiable;

            var position = isModifiable ? SceneUtils.GetScreenCenterPosition() : Vector3.zero;

            //var go = SceneUtils.CreateIdentifyGameObject(key, sender.CurrentItemIndex, Serializer.SceneLayerMap[key],
            //    Serializer.DefaultSceneDataMap[key].Position,
            //    Serializer.DefaultSceneDataMap[key].Scale, Serializer.DefaultSceneDataMap[key].SortingOrder,
            //    isModifiable);
            var go = Serializer.SceneLoader.CreateIdentifyGameObject(Serializer.SceneLayerMap[key], index, position);

            if (!isModifiable && Serializer.GoMap.ContainsKey(key))
                Destroy(Serializer.GoMap[key][0]);

            if (!Serializer.GoMap.ContainsKey(key))
                Serializer.GoMap.Add(key, new List<GameObject>());

            Serializer.GoMap[key].Add(go);
        }

        public void OnSaveClicked()
        {
            Toolkit.MessageBox.Show(
                string.Format("Are you sure save level {0}", PanelController.Level),
                "Warning",
                result =>
                {
                    if (result == DialogResult.Yes)
                    {
                        DoSave();
                    }
                },
                MessageBoxButtons.YesNo);
        }

        private void DoSave()
        {
            dirty = false;

            var path = Path.Combine(RelativePath, "Level_" + PanelController.Level + ".txt");
            Serializer.Write(PanelController.Level, path);

            PanelController.Info = Serializer.Message;
        }

        public void OnLoadClicked()
        {
            if (dirty)
            {
                Toolkit.MessageBox.Show(
                    "Loading will discard the chagnes you make.",
                    "Warning",
                    result =>
                    {
                        if (result == DialogResult.Yes)
                        {
                            DoLoad();
                        }
                    },
                    MessageBoxButtons.YesNo);
            }
            else
            {
                DoLoad();
            }
        }

        private void DoLoad()
        {
            dirty = false;

            var path = Path.Combine(RelativePath, "Level_" + PanelController.Level + ".txt");
            Serializer.Read(path);

            PanelController.Info = Serializer.Message;
        }

        public void OnClearAllButtonClicked()
        {
            Serializer.ClearGoMap();
        }


        public void RemoveItem(string path, GameObject go)
        {
            Serializer.RemoveItem(path, go);
            PanelController.Info = Serializer.Message;
        }

        private void OnLongPressHandler(object sender, EventArgs e)
        {
            if (IsModifyMode)
                return;

            IsModifyMode = true;

            OnEditStart();
        }

        private void OnEditStart()
        {
            shakeTweenerList.Clear();
            Serializer.SceneLayerMap.Where(pair => pair.Value.Modifiable).Select(pair => pair.Value.transform).ToList().ForEach(
                trans =>
                {
                    var tweener = trans.DOShakePosition(ShakeDuration, ShakePower).SetLoops(-1);
                    shakeTweenerList.Add(tweener);
                });
        }

        private void OnDoublePressHandler(object sender, EventArgs e)
        {
            if (!IsModifyMode)
                return;

            IsModifyMode = false;

            OnEditEnd();
        }

        private void OnEditEnd()
        {
            shakeTweenerList.ForEach(tweener => tweener.Kill());
        }
    }
}