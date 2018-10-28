using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour {

    private int diceNumber;
    private Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void ThrowDice()
    {
        rb.useGravity = true;
        rb.AddForce(Vector3.up * UnityEngine.Random.Range(20, 35), ForceMode.Impulse);
        rb.AddTorque((Vector3.right * UnityEngine.Random.Range(-300, 300) + Vector3.up * UnityEngine.Random.Range(-300, 300) + Vector3.forward * UnityEngine.Random.Range(-300, 300)), ForceMode.Impulse);
        StartCoroutine(DiceThrown());
       // Invoke("DownForce", 2f);
    }

    public IEnumerator DiceThrown()
    {
        while (!gameObject.GetComponent<Rigidbody>().IsSleeping())
        {
            yield return new WaitForSeconds(1f);
        }
        GameObject.Find("Main Camera").GetComponent<CameraController>().MoveCameraLookPlayerTurn();

        diceNumber = WriteDiceNumber();

        GameObject.Find("Game").GetComponent<UI>().WriteTextMovements(diceNumber);

        Invoke("MovePlayerTime", 2f);
    }

    private int WriteDiceNumber()
    {
        if (CheckRotation(90, -1, -1))
        {
            if (gameObject.name.Equals("Dice")) return 2;
            else if (gameObject.name.Equals("Dice13")) return 2;
            else return 5;
        }
        else if (CheckRotation(0, -1, 90))
        {
            if (gameObject.name.Equals("Dice")) return 6;
            else if (gameObject.name.Equals("Dice13")) return 3;
            else return 6;
        }
        else if (CheckRotation(180, -1, 270))
        {
            if (gameObject.name.Equals("Dice")) return 6;
            else if (gameObject.name.Equals("Dice13")) return 3;
            else return 6;
        }
        else if (CheckRotation(270, -1, -1))
        {
            if (gameObject.name.Equals("Dice")) return 4;
            else if (gameObject.name.Equals("Dice13")) return 2;
            else return 5;
        }
        else if (CheckRotation(0, -1, 180))
        {
            if (gameObject.name.Equals("Dice")) return 3;
            else if (gameObject.name.Equals("Dice13")) return 1;
            else return 4;
        }
        else if (CheckRotation(180, -1, 0))
        {
            if (gameObject.name.Equals("Dice")) return 3;
            else if (gameObject.name.Equals("Dice13")) return 1;
            else return 4;
        }
        else if (CheckRotation(0, -1, 0))
        {
            if (gameObject.name.Equals("Dice")) return 1;
            else if (gameObject.name.Equals("Dice13")) return 1;
            else return 4;
        }
        else if (CheckRotation(180, -1, 180))
        {
            if (gameObject.name.Equals("Dice")) return 1;
            else if (gameObject.name.Equals("Dice13")) return 1;
            else return 4;
        }
        else if (CheckRotation(0, -1, 270))
        {
            if (gameObject.name.Equals("Dice")) return 5;
            else if (gameObject.name.Equals("Dice13")) return 3;
            else return 6;
        }
        else if (CheckRotation(180, -1, 90))
        {
            if (gameObject.name.Equals("Dice")) return 5;
            else if (gameObject.name.Equals("Dice13")) return 3;
            else return 6;
        }
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

    private void DownForce()
    {
        Debug.Log("ARA");
        rb.AddForce(Vector3.down * 125f, ForceMode.Impulse);
    }
}
