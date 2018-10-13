using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int cameraRotation, cameraVelocity;

    private bool followCurrentPlayer, inPosition;
    private Game game;

    // Use this for initialization
    void Start () {
        game = GameObject.Find("Game").GetComponent<Game>();
        SetFollowCurrentPlayer(true);
    }
	
	// Update is called once per frame
	void Update () {
		if (followCurrentPlayer)
        {
            if (!inPosition)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(game.GetCurrentPlayer().transform.position - transform.position), cameraRotation * Time.deltaTime);

                Vector3 position = game.GetCameraPlayerPosition().transform.localPosition;
                transform.localPosition = transform.localPosition + ((position - transform.localPosition) * Time.deltaTime);

                if ((position - transform.localPosition).magnitude < 0.2f)
                {
                    inPosition = true;
                }
                //Debug.Log((position - transform.localPosition).magnitude);
            }
        }
	}

    public void SetFollowCurrentPlayer(bool variable)
    {
        if (!variable) transform.SetParent(null);
        else transform.SetParent(game.GetCurrentPlayer().transform);
        followCurrentPlayer = variable;
        inPosition = false;
    }

    public void MoveCameraLookPlayerTurn()
    {
        StopAllCoroutines();
        SetFollowCurrentPlayer(true);
    }

    public void MoveCameraLookDestination(Vector3 position, GameObject destination)
    {
        SetFollowCurrentPlayer(false);
        StopAllCoroutines();
        StartCoroutine(MoveCameraLookDestinationCoroutine(position, destination));
    }

    private IEnumerator MoveCameraLookDestinationCoroutine(Vector3 position, GameObject destination)
    {
        while ((position - transform.position).magnitude > 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((destination.transform.position - transform.position)), cameraRotation * Time.deltaTime);
            transform.position = transform.position + ((position - transform.position) * cameraVelocity / 50f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void MoveCameraRotation(Vector3 position, Vector3 rotation)
    {
        SetFollowCurrentPlayer(false);
        StopAllCoroutines();
        StartCoroutine(MoveCameraRotationEulerCoroutine(position, rotation));
    }

    private IEnumerator MoveCameraRotationEulerCoroutine(Vector3 position, Vector3 rotation)
    {
        while ((position - transform.position).magnitude > 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), cameraRotation * Time.deltaTime);
            transform.position = transform.position + ((position - transform.position) * cameraVelocity / 50f);
            yield return new WaitForSeconds(0.02f);
        }
    }

}
