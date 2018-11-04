using System;
using UnityEngine;

public interface IInventoryItem
{
    string Name { get; }

    string Description { get; }

    Sprite Image { get; }

    void OnPickUp();
}

public class InventoryEventArgs : EventArgs
{
    public IInventoryItem Item;

    public InventoryEventArgs(IInventoryItem item)
    {
        Item = item;
    }

}
