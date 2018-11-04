using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private static Rigidbody rb;
    public static Vector3 diceVelocity;
    private Vector3 initPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position;
        DiceSetup();
    }

    private void DiceSetup()
    {
        rb.useGravity = false;
        gameObject.SetActive(false);
    }

    public void Roll()
    {
        if (rb != null)
        {
            DiceNumberText.diceNumber = 0;
            gameObject.SetActive(true);

            SetDicePhysics();

            StartCoroutine(DiceRolled());
        }
    }

    private void SetDicePhysics()
    {
        rb.useGravity = true;
        diceVelocity = rb.velocity;
        rb.AddForce(Vector3.up * Random.Range(15, 30), ForceMode.Impulse);
        rb.AddTorque((Vector3.right * Random.Range(-300, 300) + Vector3.up * Random.Range(-300, 300) + Vector3.forward * Random.Range(-300, 300)), ForceMode.Impulse);
    }

    private IEnumerator DiceRolled()
    {
        while (!rb.IsSleeping())
        {
            yield return new WaitForSeconds(1f);
        }

        DiceNumberText.diceNumber = DetermineSideUp();
        Reset();

    }

    private int DetermineSideUp()
    {
        //TO-DO
        return 1;
    }

    private void Reset()
    {
        transform.position = initPosition;
        DiceSetup();
    }
}

