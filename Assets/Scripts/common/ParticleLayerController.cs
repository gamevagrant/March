using UnityEngine;

public class ParticleLayerController : MonoBehaviour
{
    void Start()
    {
        foreach (var ps in transform.GetComponentsInChildren<ParticleSystem>())
        {
            ps.GetComponent<Renderer>().sortingLayerName = SortingLayers.Effect;
        }
    }
}
