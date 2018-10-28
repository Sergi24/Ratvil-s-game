
using UnityEngine;
using UnityEngine.UI;

public enum Item
{
    Empty,
    Dice13,
    Dice46,
    MoveTrap,
    Trap,
    BigTrap,
    ChangeDirection,
    ChoosePosition
}


public static class Players
{
    private static int[] position;
    private static int[] direction;
    private static int[] haddocks;
    private static Item[,] items;
    private static int[] numItems;
    private static GameObject[] buttonsItems;

    public static void InitializatePlayers()
    {
        position = new int[4];
        direction = new int[4];
        haddocks = new int[4];
        items = new Item[4, 3];
        numItems = new int[4];
        buttonsItems = new GameObject[3];
        buttonsItems[0] = GameObject.Find("ButtonItem1");
        buttonsItems[1] = GameObject.Find("ButtonItem2");
        buttonsItems[2] = GameObject.Find("ButtonItem3");

        for (int numPlayer = 0; numPlayer < 4; numPlayer++)
        {
            position[numPlayer] = 0;
            direction[numPlayer] = 1;
            haddocks[numPlayer] = 0;

            for (int numItem = 0; numItem < 3; numItem++)
            {
                items[numPlayer, numItem] = Item.Empty;
            }
            numItems[numPlayer] = 0;
        }
    }


    public static void AddHaddocks(int numPlayer, int amount)
    {
        haddocks[numPlayer] += amount;
    }

    public static void AddItem(int numPlayer, Item item)
    {
        bool found = false;
        int i = 0;
        while (!found && i < 3)
        {
            if (items[numPlayer, i].Equals(Item.Empty))
            {
                items[numPlayer, i] = item;
                if (numPlayer == 0)
                {
                    buttonsItems[i].GetComponent<Image>().enabled = true;
                    if (item.Equals(Item.Dice13)) buttonsItems[i].GetComponent<Image>().color = new Color(1, 0.5f, 0);
                    else if (item.Equals(Item.Dice46)) buttonsItems[i].GetComponent<Image>().color = Color.gray;
                    else if (item.Equals(Item.BigTrap)) buttonsItems[i].GetComponent<Image>().color = Color.cyan;
                    else buttonsItems[i].GetComponent<Image>().color = Color.white;
                }
                numItems[numPlayer]++;
                found = true;
                Debug.Log(item.ToString());
            }

            i++;
        }
        if (!found)
        {
            //Choose item to replace
        }
    }

    public static Item GetItem(int numPlayer, int numItem)
    {
        return items[numPlayer, numItem];
    }

    public static Item[] GetItems(int numPlayer)
    {
        Item[] items = new Item[3];
        for (int i = 0; i < 3; i++)
        {
            items[i] = GetItem(numPlayer, i);
        }
        return items;
    }

    public static void EliminateItem(int numPlayer, int numItem)
    {
        items[numPlayer, numItem] = Item.Empty;
        if (numPlayer == 0) buttonsItems[numItem].GetComponent<Image>().enabled = false;
        numItems[numPlayer]--;
    }

    public static void SetCurrentSlot(int numPlayer, int currentSlot)
    {
        position[numPlayer] = currentSlot;
    }

    public static void SetCurrentDirection(int numPlayer, int currentDirection)
    {
        direction[numPlayer] = currentDirection;
    }

    public static int GetCurrentSquare(int numPlayer)
    {
        return position[numPlayer];
    }

    public static int GetCurrentDirection(int numPlayer)
    {
        return direction[numPlayer];
    }

    public static int GetNextSquare(int numPlayer)
    {
        return Map.GetNextSquare(position[numPlayer], direction[numPlayer]);
    }
}
