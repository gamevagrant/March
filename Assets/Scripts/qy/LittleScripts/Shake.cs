using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shake : MonoBehaviour {

    public int strangth = 1;
    public int vibrato = 10;
    public int randomness = 90;
    public bool snapping = false;
    public bool fadeout = true;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        ShakeTransform();
    }

    private void ShakeTransform()
    {
        transform.DOShakePosition(0.5f, strangth, vibrato, randomness, snapping, fadeout);
    }
}
