using UnityEngine;

public class UpDownGravity : MonoBehaviour
{
    public float height;

    private Vector3 force;
    private Rigidbody rb;
    private float downPosition, upPosition;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        downPosition = gameObject.transform.position.y;
        upPosition = downPosition + height;
        force = Vector3.up;
    }

    private void FixedUpdate()
    {

        if (gameObject.transform.position.y < downPosition)
        {
            force = Vector3.up;
        }
        else if (gameObject.transform.position.y > upPosition)
        {
            force = Vector3.down;
        }

        rb.AddForce(force, ForceMode.Acceleration);
    }
}