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
    public GameObject[] PlayersPositionCamera;

    public GameObject ChooseDirectionOption;
    public float playersVelocity, playersRotation;
    public GameObject GameCamera, spotlight;

    public GameObject RollOption;

    public GameObject DiceSides, OneToThreeSidedDice, FourToSixSidedDice;
    public GameObject BigTrap, BigTrapSprite, Hammer, HammerSprite;

    public Color blueSpotlight, redSpotlight;
    public Material SoftGreyMaterial, PinkMaterial;

    public GameObject TextMovements;

    private GameObject CurrentDiceSides, CurrentDice, LastSpotlightsNextSlot;
    private int currentItemSelected;
    private DiceType CurrentDiceType;

    private int playersTurn;
    private readonly int dice13Chance = 15,
                dice46Chance = 15,
                trapChance = 55,
                bigTrapChance = 10,
                directionChangeChance = 4,
                positionChoiceChance = 1;

    private int MovementsLeft;
    private readonly int NewPosition;
    private readonly int NewDirection;

    private int nextSlotVisualization, nextSlotVisualizationDirection, nextSlotMovements;

    private bool mapView;

    // MAP VIEW
    public void ToggleMap()
    {
        if (GameCamera != null)
        {
            if (mapView)
            {
                GameCamera.GetComponent<CameraController>().SetPlayersTurn();
                mapView = false;
            }
            else
            {
                GameCamera.GetComponent<CameraController>().Rotate(GameObject.Find("MapView").transform.position, new Vector3(87, 291, -68));
            }
            mapView = !mapView;
        }
    }

    private void Start()
    {
        Map.InitializeSquares();
        mapView = false;
        Players.InitializePlayers();
        playersTurn = 0;
        SetupDiceRoll();
        currentItemSelected = -1;
    }

    public int GetPlayersTurn()
    {
        return playersTurn;
    }

    public GameObject GetCurrentPlayer()
    {
        return players[playersTurn];
    }

    public GameObject GetPlayersPositionCamera()
    {
        return PlayersPositionCamera[playersTurn];
    }

    public void ButtonLeftDirection()
    {
        ChooseDirectionOption.SetActive(false);
        RemoveAllSpotlights();
        Players.SetCurrentSlot(playersTurn, Map.leftSquare + (Map.leftDirection * -1));
        Players.SetCurrentDirection(playersTurn, Map.leftDirection);
        MovePlayer(0, true);
    }

    public void ButtonRightDirection()
    {
        ChooseDirectionOption.SetActive(false);
        RemoveAllSpotlights();
        Players.SetCurrentSlot(playersTurn, Map.rightSquare + (Map.rightDirection * -1));
        Players.SetCurrentDirection(playersTurn, Map.rightDirection);
        MovePlayer(0, true);
    }

    public void SetupDiceRoll()
    {
        RollOption.SetActive(true);
        KillDiceSides();
        CreateDiceSides(DiceType.Dice16);
        nextSlotVisualization = Players.GetCurrentSquare(playersTurn);
        nextSlotVisualizationDirection = Players.GetCurrentDirection(playersTurn);
        nextSlotMovements = 0;
    }

    public void CreateDiceSides(DiceType diceType)
    {
        CurrentDiceType = diceType;
        GameObject sides;
        if (diceType.Equals(DiceType.Dice16))
        {
            sides = DiceSides;
        }
        else if (diceType.Equals(DiceType.Dice13))
        {
            sides = OneToThreeSidedDice;
        }
        else
        {
            sides = FourToSixSidedDice;
        }

        CurrentDiceSides = Instantiate(sides, GetCurrentPlayer().transform.position + (Vector3.up * 6), GetCurrentPlayer().transform.rotation);

        if (diceType.Equals(DiceType.Dice16))
        {
            CurrentDice = CurrentDiceSides.transform.Find("Dice").gameObject;
        }
        else if (diceType.Equals(DiceType.Dice13))
        {
            CurrentDice = CurrentDiceSides.transform.Find("Dice13").gameObject;
        }
        else
        {
            CurrentDice = CurrentDiceSides.transform.Find("Dice46").gameObject;
        }
    }

    public void KillDiceSides()
    {
        if (CurrentDiceSides != null)
        {
            Destroy(CurrentDiceSides);
        }
    }

    public void HideTextMovements()
    {
        TextMovements.SetActive(false);
    }

    public void WriteTextMovements(int movements)
    {
        TextMovements.SetActive(true);
        TextMovements.GetComponent<Text>().text = movements.ToString();
    }

    public void MovePlayer(int movements, bool movementsLeft)
    {
        GameCamera.GetComponent<CameraController>().SetPlayersTurn();
        StopAllCoroutines();
        StartCoroutine(MovePlayerCoroutine(playersTurn, movements, movementsLeft));
    }

    private IEnumerator MovePlayerCoroutine(int playerNum, int slotsNum, bool movementsLeft)
    {
        GameObject player = players[playerNum];
        if (movementsLeft)
        {
            slotsNum = MovementsLeft;
        }

        int i = 0;
        float rotation = playersRotation * 0.05f;
        Vector3 translation = Vector3.forward * playersVelocity;
        while (i < slotsNum)
        {
            TextMovements.SetActive(true);
            TextMovements.GetComponent<Text>().text = (slotsNum - i).ToString();
            if (movementsLeft)
            {
                movementsLeft = false;
            }
            else if (Map.mustChooseDirection(Players.GetCurrentSquare(playerNum), Players.GetCurrentDirection(playerNum)))
            {
                ChooseDirectionOption.SetActive(true);
                MovementsLeft = slotsNum - i;
                SpotlightSlots(Players.GetCurrentSquare(playerNum), Players.GetCurrentDirection(playerNum));
                GameCamera.GetComponent<CameraController>().Locate(new Vector3(player.transform.position.x, player.transform.position.y + 120, player.transform.position.z), player.transform.Find("GeneralView").gameObject);
                Map.mustChooseDirection(Players.GetCurrentSquare(playerNum), Players.GetCurrentDirection(playerNum));
                break;
            }
            Vector3 nextSlotPosition = Map.GetSquarePosition(Players.GetNextSquare(playerNum));
            while ((player.transform.position - new Vector3(nextSlotPosition.x, player.transform.position.y, nextSlotPosition.z)).magnitude > 2)
            {
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(new Vector3(nextSlotPosition.x, player.transform.position.y, nextSlotPosition.z) - player.transform.position), rotation);
                player.transform.Translate(translation);
                yield return new WaitForSeconds(0.015f);
            }
            Players.SetCurrentSlot(playerNum, Players.GetNextSquare(playerNum));
            i++;
            if (i == slotsNum)
            {
                TextMovements.SetActive(true);
                TextMovements.GetComponent<Text>().text = "0";
                SetupDiceRoll();
                if (Map.GetSquareType(Players.GetCurrentSquare(playersTurn)).Equals(SquareType.Trap))
                {
                    StartCoroutine(TrapController(Players.GetCurrentSquare(playersTurn)));
                }
                else if (Map.GetSquareType(Players.GetCurrentSquare(playersTurn)).Equals(SquareType.BigTrap))
                {
                    StartCoroutine(BigTrapController(Players.GetCurrentSquare(playersTurn)));
                }
            }
        }
    }

    public void SpotlightSlots(int slot, int direction)
    {
        List<int> slotsLeft = new List<int>();
        List<int> directionsLeft = new List<int>();
        List<int> movements = new List<int>();

        slotsLeft.Add(Map.rightSquare);
        directionsLeft.Add(Map.rightDirection);
        movements.Add(MovementsLeft);

        slotsLeft.Add(Map.leftSquare);
        directionsLeft.Add(Map.leftDirection);
        movements.Add(MovementsLeft);

        Color color = redSpotlight;

        int moves;
        while (slotsLeft.Count > 0)
        {
            if (slotsLeft.Count == 1)
            {
                color = blueSpotlight;
            }

            moves = movements[0];
            while (moves > 0)
            {
                Vector3 position = Map.GetSquarePosition(slotsLeft[0]);
                GameObject spotlightInstantiated = Instantiate(spotlight, new Vector3(position.x, position.y + 12f, position.z), spotlight.transform.rotation);
                spotlightInstantiated.GetComponent<Light>().color = color;
                if (Map.mustChooseDirection(slotsLeft[0], directionsLeft[0]))
                {
                    slotsLeft.Insert(1, Map.leftSquare);
                    directionsLeft.Insert(1, Map.leftDirection);
                    movements.Insert(1, moves - 1);

                    slotsLeft[0] = Map.rightSquare;
                    directionsLeft[0] = Map.rightDirection;
                }
                else
                {
                    slotsLeft[0] = Map.GetNextSquare(slotsLeft[0], directionsLeft[0]);
                }

                moves--;
            }
            slotsLeft.RemoveAt(0);
            directionsLeft.RemoveAt(0);
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

    public void RollDiceButton()
    {
        if (currentItemSelected != -1)
        {
            Players.RemoveItem(playersTurn, currentItemSelected);
            currentItemSelected = -1;
        }

        CurrentDice.GetComponent<DiceController>().RollDice();

        RollOption.SetActive(false);

        HideTextMovements();

        GameCamera.GetComponent<CameraController>().Locate(new Vector3(CurrentDice.transform.position.x, CurrentDice.transform.position.y + 20, CurrentDice.transform.position.z), CurrentDice);

    }

    public void NextSlotButton(int direction)
    {
        if (Map.mustChooseDirection(nextSlotVisualization, nextSlotVisualizationDirection * direction))
        {

        }
        else
        {
            nextSlotMovements += direction;
            TextMovements.SetActive(true);
            TextMovements.GetComponent<Text>().text = nextSlotMovements.ToString();
            nextSlotVisualization = Map.GetNextSquare(nextSlotVisualization, nextSlotVisualizationDirection * direction);
            Vector3 nextSlotVisualizationPosition = Map.GetSquarePosition(nextSlotVisualization);
            GameCamera.GetComponent<CameraController>().Rotate(new Vector3(nextSlotVisualizationPosition.x, nextSlotVisualizationPosition.y + 200, nextSlotVisualizationPosition.z), new Vector3(90, 0, 0));

            if (LastSpotlightsNextSlot != null)
            {
                Destroy(LastSpotlightsNextSlot);
            }
            LastSpotlightsNextSlot = Instantiate(spotlight, new Vector3(nextSlotVisualizationPosition.x, nextSlotVisualizationPosition.y + 12f, nextSlotVisualizationPosition.z), spotlight.transform.rotation);
        }
    }

    public void AddItem()
    {
        Players.AddItem(playersTurn, GetRandomItem());
    }

    public void ItemButton(int itemNum)
    {
        if (Players.GetItem(playersTurn, itemNum).Equals(Item.Dice13))
        {
            KillDiceSides();
            if (CurrentDiceType.Equals(DiceType.Dice13))
            {
                CreateDiceSides(DiceType.Dice16);
                currentItemSelected = -1;
            }
            else
            {
                CreateDiceSides(DiceType.Dice13);
                currentItemSelected = itemNum;
            }
        }
        else if (Players.GetItem(playersTurn, itemNum).Equals(Item.Dice46))
        {
            KillDiceSides();
            if (CurrentDiceType.Equals(DiceType.Dice46))
            {
                CreateDiceSides(DiceType.Dice16);
                currentItemSelected = -1;
            }
            else
            {
                CreateDiceSides(DiceType.Dice46);
                currentItemSelected = itemNum;
            }
        }
        else if (Players.GetItem(playersTurn, itemNum).Equals(Item.Trap))
        {
            int currentSquareNum = Players.GetCurrentSquare(playersTurn);
            if (Map.GetSquareType(currentSquareNum).Equals(SquareType.Empty))
            {
                Instantiate(HammerSprite, Map.GetSquarePosition(currentSquareNum), Map.GetSquare(currentSquareNum).transform.rotation).transform.SetParent(Map.GetSquare(currentSquareNum).transform);
                Instantiate(Hammer, GetCurrentPlayer().transform.position + new Vector3(0, -2, 0), GetCurrentPlayer().transform.rotation).transform.SetParent(Map.GetSquare(currentSquareNum).transform);
                Map.AddItemToSquare(currentSquareNum, Item.Trap);
                Map.GetSquare(currentSquareNum).GetComponent<MeshRenderer>().material.color = SoftGreyMaterial.color;

                Players.RemoveItem(playersTurn, itemNum);
            }
        }
        else if (Players.GetItem(playersTurn, itemNum).Equals(Item.BigTrap))
        {
            int currentSquareNum = Players.GetCurrentSquare(playersTurn);
            if (Map.GetSquareType(currentSquareNum).Equals(SquareType.Empty))
            {
                Instantiate(BigTrapSprite, Map.GetSquarePosition(currentSquareNum), Map.GetSquare(currentSquareNum).transform.rotation).transform.SetParent(Map.GetSquare(currentSquareNum).transform);
                Instantiate(BigTrap, Map.GetSquarePosition(currentSquareNum), GetCurrentPlayer().transform.rotation).transform.SetParent(Map.GetSquare(currentSquareNum).transform);
                Map.AddItemToSquare(currentSquareNum, Item.BigTrap);
                Map.GetSquare(currentSquareNum).GetComponent<MeshRenderer>().material.color = SoftGreyMaterial.color;

                Players.RemoveItem(playersTurn, itemNum);
            }
        }
        else if (Players.GetItem(playersTurn, itemNum).Equals(Item.ChangeDirection))
        {
            //StartCoroutine();
        }
    }

    public Item GetRandomItem()
    {
        int numRandom = Random.Range(0, 100);
        if (numRandom < dice13Chance)
        {
            return Item.Dice13;
        }
        else if (numRandom < dice13Chance + dice46Chance)
        {
            return Item.Dice46;
        }
        else if (numRandom < dice13Chance + dice46Chance + trapChance)
        {
            return Item.Trap;
        }
        else if (numRandom < dice13Chance + dice46Chance + trapChance + bigTrapChance)
        {
            return Item.BigTrap;
        }
        else if (numRandom < dice13Chance + dice46Chance + trapChance + bigTrapChance + directionChangeChance)
        {
            return Item.ChangeDirection;
        }
        else
        {
            return Item.ChoosePosition;
        }
    }

    private IEnumerator TrapController(int squareNum)
    {
        yield return new WaitForSeconds(1f);
        GameObject trapInstantiated = Map.GetSquare(squareNum).transform.GetChild(1).GetChild(0).gameObject;
        while (trapInstantiated.transform.rotation.eulerAngles.z < 90)
        {
            trapInstantiated.transform.Rotate(Vector3.forward * 5);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.5f);
        Map.SetEmptySquare(squareNum);
        Map.GetSquare(squareNum).GetComponent<MeshRenderer>().material = PinkMaterial;
        Destroy(Map.GetSquare(squareNum).transform.GetChild(1).gameObject);
        Destroy(Map.GetSquare(squareNum).transform.GetChild(0).gameObject);
    }

    private IEnumerator BigTrapController(int squareNum)
    {
        foreach (ParticleSystem ps in Map.GetSquare(Players.GetCurrentSquare(playersTurn)).transform.GetChild(1).gameObject.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }

        yield return new WaitForSeconds(3f);

        Map.SetEmptySquare(squareNum);
        Map.GetSquare(squareNum).GetComponent<MeshRenderer>().material = PinkMaterial;

        yield return new WaitForSeconds(1f);

        GameObject BigTrapInstantiated = Map.GetSquare(squareNum).transform.GetChild(1).gameObject;
        float yDown = BigTrapInstantiated.transform.position.y - 110;
        while (BigTrapInstantiated.transform.position.y > yDown)
        {
            BigTrapInstantiated.transform.Translate(Vector3.down * 0.15f);
            yield return new WaitForSeconds(0.02f);
        }

        Destroy(BigTrapInstantiated);
        Destroy(Map.GetSquare(squareNum).transform.GetChild(1).gameObject);
    }
}
