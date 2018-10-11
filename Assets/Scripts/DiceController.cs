using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

    public IEnumerator DiceThrown()
    {
        while (!gameObject.GetComponent<Rigidbody>().IsSleeping())
        {
            yield return new WaitForSeconds(1f);
        }
        GameObject.Find("Main Camera").GetComponent<CameraController>().MoveCameraLookPlayerTurn();

        Invoke("MovePlayerTime", 2f);
    }

    private void MovePlayerTime()
    {
        GameObject.Find("Game").GetComponent<Game>().MovePlayer(10, false);
    }
}
