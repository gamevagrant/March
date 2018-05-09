using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingProgressController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float Min;

    [Range(0f, 1f)]
    public float PreMax;

    private Image bar;
    private Image barRight;
    private Text percentageText;

    public float PreMaxDuration;
    public float MaxDuration;

    private Tweener tweener;
    private Tweener tweener2;

    private void Awake()
    {
        bar = transform.Find("bg/bar").GetComponent<Image>();
        barRight = transform.Find("bg/bar_right").GetComponent<Image>();
        percentageText = transform.Find("bg/right").GetComponent<Text>();

        bar.fillAmount = Min;
        NotifyUI();
    }

    private void Start()
    {
        tweener = bar.DOFillAmount(PreMax, PreMaxDuration).OnUpdate(NotifyUI).OnComplete(CompleteTween);
        Debug.LogWarning("Tween 1 start");
    }

    void CompleteTween()
    {
        Debug.LogWarning("Tween 1 complete " + bar.fillAmount);

        Debug.LogWarning("Tween 2 start " + bar.fillAmount);

        tweener2 = bar.DOFillAmount(1f, PreMaxDuration).OnUpdate(NotifyUI).OnComplete(TrimTween2);
    }

    void CompleteTween2()
    {
        Debug.LogWarning("Tween 2 complete " + bar.fillAmount);
    }

    public void TrimTween()
    {
        tweener.Complete();
    }

    public void TrimTween2()
    {
        tweener2.Complete();
    }

    private void NotifyUI()
    {
        percentageText.text = string.Format("{0:0}%", bar.fillAmount * 100);

        if (bar.fillAmount >= 1f)
        {
            barRight.gameObject.SetActive(true);
        }
    }
}
