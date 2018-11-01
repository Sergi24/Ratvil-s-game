using UnityEngine;

public class Haddock : MonoBehaviour, IInventoryItem
{
    public string Name
    {
        get
        {
            return "Haddock";
        }
    }

    public string Description
    {
        get
        {
            return "Description...";
        }
    }

    public Sprite _Image = null;

    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }

    public void OnPickUp()
    {
        // Haddocks++
        HaddocksCounter.haddocksAmount += 1;

        // Item removed
        gameObject.SetActive(false);
    }
}
