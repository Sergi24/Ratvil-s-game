using System.Collections;
using System.Collections.Generic;
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
            Invoke("SetBoxVisible", 2f);
        }
    }

    private void SetBoxVisible()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}
