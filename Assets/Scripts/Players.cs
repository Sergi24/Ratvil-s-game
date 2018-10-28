
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
    private static int[] itemsNum;
    private static GameObject[] ItemsButton;

    public static void InitializePlayers()
    {
        position = new int[4];
        direction = new int[4];
        haddocks = new int[4];
        items = new Item[4, 3];
        itemsNum = new int[4];
        ItemsButton = new GameObject[3];
        ItemsButton[0] = GameObject.Find("Item1Button");
        ItemsButton[1] = GameObject.Find("Item2Button");
        ItemsButton[2] = GameObject.Find("Item3Button");

        for (int playerNum = 0; playerNum < 4; playerNum++)
        {
            position[playerNum] = 0;
            direction[playerNum] = 1;
            haddocks[playerNum] = 0;

            for (int numItem = 0; numItem < 3; numItem++)
            {
                items[playerNum, numItem] = Item.Empty;
            }
            itemsNum[playerNum] = 0;
        }
    }


    public static void AddHaddocks(int playerNum, int amount)
    {
        haddocks[playerNum] += amount;
    }

    public static void AddItem(int playerNum, Item item)
    {
        bool found = false;
        int i = 0;
        while (!found && i < 3)
        {
            if (items[playerNum, i].Equals(Item.Empty))
            {
                items[playerNum, i] = item;
                ItemsButton[i].GetComponent<Image>().enabled = true;
                if (item.Equals(Item.Dice13))
                {
                    ItemsButton[i].GetComponent<Image>().color = new Color(1, 0.5f, 0);
                }
                else if (item.Equals(Item.Dice46))
                {
                    ItemsButton[i].GetComponent<Image>().color = Color.gray;
                }
                else if (item.Equals(Item.BigTrap))
                {
                    ItemsButton[i].GetComponent<Image>().color = Color.cyan;
                }
                else
                {
                    ItemsButton[i].GetComponent<Image>().color = Color.white;
                }

                itemsNum[playerNum]++;
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

    public static Item GetItem(int playerNum, int itemNum)
    {
        return items[playerNum, itemNum];
    }

    public static void RemoveItem(int playerNum, int itemNum)
    {
        items[playerNum, itemNum] = Item.Empty;
        ItemsButton[itemNum].GetComponent<Image>().enabled = false;
        itemsNum[playerNum]--;
    }

    public static void SetCurrentSlot(int playerNum, int currentSlot)
    {
        position[playerNum] = currentSlot;
    }

    public static void SetCurrentDirection(int playerNum, int currentDirection)
    {
        direction[playerNum] = currentDirection;
    }

    public static int GetCurrentSquare(int playerNum)
    {
        return position[playerNum];
    }

    public static int GetCurrentDirection(int playerNum)
    {
        return direction[playerNum];
    }

    public static int GetNextSquare(int playerNum)
    {
        return Map.GetNextSquare(position[playerNum], direction[playerNum]);
    }
}
