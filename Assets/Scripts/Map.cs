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
            rightDirection = -1;
            leftSlot = 25;
            leftDirection = 1;
            return true;
        }
        else return false;
    }
}
