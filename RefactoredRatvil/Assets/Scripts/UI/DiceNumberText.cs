using UnityEngine;
using UnityEngine.UI;

public class DiceNumberText : MonoBehaviour
{
    private Text text;
    public static int diceNumber;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = diceNumber.ToString();
    }
}
