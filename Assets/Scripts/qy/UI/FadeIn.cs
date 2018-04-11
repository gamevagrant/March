using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

    private MaskableGraphic graphic;
    private float _to = 0.4f;
    public float to
    {
        get
        {
            return _to;
        }
        set
        {
            _to = value;
        }
    }
    // Use this for initialization
    private void Awake()
    {
        graphic = transform.GetComponent<MaskableGraphic>();
    }
    void Start ()
    {
       
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b,0);

    }

    private void Update()
    {
        if(graphic.color.a!= _to)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, Mathf.Lerp(graphic.color.a, _to, 1 * Time.deltaTime/0.2f));

        }
    }

}
