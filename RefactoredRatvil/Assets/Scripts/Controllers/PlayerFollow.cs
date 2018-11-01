using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform PlayerTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public bool LookAtPlayer = false;
    
    private void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
    }
    
    private void LateUpdate()
    {
        Vector3 newPosition = PlayerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPosition, SmoothFactor);

        if (LookAtPlayer)
        {
            transform.LookAt(PlayerTransform);
        }
    }
}
