using March.Core.Guide;
using March.Scene;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GuideWindowController : MonoBehaviour
{
    public GuideWindowData Data;

    public UnityAction NextButtonAction;

    private Text contentText;
    private Text nextButtonText;
    private Button nextButton;
    private Transform head;

    private RectTransform rectTransform;
    private bool initialized;

    void Awake()
    {
        Initialize();

        FlushDataToUI();
    }

    void Initialize()
    {
        if (initialized)
            return;

        rectTransform = GetComponent<RectTransform>();

        contentText = transform.Find("Text").GetComponent<Text>();
        nextButtonText = transform.Find("NextButton/Text").GetComponent<Text>();
        nextButton = transform.Find("NextButton").GetComponent<Button>();
        head = transform.Find("Head");

        nextButton.onClick.AddListener(NextButtonAction);
    }

    [ContextMenu("UI to Data")]
    public void FlushUIToData()
    {
        Initialize();

        Data.Position = new Position(rectTransform.anchoredPosition);
        Data.HasNextButton = nextButton.gameObject.activeSelf;
        Data.HasHead = head.gameObject.activeSelf;
        Data.Content = contentText.text;
        Data.NextButtonContent = nextButtonText.text;
    }

    [ContextMenu("Data to UI")]
    public void FlushDataToUI()
    {
        Initialize();

        rectTransform.anchoredPosition = new Vector3(Data.Position.X, Data.Position.Y, Data.Position.Z);
        nextButton.gameObject.SetActive(Data.HasNextButton);
        head.gameObject.SetActive(Data.HasHead);
        contentText.text = Data.Content;
        nextButtonText.text = Data.NextButtonContent;
    }
}
