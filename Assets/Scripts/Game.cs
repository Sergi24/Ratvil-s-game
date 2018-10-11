using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public GameObject[] players;
    public GameObject[] cameraPlayerPosition;

    public GameObject optionChooseDirection;
    public float playersVelocity, playersRotation;
    public GameObject gameCamera;

    public GameObject optionThrow;

    private int playerTurn;

    private int remainingMovements;
    private int newPosition;
    private int newDirection;

    private bool mapGeneralView;

    private int nextSlotVisualization, nextSlotVisualizationDirection;

    // Use this for initialization
    void Start ()
    {
        Map.InitializeSlots();
        mapGeneralView = false;
        Players.SetPlayersInitialPosition();
        playerTurn = 0;
        nextSlotVisualization = Players.GetCurrentSlot(playerTurn);
        nextSlotVisualizationDirection = Players.GetCurrentDirection(playerTurn);
    }

    public GameObject GetCurrentPlayer()
    {
        return players[playerTurn];
    }

    public GameObject GetCameraPlayerPosition()
    {
        return cameraPlayerPosition[playerTurn];
    }

    public void ButtonLeftDirection()
    {
        optionChooseDirection.SetActive(false);
        Players.SetCurrentSlot(playerTurn, Map.leftSlot + (Map.leftDirection * -1));
        Players.SetCurrentDirection(playerTurn, Map.leftDirection);
        MovePlayer(0, true);
    }

    public void ButtonRightDirection()
    {
        optionChooseDirection.SetActive(false);
        Players.SetCurrentSlot(playerTurn, Map.rightSlot + (Map.rightDirection*-1));
        Players.SetCurrentDirection(playerTurn, Map.rightDirection);
        MovePlayer(0, true);
    }

    public void ButtonMapGeneralView()
    {
        if (mapGeneralView)
        {
            gameCamera.GetComponent<CameraController>().MoveCameraLookPlayerTurn();
            mapGeneralView = false;
        }
        else
        {
            mapGeneralView = true;
            gameCamera.GetComponent<CameraController>().MoveCameraRotation(GameObject.Find("MapGeneralView").transform.position, new Vector3(87, 291, -68));
        }
    }

    public void MovePlayer(int movements, bool thereAreRemainingMovements)
    {
        gameCamera.GetComponent<CameraController>().MoveCameraLookPlayerTurn();
        StopAllCoroutines();
        StartCoroutine(MovePlayerCoroutine(playerTurn, movements, thereAreRemainingMovements));
    }

    private IEnumerator MovePlayerCoroutine(int numPlayer, int numSlots, bool thereAreRemainingMovements)
    {
        GameObject player = players[numPlayer];
        if (thereAreRemainingMovements)
        {
            numSlots = remainingMovements;
        }

        int i = 0;
        while (i < numSlots)
        {
            if (thereAreRemainingMovements) thereAreRemainingMovements = false;
            else if (Map.IsNeedToChooseDirection(Players.GetCurrentSlot(numPlayer), Players.GetCurrentDirection(numPlayer)))
            {
                optionChooseDirection.SetActive(true);

                gameCamera.GetComponent<CameraController>().MoveCameraLookDestination(new Vector3(player.transform.position.x, player.transform.position.y + 120, player.transform.position.z), player.transform.Find("GeneralView").gameObject);
                remainingMovements = numSlots - i;
                break;
            }
            Vector3 nextSlotPosition = Map.GetSlotPosition(Players.GetNextSlot(numPlayer));
            while ((player.transform.position - new Vector3(nextSlotPosition.x, player.transform.position.y, nextSlotPosition.z)).magnitude > 2)
            {
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(new Vector3(nextSlotPosition.x, player.transform.position.y, nextSlotPosition.z) - player.transform.position), playersRotation * Time.deltaTime);
                player.transform.Translate(Vector3.forward * 20 * playersVelocity * Time.deltaTime);
                yield return new WaitForSeconds(0.02f);
            }

            Players.SetCurrentSlot(numPlayer, Players.GetNextSlot(numPlayer));
            i++;
        }
    }


    public void ButtonThrowDice()
    {
        GameObject dice = GameObject.Find("Dice");
        dice.GetComponent<Rigidbody>().useGravity = true;
        dice.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(20, 35), ForceMode.Impulse);
        dice.GetComponent<Rigidbody>().AddTorque((Vector3.right * Random.Range(-300, 300) + Vector3.up * Random.Range(-300, 300) + Vector3.forward * Random.Range(-300, 300)), ForceMode.Impulse);
        optionThrow.SetActive(false);

        gameCamera.GetComponent<CameraController>().MoveCameraLookDestination(new Vector3(dice.transform.position.x, dice.transform.position.y + 20, dice.transform.position.z), dice);
        StartCoroutine(dice.GetComponent<DiceController>().DiceThrown());
    }

    public void ButtonNextSlot(int direction)
    {
        if (Map.IsNeedToChooseDirection(nextSlotVisualization, nextSlotVisualizationDirection * direction))
        {

        }
        else
        {
            nextSlotVisualization = Map.GetNextSlot(nextSlotVisualization, nextSlotVisualizationDirection * direction);
            Vector3 nextSlotVisualizationPosition = Map.GetSlotPosition(nextSlotVisualization);
            gameCamera.GetComponent<CameraController>().MoveCameraRotation(new Vector3(nextSlotVisualizationPosition.x, nextSlotVisualizationPosition.y + 100, nextSlotVisualizationPosition.z), new Vector3(90, 0, 0));
        }
    }
}
