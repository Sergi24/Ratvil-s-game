using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //  this script is in charge of the game, it says whose turn it is and what happens

    //  This is what I would want to happen in a single turn:
    //BoardManager calls function StartTurn() this
    //Tells Inputs that it is allowed to take inputs from the player,
    //Inputs then gets the input from the player, on a click it calls GetMousePosition now this
    //gets the position of the mouse and sends it to BoardManager
    //BoardManager uses this position to determine which tile the player is trying to move to. It then checks if the move is allowed,
    //if the move is allowed it tells the BoardManager the player is moving and Mover how to move the player,
    //if the move is not allowed it reports back the Inputs that it wasn't valid, GoTo 2.
    //GameManager now knows the player is moving and tells Inputs to stop getting inputs from the player.Mover uses the directions from BoardManager to move the player around the board, once it finishes it tells the GameManager,
    //GameManager knows the player has stopped moving, the players turn is now over and the GameManager calls EndTurn() this,
    //tells Inputs which player it now controls,
    //calls StartTurn() GoTo 1.

    public static GameManager instance;

    public List<GameObject> playerObjects = new List<GameObject>();

    public List<Player> players = new List<Player>();
    private int currentPlayerIndex = 0;
    private Player activePlayer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(playerObj.GetComponent<Player>());
        }

        activePlayer = players[0];
    }

    private void Update()
    {
        players[currentPlayerIndex].TurnUpdate();

    }

    public void StartTurn()
    {

    }

    public void NextTurn()
    {
        if (currentPlayerIndex + 1 < players.Count)
        {
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = 0;
        }
    }

    public void EndTurn()
    {

    }

    private void GameOver()
    {

    }
}
