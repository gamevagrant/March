using System.Collections;
using UnityEngine;

public class Guide_9_2Controller : MonoBehaviour , GuideCustomInterface
{
    private const string ShavolPrefab = "Shavol";
    private Animator shavol;

    void Start()
    {
        shavol = Instantiate(Resources.Load<GameObject>(ShavolPrefab), transform).GetComponent<Animator>();
        shavol.gameObject.SetActive(false);
    }

    public IEnumerator Hide()
    {
        shavol.gameObject.SetActive(value: true);
        shavol.Play("ShavolIt");
        yield return new WaitForSeconds(1f);
    }
}
