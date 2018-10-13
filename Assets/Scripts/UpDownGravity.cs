using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownGravity : MonoBehaviour {

    public float height;

    private Vector3 force;
    private Rigidbody rb;
    private float downPosition, upPosition;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        downPosition = gameObject.transform.position.y;
        upPosition = downPosition + height;
        force = Vector3.up;
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (gameObject.transform.position.y < downPosition) force = Vector3.up;
        else if (gameObject.transform.position.y > upPosition) force = Vector3.down;

        rb.AddForce(force, ForceMode.Acceleration);
	}
}
