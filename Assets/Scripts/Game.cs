using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public GameObject[] players;
    public GameObject[] cameraPlayerPosition;

    public GameObject optionChooseDirection;
    public float playersVelocity, playersRotation;
    public GameObject gameCamera, spotlight;

    public GameObject optionThrow;
    public GameObject diceStructure;

    public Color colorBlueSpotlight, colorRedSpotlight;

    public Text textMovements;

    private GameObject currentDiceStructure;

    private int playerTurn;

    private int remainingMovements;
    private int newPosition;
    private int newDirection;

    private bool mapGeneralView;

    private int nextSlotVisualization, nextSlotVisualizationDirection, nextSlotMovements;

    // Use this for initialization
    void Start ()
    {
        Map.InitializeSlots();
        mapGeneralView = false;
        Players.InitializatePlayers();
        playerTurn = 0;
        PrepareDiceThrow();
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
        RemoveAllSpotlights();
        Players.SetCurrentSlot(playerTurn, Map.leftSlot + (Map.leftDirection * -1));
        Players.SetCurrentDirection(playerTurn, Map.leftDirection);
        MovePlayer(0, true);
    }

    public void ButtonRightDirection()
    {
        optionChooseDirection.SetActive(false);
        RemoveAllSpotlights();
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

    public void PrepareDiceThrow()
    {
        optionThrow.SetActive(true);
        currentDiceStructure = Instantiate(diceStructure, GetCurrentPlayer().transform.position + (Vector3.up * 6), diceStructure.transform.rotation);
        nextSlotVisualization = Players.GetCurrentSlot(playerTurn);
        nextSlotVisualizationDirection = Players.GetCurrentDirection(playerTurn);
        nextSlotMovements = 0;
    }

    public void DestroyDiceStructure()
    {
        Destroy(currentDiceStructure);
    }

    public void ResetTextMovements()
    {
        textMovements.text = "";
    }

    public void WriteTextMovements(int movements)
    {
        textMovements.text = movements.ToString();
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
            textMovements.text = (numSlots - i).ToString();
            if (thereAreRemainingMovements) thereAreRemainingMovements = false;
            else if (Map.IsNeedToChooseDirection(Players.GetCurrentSlot(numPlayer), Players.GetCurrentDirection(numPlayer)))
            {
                optionChooseDirection.SetActive(true);
                remainingMovements = numSlots - i;
                SpotlightSlots(Players.GetCurrentSlot(numPlayer), Players.GetCurrentDirection(numPlayer));
                gameCamera.GetComponent<CameraController>().MoveCameraLookDestination(new Vector3(player.transform.position.x, player.transform.position.y + 120, player.transform.position.z), player.transform.Find("GeneralView").gameObject);
                Map.IsNeedToChooseDirection(Players.GetCurrentSlot(numPlayer), Players.GetCurrentDirection(numPlayer));
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
            if (i == numSlots)
            {
                textMovements.text = "0";
                PrepareDiceThrow();
            }
        }
    }

    public void SpotlightSlots(int slot, int direction)
    {
        List<int> remainingSlot = new List<int>();
        List<int> remainingDirection = new List<int>();
        List<int> movements = new List<int>();

        remainingSlot.Add(Map.rightSlot);
        remainingDirection.Add(Map.rightDirection);
        movements.Add(remainingMovements);

        remainingSlot.Add(Map.leftSlot);
        remainingDirection.Add(Map.leftDirection);
        movements.Add(remainingMovements);

        Color color = colorRedSpotlight;

        int moves;
        while (remainingSlot.Count > 0)
        {
            if (remainingSlot.Count == 1) color = colorBlueSpotlight;
            moves = movements[0];
            while (moves > 0) {
                Vector3 position = Map.GetSlotPosition(remainingSlot[0]);
                GameObject spotlightInstantiated = Instantiate(spotlight, new Vector3(position.x, position.y + 12f, position.z), spotlight.transform.rotation);
                spotlightInstantiated.GetComponent<Light>().color = color;
                if (Map.IsNeedToChooseDirection(remainingSlot[0], remainingDirection[0]))
                {
                    remainingSlot.Insert(1, Map.leftSlot);
                    remainingDirection.Insert(1, Map.leftDirection);
                    movements.Insert(1, moves - 1);

                    remainingSlot[0] = Map.rightSlot;
                    remainingDirection[0] = Map.rightDirection;
                }
                else
                {
                    remainingSlot[0] = Map.GetNextSlot(remainingSlot[0], remainingDirection[0]);
                }

                moves--;
            }
            remainingSlot.RemoveAt(0);
            remainingDirection.RemoveAt(0);
            movements.RemoveAt(0);
        }
    }

    public void RemoveAllSpotlights()
    {
        foreach(GameObject spotlight in GameObject.FindGameObjectsWithTag("Spotlight"))
        {
            Destroy(spotlight);
        }
    }

    public void ButtonThrowDice()
    {
        GameObject dice = GameObject.Find("Dice");
        dice.GetComponent<Rigidbody>().useGravity = true;
        dice.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(20, 35), ForceMode.Impulse);
        dice.GetComponent<Rigidbody>().AddTorque((Vector3.right * Random.Range(-300, 300) + Vector3.up * Random.Range(-300, 300) + Vector3.forward * Random.Range(-300, 300)), ForceMode.Impulse);
        optionThrow.SetActive(false);

        ResetTextMovements();

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
            nextSlotMovements += direction;
            textMovements.text = nextSlotMovements.ToString();
            nextSlotVisualization = Map.GetNextSlot(nextSlotVisualization, nextSlotVisualizationDirection * direction);
            Vector3 nextSlotVisualizationPosition = Map.GetSlotPosition(nextSlotVisualization);
            gameCamera.GetComponent<CameraController>().MoveCameraRotation(new Vector3(nextSlotVisualizationPosition.x, nextSlotVisualizationPosition.y + 100, nextSlotVisualizationPosition.z), new Vector3(90, 0, 0));
        }
    }
}
