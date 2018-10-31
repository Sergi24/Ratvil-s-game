using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GotoGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GotoTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
