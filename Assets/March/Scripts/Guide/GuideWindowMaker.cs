using March.Core.Guide;
using March.Scene;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class GuideWindowMaker : MonoBehaviour
{
    public enum PathType
    {
        FromResource,
        FromAssetBundle,
    }

    public PathType SavePath;
    public string FileName;

    public List<RectTransform> ImageList;

    private Dictionary<PathType, string> pathDict = new Dictionary<PathType, string>
    {
        {PathType.FromResource, "March/Data/PlayGuide"},
        {PathType.FromAssetBundle, "AssetBundleResources/PlayGuide"}
    };

    private Transform imageContainer;
    private Transform guideMaskContainer;
    private Transform guideHand;
    private Transform guideImage;
    private GuideWindowController guideWindow;

    private GuideData data;
    private bool initialized;

    public RectTransform TestImage;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        if (initialized)
            return;

        initialized = true;
        imageContainer = transform.Find("ImageContainer");

        guideMaskContainer = transform.Find("GuideMask");
        guideHand = transform.Find("GuideHand");
        guideImage = transform.Find("GuideImage");
        guideWindow = transform.Find("GuideWindow").GetComponent<GuideWindowController>();
    }

    [ContextMenu("Save")]
    void OnSaveClicked()
    {
        if (string.IsNullOrEmpty(FileName))
        {
            Debug.Log("File could not be empty.");
            return;
        }

        Initialize();

        SaveGuideData();

        var json = JsonUtility.ToJson(data);
        var path = string.Format("{0}/{1}/{2}", Application.dataPath, pathDict[SavePath], FileName);
        File.WriteAllText(path, json);

        Debug.Log("Save file to path: " + path);
    }

    void SaveGuideData()
    {
        data.Hand.Show = guideHand.gameObject.activeSelf;
        data.Hand.Position = new Position(guideHand.GetComponent<RectTransform>().anchoredPosition);

        var path = string.Format("{0}/{1}/{2}", Application.dataPath, pathDict[SavePath], FileName);
        data.Name = new FileInfo(path).Name.Replace(new FileInfo(path).Extension, string.Empty);
        guideWindow.FlushUIToData();
        data.Window = guideWindow.Data;
        data.ItemList.Clear();
        data.ItemList.AddRange(ImageList.Select(rectTransform => new GuideItem
        {
            ObjectName = rectTransform.name,
            AnchorMin = new Position(rectTransform.anchorMin),
            AnchorMax = new Position(rectTransform.anchorMax),
            Pivot = new Position(rectTransform.pivot),
            AnchorPosition = new Position(rectTransform.anchoredPosition),
            Size = new Position(rectTransform.rect.size)
        }));
    }

    [ContextMenu("Load")]
    void OnLoadClicked()
    {
        if (string.IsNullOrEmpty(FileName))
        {
            Debug.Log("File could not be empty.");
            return;
        }

        initialized = false;

        Initialize();

        var path = string.Format("{0}/{1}/{2}", Application.dataPath, pathDict[SavePath], FileName);
        var json = File.ReadAllText(path);
        data = JsonUtility.FromJson<GuideData>(json);

        Debug.Log("Load file from path: " + path);

        LoadGuideData();
    }

    void LoadGuideData()
    {
        guideHand.gameObject.SetActive(data.Hand.Show);
        guideHand.GetComponent<RectTransform>().anchoredPosition = new Vector3(data.Hand.Position.X, data.Hand.Position.Y, data.Hand.Position.Z);

        guideWindow.Data = data.Window;
        guideWindow.FlushDataToUI();

        CleanupImage();
        ImageList.Clear();
        ImageList.AddRange(data.ItemList.Select(item =>
        {
            var rect = Instantiate(guideImage.gameObject, imageContainer).GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector3(item.AnchorPosition.X, item.AnchorPosition.Y, item.AnchorPosition.Z);
            rect.sizeDelta = new Vector3(item.Size.X, item.Size.Y, item.Size.Z);
            rect.name = item.ObjectName;
            return rect;
        }));
    }
    private void CleanupImage()
    {
        for (var i = imageContainer.transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(imageContainer.transform.GetChild(i).gameObject);
        }
    }

    [ContextMenu("RectTransform information")]
    void GetRectInformation()
    {
        Debug.LogWarning(TestImage);

        Debug.LogWarning(TestImage.anchoredPosition);
        Debug.LogWarning(TestImage.sizeDelta);
        Debug.LogWarning(TestImage.rect);
        Debug.LogWarning(TestImage.rect.center);
        Debug.LogWarning(TestImage.rect.size);
        Debug.LogWarning("x-" + TestImage.rect.x + ", y-" + TestImage.rect.y + ", width-" + TestImage.rect.width + ", height-" + TestImage.rect.height);
    }
}
