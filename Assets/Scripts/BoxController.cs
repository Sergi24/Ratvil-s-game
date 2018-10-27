using UnityEngine;

public class BoxController : MonoBehaviour
{
    private Game game;

    private void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<ParticleSystem>().Emit(200);
            game.AddItem();
            Invoke("SetVisibleBox", 2f);
        }
    }

    private void SetVisibleBox()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}
