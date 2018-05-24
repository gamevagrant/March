using System.Collections;
using UnityEngine;

public class Guide_9_2Controller : MonoBehaviour , GuideCustomInterface
{
    private const string ShavolPrefab = "Shavol";
    private Animator shavol;

    void Start()
    {
        shavol = Instantiate(Resources.Load<GameObject>(ShavolPrefab), transform).GetComponent<Animator>();
        shavol.enabled = false;
    }

    public IEnumerator Hide()
    {
        shavol.enabled = true;
        shavol.Play("ShavolIt");
        yield return new WaitForSeconds(1f);
    }
}
