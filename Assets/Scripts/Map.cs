using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map {

    public static int rightSlot, leftSlot, rightDirection, leftDirection;

    private static GameObject[] slots;

    private static GameObject objects;

    public static void InitializeSlots()
    {
        slots = new GameObject[69];

        for (int i = 0; i< slots.Length; i++)
        {
            slots[i] = GameObject.Find(string.Concat("ReceptionSlot", i));
        }
    }

    public static Vector3 GetSlotPosition(int numSlot)
    {
        return slots[numSlot].transform.position;
    }

    public static int GetNextSlot(int position, int direction)
    {
        if (position + direction > 68) return 0;
        else if (position + direction < 0) return 68;
        else return position + direction;
    }

    public static bool IsNeedToChooseDirection(int position, int direction)
    {
        if (position == 6 && direction == 1)
        {
            rightSlot = 7;
            rightDirection = 1;
            leftSlot = 65;
            leftDirection = -1;
            return true;
        }
        else if (position == 7 && direction == -1)
        {
            rightSlot = 65;
            rightDirection = -1;
            leftSlot = 6;
            leftDirection = -1;
            return true;
        }
        else if (position == 65 && direction == 1)
        {
            rightSlot = 6;
            rightDirection = -1;
            leftSlot = 7;
            leftDirection = 1;
            return true;
        }
        else if (position == 24 && direction == 1)
        {
            rightSlot = 39;
            rightDirection = -1;
            leftSlot = 25;
            leftDirection = 1;
            return true;
        }
        else if (position == 25 && direction == -1)
        {
            rightSlot = 24;
            rightDirection = -1;
            leftSlot = 39;
            leftDirection = -1;
            return true;
        }
        else if (position == 39 && direction == 1)
        {
            rightSlot = 25;
            rightDirection = 1;
            leftSlot = 24;
            leftDirection = -1;
            return true;
        }
        else if (position == 42 && direction == 1)
        {
            rightSlot = 66;
            rightDirection = 1;
            leftSlot = 43;
            leftDirection = 1;
            return true;
        }
        else if (position == 43 && direction == -1)
        {
            rightSlot = 42;
            rightDirection = -1;
            leftSlot = 66;
            leftDirection = 1;
            return true;
        }
        else if (position == 66 && direction == -1)
        {
            rightSlot = 43;
            rightDirection = 1;
            leftSlot = 42;
            leftDirection = -1;
            return true;
        }
        else if (position == 32 && direction == 1)
        {
            rightSlot = 33;
            rightDirection = 1;
            leftSlot = 40;
            leftDirection = 1;
            return true;
        }
        else if (position == 33 && direction == -1)
        {
            rightSlot = 40;
            rightDirection = 1;
            leftSlot = 32;
            leftDirection = -1;
            return true;
        }
        else if (position == 40 && direction == -1)
        {
            rightSlot = 32;
            rightDirection = -1;
            leftSlot = 33;
            leftDirection = 1;
            return true;
        }
        else return false;
    }
}
