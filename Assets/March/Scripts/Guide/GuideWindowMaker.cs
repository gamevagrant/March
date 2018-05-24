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

    public enum GuideType
    {
        Guide_1_1,
        Guide_1_2,
        Guide_2_1,
        Guide_2_2,
        Guide_2_3,
        Guide_2_4,
        Guide_2_5,
        Guide_3_1,
        Guide_3_2,
        Guide_3_3,
        Guide_3_4,
        Guide_3_5,
        Guide_4_1,
        Guide_4_2,
        Guide_4_3,
        Guide_4_4,
        Guide_4_5,
        Guide_5_1,
        Guide_5_2,
        Guide_6_1,
        Guide_6_2,
        Guide_7_1,
        Guide_7_2,
        Guide_7_3,
        Guide_7_4,
        Guide_8_1,
        Guide_9_1,
        Guide_9_2,
        Guide_12_1,
        Guide_18_1,
    }

    public PathType SavePath;
    public GuideType GuideName;

    public List<RectTransform> ImageList;

    public List<GuideCondition> ConditionList;

    private Dictionary<PathType, string> pathDict = new Dictionary<PathType, string>
    {
        {PathType.FromResource, "March/Data/Resources/PlayGuide"},
        {PathType.FromAssetBundle, "AssetBundleResources/PlayGuide"}
    };

    private Transform imageContainer;
    private Transform guideMaskContainer;
    //private Transform guideHand;
    private Transform guideImage;
    private GuideWindowController guideWindow;
    private GuideHandController guideHandController;

    private GuideData data;

    private string GuideFileName
    {
        get { return string.Format("{0}.json", GuideName); }
    }

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        imageContainer = transform.Find("ImageContainer");
        ImageList.Clear();
        for (var i = 0; i < imageContainer.childCount; ++i)
        {
            ImageList.Add(imageContainer.GetChild(i).GetComponent<RectTransform>());
        }

        guideMaskContainer = transform.Find("GuideMask");
        guideHandController = transform.Find("GuideHand").GetComponent<GuideHandController>();
        guideImage = transform.Find("GuideImage");
        guideWindow = transform.Find("GuideWindow").GetComponent<GuideWindowController>();
    }

    [ContextMenu("Save")]
    void OnSaveClicked()
    {
        Initialize();

        SaveGuideData();

        var json = JsonUtility.ToJson(data);
        var path = string.Format("{0}/{1}/{2}", Application.dataPath, pathDict[SavePath], GuideFileName);
        File.WriteAllText(path, json);

        Debug.Log("Save file to path: " + path);
    }

    void SaveGuideData()
    {
        var path = string.Format("{0}/{1}/{2}", Application.dataPath, pathDict[SavePath], GuideFileName);
        data.Name = new FileInfo(path).Name.Replace(new FileInfo(path).Extension, string.Empty);

        // guide hand data.
        guideHandController.FlushUIToData();
        data.Hand = guideHandController.Data;

        // guide window data.
        guideWindow.Data.Position = new Position(guideWindow.GetComponent<RectTransform>().anchoredPosition);
        data.Window = guideWindow.Data;

        // guide item data list.
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

        // guide condition list.
        data.ConditionList.Clear();
        data.ConditionList.AddRange(ConditionList);
    }

    [ContextMenu("Load")]
    void OnLoadClicked()
    {
        Initialize();

        var path = string.Format("{0}/{1}/{2}", Application.dataPath, pathDict[SavePath], GuideFileName);
        var json = File.ReadAllText(path);
        data = JsonUtility.FromJson<GuideData>(json);

        Debug.Log("Load file from path: " + path);

        LoadGuideData();
    }

    void LoadGuideData()
    {
        guideHandController.Data = data.Hand;
        guideHandController.FlushDataToUI();

        guideWindow.Data = data.Window;
        guideWindow.FlushDataToUI();

        CleanupImage();
        ImageList.Clear();
        ImageList.AddRange(data.ItemList.Select(item =>
        {
            var rect = Instantiate(guideImage.gameObject, imageContainer).GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(item.AnchorMin.X, item.AnchorMin.Y);
            rect.anchorMax = new Vector2(item.AnchorMax.X, item.AnchorMax.Y);
            rect.pivot = new Vector2(item.Pivot.X, item.Pivot.Y);
            rect.anchoredPosition = new Vector3(item.AnchorPosition.X, item.AnchorPosition.Y, item.AnchorPosition.Z);
            rect.sizeDelta = new Vector3(item.Size.X, item.Size.Y, item.Size.Z);
            rect.name = item.ObjectName;
            return rect;
        }));

        ConditionList.Clear();
        ConditionList.AddRange(data.ConditionList);
    }
    private void CleanupImage()
    {
        for (var i = imageContainer.transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(imageContainer.transform.GetChild(i).gameObject);
        }
    }
}
