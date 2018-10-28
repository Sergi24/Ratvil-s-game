using UnityEngine;

public enum SquareType
{
    Empty,
    Trap,
    BigTrap
}

public static class Map
{
    public static int rightSquare, leftSquare, rightDirection, leftDirection;

    private static GameObject[] Squares;
    private static SquareType[] squareTypes;
    private static readonly GameObject Objects;

    public static void InitializeSquares()
    {
        Squares = new GameObject[69];
        squareTypes = new SquareType[69];

        for (int i = 0; i < Squares.Length; i++)
        {
            Squares[i] = GameObject.Find(string.Concat("ReceptionSlot", i));
            squareTypes[i] = SquareType.Empty;
        }
    }

    public static GameObject GetSquare(int squareNum)
    {
        return Squares[squareNum];
    }

    public static Vector3 GetSquarePosition(int squareNum)
    {
        return Squares[squareNum].transform.position;
    }

    public static SquareType GetSquareType(int squareNum)
    {
        return squareTypes[squareNum];
    }

    public static int GetNextSquare(int position, int direction)
    {
        if (position + direction > 68)
        {
            return 0;
        }
        else if (position + direction < 0)
        {
            return 68;
        }
        else
        {
            return position + direction;
        }
    }

    public static bool mustChooseDirection(int position, int direction)
    {
        if (position == 6 && direction == 1)
        {
            rightSquare = 7;
            rightDirection = 1;
            leftSquare = 65;
            leftDirection = -1;
            return true;
        }
        else if (position == 7 && direction == -1)
        {
            rightSquare = 65;
            rightDirection = -1;
            leftSquare = 6;
            leftDirection = -1;
            return true;
        }
        else if (position == 65 && direction == 1)
        {
            rightSquare = 6;
            rightDirection = -1;
            leftSquare = 7;
            leftDirection = 1;
            return true;
        }
        else if (position == 24 && direction == 1)
        {
            rightSquare = 39;
            rightDirection = -1;
            leftSquare = 25;
            leftDirection = 1;
            return true;
        }
        else if (position == 25 && direction == -1)
        {
            rightSquare = 24;
            rightDirection = -1;
            leftSquare = 39;
            leftDirection = -1;
            return true;
        }
        else if (position == 39 && direction == 1)
        {
            rightSquare = 25;
            rightDirection = 1;
            leftSquare = 24;
            leftDirection = -1;
            return true;
        }
        else if (position == 42 && direction == 1)
        {
            rightSquare = 66;
            rightDirection = 1;
            leftSquare = 43;
            leftDirection = 1;
            return true;
        }
        else if (position == 43 && direction == -1)
        {
            rightSquare = 42;
            rightDirection = -1;
            leftSquare = 66;
            leftDirection = 1;
            return true;
        }
        else if (position == 66 && direction == -1)
        {
            rightSquare = 43;
            rightDirection = 1;
            leftSquare = 42;
            leftDirection = -1;
            return true;
        }
        else if (position == 32 && direction == 1)
        {
            rightSquare = 33;
            rightDirection = 1;
            leftSquare = 40;
            leftDirection = 1;
            return true;
        }
        else if (position == 33 && direction == -1)
        {
            rightSquare = 40;
            rightDirection = 1;
            leftSquare = 32;
            leftDirection = -1;
            return true;
        }
        else if (position == 40 && direction == -1)
        {
            rightSquare = 32;
            rightDirection = -1;
            leftSquare = 33;
            leftDirection = 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void AddItemToSquare(int squareNum, Item item)
    {
        if (item.Equals(Item.Trap))
        {
            squareTypes[squareNum] = SquareType.Trap;
        }
        else if (item.Equals(Item.BigTrap))
        {
            squareTypes[squareNum] = SquareType.BigTrap;
        }
    }

    public static void SetEmptySquare(int squareNum)
    {
        squareTypes[squareNum] = SquareType.Empty;
    }
}
