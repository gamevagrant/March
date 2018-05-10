using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingProgressController : MonoBehaviour
{
    public Transform Begin;
    public Transform End;

    public int Partition;
    public float TweenDuration;

    [Range(0f, 1f)]
    public float MaxFillAmount;

    public Ease EaseType;

    private Image bar;
    private Image barRight;
    private Transform person;
    private Text percentageText;

    private Tweener currentTweener;

    private void Awake()
    {
        bar = transform.Find("bg/bar").GetComponent<Image>();
        barRight = transform.Find("bg/bar_right").GetComponent<Image>();
        person = transform.Find("bg/person");
        percentageText = transform.Find("bg/right").GetComponent<Text>();

        bar.fillAmount = 0f;
        NotifyUI();
    }

    private void Start()
    {
        Debug.LogWarning("Tween start");

        GenerateTween(0f);
    }

    private void GenerateTween(float start)
    {
        var end = start + 1f / Partition * (1 - start);

        if (end > MaxFillAmount)
            return;

        currentTweener = bar.DOFillAmount(start + 1f / Partition * (1 - start), TweenDuration).SetEase(EaseType)
            .OnUpdate(NotifyUI).OnComplete(() =>
            {
                start = end;
                GenerateTween(start);
            });
    }

    public void StopTween()
    {
        currentTweener.Kill();
        bar.fillAmount = 1f;
        NotifyUI();
    }

    private void NotifyUI()
    {
        percentageText.text = string.Format("{0:0}%", bar.fillAmount * 100);
        person.position = Begin.position + (End.position - Begin.position) * bar.fillAmount;

        if (bar.fillAmount >= 1f)
        {
            barRight.gameObject.SetActive(true);
        }
    }
}
