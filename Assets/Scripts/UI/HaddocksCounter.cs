using UnityEngine;
using UnityEngine.UI;

public class HaddocksCounter : MonoBehaviour
{
    private Text text;
    public static int haddocksAmount;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = haddocksAmount.ToString();
    }
}
