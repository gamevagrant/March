using March.Core.Guide;
using System.Collections.Generic;
using March.Scene;
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
        Cleanup();

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
            rect.anchorMin = new Vector2(item.AnchorMin.X, item.AnchorMin.Y);
            rect.anchorMax = new Vector2(item.AnchorMax.X, item.AnchorMax.Y);
            rect.pivot = new Vector2(item.Pivot.X, item.Pivot.Y);
            rect.anchoredPosition = new Vector3(item.AnchorPosition.X, item.AnchorPosition.Y, item.AnchorPosition.Z);
            rect.sizeDelta = new Vector3(item.Size.X, item.Size.Y, item.Size.Z);
            rect.name = item.ObjectName;

            // normalize rect into normal space.
            var rectNormal = Instantiate(Resources.Load<GameObject>(GuideImage), guideInstance.transform, false).GetComponent<RectTransform>();
            rectNormal.sizeDelta = rect.sizeDelta;
            rectNormal.localPosition = rect.localPosition;

            item.AnchorMin = item.AnchorMax = item.Pivot = new Position(0.5f, 0.5f, 0);
            item.AnchorPosition = new Position(rectNormal.anchoredPosition.x, rectNormal.anchoredPosition.y, 0);
            item.Size = new Position(rectNormal.sizeDelta.x, rectNormal.sizeDelta.y, 0);
        });

        CleanupMask();

        var mask0 = Instantiate(Resources.Load<RectTransform>(GuideMask), maskContainer).GetComponent<RectTransform>();
        mask0.anchoredPosition = Vector2.zero;
        mask0.name = mask0.name + "_0";
        mask0.sizeDelta = maskContainer.GetComponent<RectTransform>().rect.size;
        GenerateMask(mask0);
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

            parent.gameObject.SetActive(false);

            var maskList = GenerateMask(item, parent);
            maskList.ForEach(GenerateMask);
        }
    }

    private bool InRange(GuideItem item, RectTransform parent)
    {
        return item.AnchorPosition.X > parent.anchoredPosition.x - parent.rect.size.x / 2 && (item.AnchorPosition.X < parent.anchoredPosition.x + parent.rect.size.x / 2)
               && (item.AnchorPosition.Y > parent.anchoredPosition.y - parent.rect.size.y / 2) && item.AnchorPosition.Y < parent.anchoredPosition.y + parent.rect.size.y / 2;
    }

    private List<RectTransform> GenerateMask(GuideItem item, RectTransform parent)
    {
        var result = new List<RectTransform>();

        if (item.AnchorPosition.X - item.Size.X / 2 > parent.anchoredPosition.x - parent.rect.size.x / 2)
        {
            var left = Instantiate(Resources.Load<RectTransform>(GuideMask), maskContainer);
            left.name = parent.name + "_1";
            left.anchoredPosition = new Vector2(
                (item.AnchorPosition.X - item.Size.X / 2 + (parent.anchoredPosition.x - parent.rect.size.x / 2)) / 2,
                (item.AnchorPosition.Y - item.Size.Y / 2 + (parent.anchoredPosition.y + parent.rect.size.y / 2)) / 2);
            left.sizeDelta = new Vector2(
                Mathf.Abs(item.AnchorPosition.X - item.Size.X / 2 - (parent.anchoredPosition.x - parent.rect.size.x / 2)),
                Mathf.Abs(item.AnchorPosition.Y - item.Size.Y / 2 - (parent.anchoredPosition.y + parent.rect.size.y / 2)));
            result.Add(left);
        }

        if (item.AnchorPosition.Y - item.Size.Y / 2 > parent.anchoredPosition.y - parent.rect.size.y / 2)
        {
            var bottom = Instantiate(Resources.Load<RectTransform>(GuideMask), maskContainer);
            bottom.name = parent.name + "_2";
            bottom.anchoredPosition = new Vector2(
                (item.AnchorPosition.X + item.Size.X / 2 + (parent.anchoredPosition.x - parent.rect.size.x / 2)) / 2,
                (item.AnchorPosition.Y - item.Size.Y / 2 + (parent.anchoredPosition.y - parent.rect.size.y / 2)) / 2);
            bottom.sizeDelta = new Vector2(
                Mathf.Abs(item.AnchorPosition.X + item.Size.X / 2 - (parent.anchoredPosition.x - parent.rect.size.x / 2)),
                Mathf.Abs(item.AnchorPosition.Y - item.Size.Y / 2 - (parent.anchoredPosition.y - parent.rect.size.y / 2)));
            result.Add(bottom);
        }

        if (item.AnchorPosition.X + item.Size.X / 2 < parent.anchoredPosition.x + parent.rect.size.x / 2)
        {
            var right = Instantiate(Resources.Load<RectTransform>(GuideMask), maskContainer);
            right.name = parent.name + "_3";
            right.anchoredPosition = new Vector2(
                (item.AnchorPosition.X + item.Size.X / 2 + (parent.anchoredPosition.x + parent.rect.size.x / 2)) / 2,
                (item.AnchorPosition.Y + item.Size.Y / 2 + (parent.anchoredPosition.y - parent.rect.size.y / 2)) / 2);
            right.sizeDelta = new Vector2(
                Mathf.Abs(item.AnchorPosition.X + item.Size.X / 2 - (parent.anchoredPosition.x + parent.rect.size.x / 2)),
                Mathf.Abs(item.AnchorPosition.Y + item.Size.Y / 2 - (parent.anchoredPosition.y - parent.rect.size.y / 2)));
            result.Add(right);
        }

        if (item.AnchorPosition.Y + item.Size.Y / 2 < parent.anchoredPosition.y + parent.rect.size.y / 2)
        {
            var top = Instantiate(Resources.Load<RectTransform>(GuideMask), maskContainer);
            top.name = parent.name + "_4";
            top.anchoredPosition = new Vector2(
                (item.AnchorPosition.X - item.Size.X / 2 + (parent.anchoredPosition.x + parent.rect.size.x / 2)) / 2,
                (item.AnchorPosition.Y + item.Size.Y / 2 + (parent.anchoredPosition.y + parent.rect.size.y / 2)) / 2);
            top.sizeDelta = new Vector2(
                Mathf.Abs(item.AnchorPosition.X - item.Size.X / 2 - (parent.anchoredPosition.x + parent.rect.size.x / 2)),
                Mathf.Abs(item.AnchorPosition.Y + item.Size.Y / 2 - (parent.anchoredPosition.y + parent.rect.size.y / 2)));
            result.Add(top);
        }

        return result;
    }
}
