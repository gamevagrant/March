﻿using March.Core.Guide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideController : MonoBehaviour
{
    public TextAsset GuideText;
    public GuideData GuideData;

    public int StepMax;

    private const string GuideTemplate = "GuideTemplate";
    private const string GuideHand = "GuideHand";
    private const string GuideWindow = "GuideWindow";
    private const string GuideMask = "GuideMask";
    private const string GuideImage = "GuideImage";

    private GameObject guideInstance;
    private RectTransform guideHand;
    private GuideWindowController guideWindow;
    private Transform maskContainer;

    private Queue<GuideItem> guideItemQueue = new Queue<GuideItem>();

    private int step;

    private void Start()
    {
        Generate();
    }

    private void Initialize()
    {
        if (guideInstance == null)
        {
            guideInstance = Instantiate(Resources.Load<GameObject>(GuideTemplate), transform);
        }

        if (maskContainer == null)
            maskContainer = guideInstance.transform.Find("Mask");

        if (GuideText != null)
        {
            GuideData = JsonUtility.FromJson<GuideData>(GuideText.text);
        }

        guideItemQueue.Clear();
        GuideData.ItemList.ForEach(guideItemQueue.Enqueue);

        step = 0;
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        Initialize();

        name = GuideData.Name;

        if (GuideData.Hand.Show)
        {
            guideHand = Instantiate(Resources.Load<GameObject>(GuideHand), guideInstance.transform, false).GetComponent<RectTransform>();
            guideHand.anchoredPosition = new Vector3(GuideData.Hand.Position.X, GuideData.Hand.Position.Y, GuideData.Hand.Position.Z);
        }

        guideWindow = Instantiate(Resources.Load<GameObject>(GuideWindow), guideInstance.transform, false).GetComponent<GuideWindowController>();
        guideWindow.Data = GuideData.Window;
        guideWindow.FlushDataToUI();

        GuideData.ItemList.ForEach(item =>
        {
            var rect = Instantiate(Resources.Load<GameObject>(GuideImage), guideInstance.transform, false).GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector3(item.Position.X, item.Position.Y, item.Position.Z);
            rect.sizeDelta = new Vector3(item.Size.X, item.Size.Y, item.Size.Z);
            rect.name = item.ObjectName;
        });

        CleanupMask();
        GenerateMask(maskContainer.GetComponent<RectTransform>());
    }

    [ContextMenu("Cleanup")]
    public void Cleanup()
    {
        if (guideInstance != null)
            DestroyImmediate(guideInstance);
    }

    private void CleanupMask()
    {
        for (var i = 0; i < maskContainer.transform.childCount; ++i)
        {
            Destroy(maskContainer.transform.GetChild(i).gameObject);
        }
    }

    private void GenerateMask(RectTransform parent)
    {
        if (step > StepMax)
            return;

        if (guideItemQueue.Count <= 0)
            return;

        var item = guideItemQueue.Peek();
        if (InRange(item, parent))
        {
            step++;

            guideItemQueue.Dequeue();

            parent.GetComponent<Image>().enabled = false;

            var maskList = GenerateMask(item, parent);
            maskList.ForEach(GenerateMask);
        }
    }

    private bool InRange(GuideItem item, RectTransform parent)
    {
        return item.Position.X > parent.anchoredPosition.x - parent.rect.size.x / 2 && (item.Position.X < parent.anchoredPosition.x + parent.rect.size.x / 2)
               && (item.Position.Y > parent.anchoredPosition.y - parent.rect.size.y / 2) && item.Position.Y < parent.anchoredPosition.y + parent.rect.size.y / 2;
    }

    private List<RectTransform> GenerateMask(GuideItem item, RectTransform parent)
    {
        var result = new List<RectTransform>();

        if (item.Position.X - item.Size.X / 2 > parent.anchoredPosition.x - parent.rect.size.x / 2)
        {
            var left = Instantiate(Resources.Load<RectTransform>(GuideMask), maskContainer);
            left.name = parent.name + "_1";
            left.anchoredPosition = new Vector2(
                (item.Position.X - item.Size.X / 2 + (parent.anchoredPosition.x - parent.rect.size.x / 2)) / 2,
                (item.Position.Y - item.Size.Y / 2 + (parent.anchoredPosition.y + parent.rect.size.y / 2)) / 2);
            left.sizeDelta = new Vector2(
                Mathf.Abs(item.Position.X - item.Size.X / 2 - (parent.anchoredPosition.x - parent.rect.size.x / 2)),
                Mathf.Abs(item.Position.Y - item.Size.Y / 2 - (parent.anchoredPosition.y + parent.rect.size.y / 2)));
            result.Add(left);
        }

        if (item.Position.Y - item.Size.Y / 2 > parent.anchoredPosition.y - parent.rect.size.y / 2)
        {
            var bottom = Instantiate(Resources.Load<RectTransform>(GuideMask), maskContainer);
            bottom.name = parent.name + "_2";
            bottom.anchoredPosition = new Vector2(
                (item.Position.X + item.Size.X / 2 + (parent.anchoredPosition.x - parent.rect.size.x / 2)) / 2,
                (item.Position.Y - item.Size.Y / 2 + (parent.anchoredPosition.y - parent.rect.size.y / 2)) / 2);
            bottom.sizeDelta = new Vector2(
                Mathf.Abs(item.Position.X + item.Size.X / 2 - (parent.anchoredPosition.x - parent.rect.size.x / 2)),
                Mathf.Abs(item.Position.Y - item.Size.Y / 2 - (parent.anchoredPosition.y - parent.rect.size.y / 2)));
            result.Add(bottom);
        }

        if (item.Position.X + item.Size.X / 2 < parent.anchoredPosition.x + parent.rect.size.x / 2)
        {
            var right = Instantiate(Resources.Load<RectTransform>(GuideMask), maskContainer);
            right.name = parent.name + "_3";
            right.anchoredPosition = new Vector2(
                (item.Position.X + item.Size.X / 2 + (parent.anchoredPosition.x + parent.rect.size.x / 2)) / 2,
                (item.Position.Y + item.Size.Y / 2 + (parent.anchoredPosition.y - parent.rect.size.y / 2)) / 2);
            right.sizeDelta = new Vector2(
                Mathf.Abs(item.Position.X + item.Size.X / 2 - (parent.anchoredPosition.x + parent.rect.size.x / 2)),
                Mathf.Abs(item.Position.Y + item.Size.Y / 2 - (parent.anchoredPosition.y - parent.rect.size.y / 2)));
            result.Add(right);
        }

        if (item.Position.Y + item.Size.Y / 2 < parent.anchoredPosition.y + parent.rect.size.y / 2)
        {
            var top = Instantiate(Resources.Load<RectTransform>(GuideMask), maskContainer);
            top.name = parent.name + "_4";
            top.anchoredPosition = new Vector2(
                (item.Position.X - item.Size.X / 2 + (parent.anchoredPosition.x + parent.rect.size.x / 2)) / 2,
                (item.Position.Y + item.Size.Y / 2 + (parent.anchoredPosition.y + parent.rect.size.y / 2)) / 2);
            top.sizeDelta = new Vector2(
                Mathf.Abs(item.Position.X - item.Size.X / 2 - (parent.anchoredPosition.x + parent.rect.size.x / 2)),
                Mathf.Abs(item.Position.Y + item.Size.Y / 2 - (parent.anchoredPosition.y + parent.rect.size.y / 2)));
            result.Add(top);
        }

        return result;
    }
}
