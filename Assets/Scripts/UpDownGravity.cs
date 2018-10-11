using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownGravity : MonoBehaviour {

    public int intensity;

    private Rigidbody rb;
    private float downPosition;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        downPosition = gameObject.transform.position.y;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (gameObject.transform.position.y < downPosition) rb.AddForce(Vector3.up * intensity, ForceMode.Force);
	}
}
