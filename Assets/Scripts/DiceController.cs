using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

    private int diceNumber;

    public IEnumerator DiceThrown()
    {
        while (!gameObject.GetComponent<Rigidbody>().IsSleeping())
        {
            yield return new WaitForSeconds(1f);
        }
        GameObject.Find("Main Camera").GetComponent<CameraController>().MoveCameraLookPlayerTurn();

        diceNumber = WriteDiceNumber();

        GameObject.Find("Game").GetComponent<Game>().WriteTextMovements(diceNumber);

        Invoke("MovePlayerTime", 2f);
    }

    private int WriteDiceNumber()
    {
        if (CheckRotation(90, -1, -1)) return 2;
        else if (CheckRotation(0, -1, 90)) return 6;
        else if (CheckRotation(180, -1, 270)) return 6;
        else if (CheckRotation(270, -1, -1)) return 4;
        else if (CheckRotation(0, -1, 180)) return 3;
        else if (CheckRotation(180, -1, 0)) return 3;
        else if (CheckRotation(0, -1, 0)) return 1;
        else if (CheckRotation(180, -1, 180)) return 1;
        else if (CheckRotation(0, -1, 270)) return 5;
        else if (CheckRotation(180, -1, 90)) return 5;
        else return 6;
    }

    private bool CheckRotation(int x, int y, int z)
    {
        if (y == -1 && z == -1)
        {
            if (Math.Abs(transform.eulerAngles.x - x) < 1) return true;
            else return false;
        }
        else if (x == -1)
        {
            if (Math.Abs(transform.eulerAngles.y - y) < 1 && Math.Abs(transform.eulerAngles.z - z) < 1) return true;
            else return false;
        }
        else if (y == -1)
        {
            if (Math.Abs(transform.eulerAngles.x - x) < 1 && Math.Abs(transform.eulerAngles.z - z) < 1) return true;
            else return false;
        }
        else return false;
    }

    private void MovePlayerTime()
    {
        GameObject.Find("Game").GetComponent<Game>().DestroyDiceStructure();
        GameObject.Find("Game").GetComponent<Game>().MovePlayer(diceNumber, false);
    }
}
