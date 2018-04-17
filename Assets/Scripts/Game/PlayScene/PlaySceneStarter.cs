using UnityEngine;

public class PlaySceneStarter : MonoBehaviour
{
    private const string BackgroundPath = "Background/2/bg";

    void Start()
    {
        LoadBackground();
    }

    private void LoadBackground()
    {
        var sprite = Resources.Load<Sprite>(BackgroundPath);
        var background = new GameObject("Background", typeof(ScaleToScreenSize), typeof(SpriteRenderer));
        background.transform.parent = transform;
        background.transform.localPosition = Vector3.zero;
        background.transform.localRotation = Quaternion.identity;
        background.transform.localScale = Vector3.one;
        var render = background.GetComponent<SpriteRenderer>();
        render.sprite = sprite;
    }
}
