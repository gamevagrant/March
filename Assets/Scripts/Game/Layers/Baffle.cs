using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baffle : MonoBehaviour{

    public BAFFLE_TYPE type;
    public Node node;

    public void DestroyBaffle()
    {
        if (type == BAFFLE_TYPE.BAFFLE_BOTTOM)
        {
            node.bafflebottom = null;
        }
        else if (type == BAFFLE_TYPE.BAFFLE_RIGHT)
        {
            node.baffleright = null;
        }
        Destroy(gameObject);
    }
}
