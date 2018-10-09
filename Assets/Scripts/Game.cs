using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public GameObject[] players;
    public GameObject optionChooseDirection;
    public float playersVelocity, playersRotation;

    private int playerTurn;

    private int remainingMovements;
    private int newPosition;
    private int newDirection;

    // Use this for initialization
    void Start ()
    {
        Map.InitializeSlots();
        Players.SetPlayersInitialPosition();
        playerTurn = 0;
        StartCoroutine(MovePlayer(playerTurn, 7, false));
    }

    public void ButtonLeftDirection()
    {
        optionChooseDirection.SetActive(false);
        Players.SetCurrentSlot(playerTurn, Map.leftSlot + (Map.leftDirection * -1));
        Players.SetCurrentDirection(playerTurn, Map.leftDirection);
        StartCoroutine(MovePlayer(playerTurn, 0, true));
    }

    public void ButtonRightDirection()
    {
        optionChooseDirection.SetActive(false);
        Players.SetCurrentSlot(playerTurn, Map.rightSlot + (Map.rightDirection*-1));
        Players.SetCurrentDirection(playerTurn, Map.rightDirection);
        StartCoroutine(MovePlayer(playerTurn, 0, true));
    }

    private IEnumerator MovePlayer(int numPlayer, int numSlots, bool thereAreRemainingMovements)
    {
        GameObject player = players[numPlayer];
        int i = 0;
        if (thereAreRemainingMovements)
        {
            numSlots = remainingMovements;
        }
        while (i < numSlots)
        {
            if (thereAreRemainingMovements) thereAreRemainingMovements = false;
            else if (Map.IsNeedToChooseDirection(Players.GetCurrentSlot(numPlayer), Players.GetCurrentDirection(numPlayer)))
            {
                Debug.Log("CHOOSE DIRECTION");
                optionChooseDirection.SetActive(true);
                remainingMovements = numSlots - i;
                break;
            }
            Vector3 nextSlotPosition = Map.GetSlotPosition(Players.GetNextSlot(numPlayer));
            Debug.Log(nextSlotPosition);
            while ((player.transform.position - new Vector3(nextSlotPosition.x, player.transform.position.y, nextSlotPosition.z)).magnitude > 2)
            {
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(new Vector3(nextSlotPosition.x, player.transform.position.y, nextSlotPosition.z) - player.transform.position), playersRotation * Time.deltaTime);
                Debug.DrawLine(player.transform.position, new Vector3(nextSlotPosition.x, player.transform.position.y, nextSlotPosition.z));
                player.transform.Translate(Vector3.forward * 50 * playersVelocity * Time.deltaTime);
                yield return new WaitForSeconds(0.05f);
            }

            Players.SetCurrentSlot(numPlayer, Players.GetNextSlot(numPlayer));
            i++;
        }
    }
}
