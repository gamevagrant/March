using UnityEngine;

public class Guide_9_2Controller : MonoBehaviour
{
    private const string ShavolPrefab = "Shavol";
    private Animator shavol;

    void Start()
    {
        shavol = Instantiate(Resources.Load<GameObject>(ShavolPrefab), transform).GetComponent<Animator>();
    }
}
