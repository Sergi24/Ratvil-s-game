using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DiceType
{
    Dice16,
    Dice13,
    Dice46
}

public class Game : MonoBehaviour
{

    public GameObject[] players;
    public GameObject[] cameraPlayerPosition;

    public GameObject optionChooseDirection;
    public float playersVelocity, playersRotation;
    public GameObject gameCamera, spotlight;

    public GameObject diceStructure, diceStructure13, diceStructure46;
    public GameObject bigTrap, bigTrapSprite, hammer, hammerSprite;

    public Color colorBlueSpotlight, colorRedSpotlight;
    public Material materialSoftGrey, materialPink;

    private GameObject currentDiceStructure, currentDice, lastSpotlightNextSlot;
    private int currentItemSelected;
    private DiceType currentDiceStructureType;

    private int numPlayers;
    private int playerTurn;

    private int probDice13 = 15,
                probDice46 = 15,
                probTrap = 55,
                probBigTrap = 10,
                probChangeDirection = 4,
                probChoosePosition = 1;

    private int remainingMovements;
    private int newPosition;
    private int newDirection;

    private bool mapGeneralView;

    private int nextSlotVisualization, nextSlotVisualizationDirection, nextSlotMovements;

    private UI ui;

    // Use this for initialization
    void Start()
    {
        Map.InitializeSquares();
        mapGeneralView = false;
        Players.InitializatePlayers();
        playerTurn = 0;
        PrepareDiceThrow();
        currentItemSelected = -1;
        numPlayers = 2;

        ui = gameObject.GetComponent<UI>();
    }

    public IEnumerator ChangeTurn()
    {
        ui.ShowOptionThrow(false);
        ui.ShowOptionItems(false);
        yield return new WaitForSeconds(1f);
        gameCamera.GetComponent<CameraController>().MoveCameraRotation(GameObject.Find("MapGeneralView").transform.position, new Vector3(87, 291, -68));
        yield return new WaitForSeconds(1f);
        playerTurn = (playerTurn + 1) % numPlayers;

        PrepareDiceThrow();

        gameCamera.GetComponent<CameraController>().MoveCameraLookPlayerTurn();

        if (playerTurn != 0) StartCoroutine(AIturn());
        else
        {
            ui.ShowOptionThrow(true);
            ui.ShowOptionItems(true);
        }
    }

    public IEnumerator AIturn()
    {
        yield return new WaitForSeconds(3f);

        if (AIchooseDice()) yield return new WaitForSeconds(1f);

        ButtonThrowDice();
    }

    public bool AIchooseDice()
    {
        if (Random.Range(0, 2) == 0)
        {
            Item[] items = Players.GetItems(playerTurn);
            int randNum = Random.Range(0, 3);
            for (int i = randNum; i < randNum + 3; i++)
            {
                if (items[i % 3].Equals(Item.Dice13) || items[i % 3].Equals(Item.Dice46))
                {
                    ButtonItem(i % 3);
                    return true;
                }
            }
        }
        return false;
    }

    public void AIchooseDirection()
    {
        if (Random.Range(0, 2) == 0) ButtonLeftDirection();
        else ButtonRightDirection();
    }

    public void AIchooseItem()
    {
        if (Random.Range(0, 2) == 0)
        {
            Item[] items = Players.GetItems(playerTurn);
            int randNum = Random.Range(0, 3);
            for (int i = randNum; i < randNum + 3; i++)
            {
                if (!items[i % 3].Equals(Item.Empty) && !items[i % 3].Equals(Item.Dice13) && !items[i % 3].Equals(Item.Dice46))
                {
                    ButtonItem(i % 3);
                    break;
                }
            }
        }
    }

    public int GetPlayerTurn()
    {
        return playerTurn;
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
        Players.SetCurrentSlot(playerTurn, Map.leftSquare + (Map.leftDirection * -1));
        Players.SetCurrentDirection(playerTurn, Map.leftDirection);
        MovePlayer(0, true);
    }

    public void ButtonRightDirection()
    {
        optionChooseDirection.SetActive(false);
        RemoveAllSpotlights();
        Players.SetCurrentSlot(playerTurn, Map.rightSquare + (Map.rightDirection * -1));
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
        DestroyDiceStructure();
        CreateDiceStructure(DiceType.Dice16);
        nextSlotVisualization = Players.GetCurrentSquare(playerTurn);
        nextSlotVisualizationDirection = Players.GetCurrentDirection(playerTurn);
        nextSlotMovements = 0;
    }

    public void CreateDiceStructure(DiceType diceType)
    {
        currentDiceStructureType = diceType;
        GameObject structure;
        if (diceType.Equals(DiceType.Dice16)) structure = diceStructure;
        else if (diceType.Equals(DiceType.Dice13)) structure = diceStructure13;
        else structure = diceStructure46;
        currentDiceStructure = Instantiate(structure, GetCurrentPlayer().transform.position + (Vector3.up * 6), GetCurrentPlayer().transform.rotation);

        if (diceType.Equals(DiceType.Dice16)) currentDice = currentDiceStructure.transform.Find("Dice").gameObject;
        else if (diceType.Equals(DiceType.Dice13)) currentDice = currentDiceStructure.transform.Find("Dice13").gameObject;
        else currentDice = currentDiceStructure.transform.Find("Dice46").gameObject;
    }

    public void DestroyDiceStructure()
    {
        if (currentDiceStructure != null) Destroy(currentDiceStructure);
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
        float rotation = playersRotation * 0.05f;
        Vector3 translation = Vector3.forward * playersVelocity;
        while (i < numSlots)
        {
            ui.textMovements.SetActive(true);
            ui.textMovements.GetComponent<Text>().text = (numSlots - i).ToString();
            if (thereAreRemainingMovements) thereAreRemainingMovements = false;
            else if (Map.IsNeedToChooseDirection(Players.GetCurrentSquare(numPlayer), Players.GetCurrentDirection(numPlayer)))
            {
                remainingMovements = numSlots - i;
                if (numPlayer == 0)
                {
                    optionChooseDirection.SetActive(true);
                    SpotlightSlots(Players.GetCurrentSquare(numPlayer), Players.GetCurrentDirection(numPlayer));
                    gameCamera.GetComponent<CameraController>().MoveCameraLookDestination(new Vector3(player.transform.position.x, player.transform.position.y + 120, player.transform.position.z), player.transform.Find("GeneralView").gameObject);
                    Map.IsNeedToChooseDirection(Players.GetCurrentSquare(numPlayer), Players.GetCurrentDirection(numPlayer));
                }
                else AIchooseDirection();
                break;
            }
            Vector3 nextSlotPosition = Map.GetSquarePosition(Players.GetNextSquare(numPlayer));
            while ((player.transform.position - new Vector3(nextSlotPosition.x, player.transform.position.y, nextSlotPosition.z)).magnitude > 2)
            {
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(new Vector3(nextSlotPosition.x, player.transform.position.y, nextSlotPosition.z) - player.transform.position), rotation);
                player.transform.Translate(translation);
                yield return new WaitForSeconds(0.015f);
            }
            Players.SetCurrentSlot(numPlayer, Players.GetNextSquare(numPlayer));
            i++;
            if (i == numSlots)
            {
                ui.textMovements.SetActive(true);
                ui.textMovements.GetComponent<Text>().text = "0";
                if (Map.GetSquareType(Players.GetCurrentSquare(playerTurn)).Equals(SquareType.Trap))
                {
                    yield return StartCoroutine(TrapController(Players.GetCurrentSquare(playerTurn)));
                }
                else if (Map.GetSquareType(Players.GetCurrentSquare(playerTurn)).Equals(SquareType.BigTrap))
                {
                    yield return StartCoroutine(BigTrapController(Players.GetCurrentSquare(playerTurn)));
                }
                if (playerTurn != 0)
                {
                    yield return new WaitForSeconds(1f);
                    AIchooseItem();
                }
                StartCoroutine(ChangeTurn());
            }
        }
        GetCurrentPlayer().GetComponent<PlayerAnimator>().Stop();
    }

    public void SpotlightSlots(int slot, int direction)
    {
        List<int> remainingSlot = new List<int>();
        List<int> remainingDirection = new List<int>();
        List<int> movements = new List<int>();

        remainingSlot.Add(Map.rightSquare);
        remainingDirection.Add(Map.rightDirection);
        movements.Add(remainingMovements);

        remainingSlot.Add(Map.leftSquare);
        remainingDirection.Add(Map.leftDirection);
        movements.Add(remainingMovements);

        Color color = colorRedSpotlight;

        int moves;
        while (remainingSlot.Count > 0)
        {
            if (remainingSlot.Count == 1) color = colorBlueSpotlight;
            moves = movements[0];
            while (moves > 0)
            {
                Vector3 position = Map.GetSquarePosition(remainingSlot[0]);
                GameObject spotlightInstantiated = Instantiate(spotlight, new Vector3(position.x, position.y + 12f, position.z), spotlight.transform.rotation);
                spotlightInstantiated.GetComponent<Light>().color = color;
                if (Map.IsNeedToChooseDirection(remainingSlot[0], remainingDirection[0]))
                {
                    remainingSlot.Insert(1, Map.leftSquare);
                    remainingDirection.Insert(1, Map.leftDirection);
                    movements.Insert(1, moves - 1);

                    remainingSlot[0] = Map.rightSquare;
                    remainingDirection[0] = Map.rightDirection;
                }
                else
                {
                    remainingSlot[0] = Map.GetNextSquare(remainingSlot[0], remainingDirection[0]);
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
        foreach (GameObject spotlight in GameObject.FindGameObjectsWithTag("Spotlight"))
        {
            Destroy(spotlight);
        }
    }

    public void ButtonThrowDice()
    {
        if (currentItemSelected != -1)
        {
            Players.EliminateItem(playerTurn, currentItemSelected);
            currentItemSelected = -1;
        }

        currentDice.GetComponent<DiceController>().ThrowDice();

        ui.ShowOptionThrow(false);
        ui.ShowTextMovements(false);

        gameCamera.GetComponent<CameraController>().MoveCameraLookDestination(new Vector3(currentDice.transform.position.x, currentDice.transform.position.y + 30, currentDice.transform.position.z), currentDice);

        if (GetCurrentPlayer().GetComponent<PlayerAnimator>() != null)
        {
            GetCurrentPlayer().GetComponent<PlayerAnimator>().Run();
        }
    }

    public void ButtonNextSlot(int direction)
    {
        if (Map.IsNeedToChooseDirection(nextSlotVisualization, nextSlotVisualizationDirection * direction))
        {

        }
        else
        {
            nextSlotMovements += direction;
            ui.textMovements.SetActive(true);
            ui.textMovements.GetComponent<Text>().text = nextSlotMovements.ToString();
            nextSlotVisualization = Map.GetNextSquare(nextSlotVisualization, nextSlotVisualizationDirection * direction);
            Vector3 nextSlotVisualizationPosition = Map.GetSquarePosition(nextSlotVisualization);
            gameCamera.GetComponent<CameraController>().MoveCameraRotation(new Vector3(nextSlotVisualizationPosition.x, nextSlotVisualizationPosition.y + 200, nextSlotVisualizationPosition.z), new Vector3(90, 0, 0));

            if (lastSpotlightNextSlot != null)
            {
                Destroy(lastSpotlightNextSlot);
            }
            lastSpotlightNextSlot = Instantiate(spotlight, new Vector3(nextSlotVisualizationPosition.x, nextSlotVisualizationPosition.y + 12f, nextSlotVisualizationPosition.z), spotlight.transform.rotation);
        }
    }

    public void AddItem()
    {
        Players.AddItem(playerTurn, GetRandomItem());
    }

    public void ButtonItem(int numItem)
    {
        if (Players.GetItem(playerTurn, numItem).Equals(Item.Dice13))
        {
            DestroyDiceStructure();
            if (currentDiceStructureType.Equals(DiceType.Dice13))
            {
                CreateDiceStructure(DiceType.Dice16);
                currentItemSelected = -1;
            }
            else
            {
                CreateDiceStructure(DiceType.Dice13);
                currentItemSelected = numItem;
            }
        }
        else if (Players.GetItem(playerTurn, numItem).Equals(Item.Dice46))
        {
            DestroyDiceStructure();
            if (currentDiceStructureType.Equals(DiceType.Dice46))
            {
                CreateDiceStructure(DiceType.Dice16);
                currentItemSelected = -1;
            }
            else
            {
                CreateDiceStructure(DiceType.Dice46);
                currentItemSelected = numItem;
            }
        }
        else if (Players.GetItem(playerTurn, numItem).Equals(Item.Trap))
        {
            int currentSquareNum = Players.GetCurrentSquare(playerTurn);
            if (Map.GetSquareType(currentSquareNum).Equals(SquareType.Empty))
            {
                Instantiate(hammerSprite, Map.GetSquarePosition(currentSquareNum), Map.GetSquare(currentSquareNum).transform.rotation).transform.SetParent(Map.GetSquare(currentSquareNum).transform);
                Instantiate(hammer, GetCurrentPlayer().transform.position + new Vector3(0, -2, 0), GetCurrentPlayer().transform.rotation).transform.SetParent(Map.GetSquare(currentSquareNum).transform);
                Map.AddItemToSquare(currentSquareNum, Item.Trap);
                Map.GetSquare(currentSquareNum).GetComponent<MeshRenderer>().material.color = materialSoftGrey.color;

                Players.EliminateItem(playerTurn, numItem);
            }
        }
        else if (Players.GetItem(playerTurn, numItem).Equals(Item.BigTrap))
        {
            int currentSquareNum = Players.GetCurrentSquare(playerTurn);
            if (Map.GetSquareType(currentSquareNum).Equals(SquareType.Empty))
            {
                Instantiate(bigTrapSprite, Map.GetSquarePosition(currentSquareNum), Map.GetSquare(currentSquareNum).transform.rotation).transform.SetParent(Map.GetSquare(currentSquareNum).transform);
                Instantiate(bigTrap, Map.GetSquarePosition(currentSquareNum), GetCurrentPlayer().transform.rotation).transform.SetParent(Map.GetSquare(currentSquareNum).transform);
                Map.AddItemToSquare(currentSquareNum, Item.BigTrap);
                Map.GetSquare(currentSquareNum).GetComponent<MeshRenderer>().material.color = materialSoftGrey.color;

                Players.EliminateItem(playerTurn, numItem);
            }
        }
        else if (Players.GetItem(playerTurn, numItem).Equals(Item.ChangeDirection))
        {
            //StartCoroutine();
        }
    }

    public Item GetRandomItem()
    {
        int numRandom = Random.Range(0, 100);
        if (numRandom < probDice13) return Item.Dice13;
        else if (numRandom < probDice13 + probDice46) return Item.Dice46;
        else if (numRandom < probDice13 + probDice46 + probTrap) return Item.Trap;
        else if (numRandom < probDice13 + probDice46 + probTrap + probBigTrap) return Item.BigTrap;
        else if (numRandom < probDice13 + probDice46 + probTrap + probBigTrap + probChangeDirection) return Item.ChangeDirection;
        else return Item.ChoosePosition;
    }

    private IEnumerator TrapController(int numSquare)
    {
        yield return new WaitForSeconds(1f);
        GameObject trapInstantiated = Map.GetSquare(numSquare).transform.GetChild(1).GetChild(0).gameObject;
        while (trapInstantiated.transform.rotation.eulerAngles.z < 90)
        {
            trapInstantiated.transform.Rotate(Vector3.forward * 5);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.5f);
        Map.SetEmptySquare(numSquare);
        Map.GetSquare(numSquare).GetComponent<MeshRenderer>().material = materialPink;
        Destroy(Map.GetSquare(numSquare).transform.GetChild(1).gameObject);
        Destroy(Map.GetSquare(numSquare).transform.GetChild(0).gameObject);
    }

    private IEnumerator BigTrapController(int numSquare)
    {
        foreach (ParticleSystem ps in Map.GetSquare(Players.GetCurrentSquare(playerTurn)).transform.GetChild(1).gameObject.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }

        yield return new WaitForSeconds(3f);

        Map.SetEmptySquare(numSquare);
        Map.GetSquare(numSquare).GetComponent<MeshRenderer>().material = materialPink;

        yield return new WaitForSeconds(1f);

        GameObject bigTrapInstantiated = Map.GetSquare(numSquare).transform.GetChild(1).gameObject;
        float yDown = bigTrapInstantiated.transform.position.y - 110;
        while (bigTrapInstantiated.transform.position.y > yDown)
        {
            bigTrapInstantiated.transform.Translate(Vector3.down * 0.15f);
            yield return new WaitForSeconds(0.02f);
        }

        Destroy(bigTrapInstantiated);
        Destroy(Map.GetSquare(numSquare).transform.GetChild(1).gameObject);
    }
}
