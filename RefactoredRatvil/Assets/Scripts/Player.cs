using UnityEngine;

public class Player : MonoBehaviour
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public float Haddocks { get; set; }

    private void Start()
    {
        Haddocks = 100;
    }

    private void Update()
    {

    }

    public void TurnUpdate()
    {

    }
}
