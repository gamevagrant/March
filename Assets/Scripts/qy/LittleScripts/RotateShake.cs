using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShake : MonoBehaviour {

    public float speed = 1;
    public float angles = 20;

    private float sin;
    private Quaternion q;
    // Use this for initialization
    void Start () {
        q = Quaternion.identity;

    }
	
	// Update is called once per frame
	void Update () {
        sin = Mathf.Sin(Time.time * speed);
        q.eulerAngles = new Vector3(0, 0, sin * angles);
        transform.localRotation = q;
    }
}
