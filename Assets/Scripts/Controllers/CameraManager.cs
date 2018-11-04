using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera mapViewCamera;

    public void Start()
    {
        mainCamera.GetComponent<Camera>().enabled = true;
        mapViewCamera.GetComponent<Camera>().enabled = false;
    }

    public void ToggleMapView()
    {
        if (mapViewCamera.enabled)
        {
            mapViewCamera.enabled = false;
            mainCamera.enabled = true;
        }
        else
        {
            mapViewCamera.enabled = true;
            mainCamera.enabled = false;
        }
    }
}
