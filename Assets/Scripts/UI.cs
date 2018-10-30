using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public GameObject optionItems, optionThrow;
    public GameObject textMovements;

    public void ShowOptionItems(bool show)
    {
        optionItems.SetActive(show);
    }

    public void ShowOptionThrow(bool show)
    {
        optionThrow.SetActive(show);
    }

    public void ShowTextMovements(bool show)
    {
        textMovements.SetActive(show);
    }

    public void WriteTextMovements(int num)
    {
        ShowTextMovements(true);
        textMovements.GetComponent<Text>().text = num.ToString();
    }


}
