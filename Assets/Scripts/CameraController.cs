using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int cameraRotation, cameraVelocity;

    private bool currentPlayer, positioned;
    private Game game;

    private void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        SetCurrentPlayer(true);
    }

    private void Update()
    {
        if (currentPlayer)
        {
            if (!positioned)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(game.GetCurrentPlayer().transform.position - transform.position), cameraRotation * Time.deltaTime);

                Vector3 position = game.GetPlayersPositionCamera().transform.localPosition;
                transform.localPosition = transform.localPosition + ((position - transform.localPosition) * Time.deltaTime);

                if ((position - transform.localPosition).magnitude < 0.2f)
                {
                    positioned = true;
                }
                //Debug.Log((position - transform.localPosition).magnitude);
            }
        }
    }

    public void SetCurrentPlayer(bool variable)
    {
        if (!variable)
        {
            transform.SetParent(null);
        }
        else
        {
            transform.SetParent(game.GetCurrentPlayer().transform);
        }

        currentPlayer = variable;
        positioned = false;
    }

    public void SetPlayersTurn()
    {
        StopAllCoroutines();
        SetCurrentPlayer(true);
    }

    public void Locate(Vector3 position, GameObject destination)
    {
        SetCurrentPlayer(false);
        StopAllCoroutines();
        StartCoroutine(LocateCoroutine(position, destination));
    }

    private IEnumerator LocateCoroutine(Vector3 position, GameObject destination)
    {
        while ((position - transform.position).magnitude > 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((destination.transform.position - transform.position)), cameraRotation * 0.05f);
            transform.position = transform.position + ((position - transform.position) * cameraVelocity / 50f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void Rotate(Vector3 position, Vector3 rotation)
    {
        SetCurrentPlayer(false);
        StopAllCoroutines();
        StartCoroutine(RotateEulerCoroutine(position, rotation));
    }

    private IEnumerator RotateEulerCoroutine(Vector3 position, Vector3 rotation)
    {
        while ((position - transform.position).magnitude > 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), cameraRotation * 0.05f);
            transform.position = transform.position + ((position - transform.position) * cameraVelocity / 50f);
            yield return new WaitForSeconds(0.02f);
        }
    }

}
