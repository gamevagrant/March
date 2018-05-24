using DG.Tweening;
using March.Core.Guide;
using March.Scene;
using UnityEngine;

public class GuideHandController : MonoBehaviour
{
    public GuideHandData Data;

    private Vector2 baseUnit;
    private RectTransform guideHand;
    private Tweener tweener;

    private void Initialize()
    {
        guideHand = GetComponent<RectTransform>();
        baseUnit = guideHand.sizeDelta;
    }

    [ContextMenu("Flush UI to Data")]
    public void FlushUIToData()
    {
        Initialize();

        Data.Show = gameObject.activeSelf;
        Data.Position = new Position(guideHand.anchoredPosition);
    }

    [ContextMenu("Flush Data to UI")]
    public void FlushDataToUI()
    {
        Initialize();

        gameObject.SetActive(Data.Show);
        guideHand.anchoredPosition = new Vector2(Data.Position.X, Data.Position.Y);
    }

    public void Animate()
    {
        if (tweener == null)
            tweener = guideHand
                .DOAnchorPos(
                    new Vector2(baseUnit.x * Data.Direction.X + guideHand.anchoredPosition.x,
                        baseUnit.y * Data.Direction.Y + guideHand.anchoredPosition.y), Data.Duration).SetLoops(-1);
    }

    void OnDestroy()
    {
        tweener.Kill();
    }
}
