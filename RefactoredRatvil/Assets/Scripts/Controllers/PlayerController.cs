using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    // this is the script which actually moves the player from A to B, it will get the path from the BoardManager then move the player

    private Animator animator;

    private NavMeshAgent navMeshAgent;

    private bool jogging = false;

    public Inventory Inventory;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Camera.main)
        {
            // MOUSE ONLY - NEEDS TO BE CHANGED
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0) && (Physics.Raycast(ray, out hit, 100)))
            {
                navMeshAgent.destination = hit.point;
            }

            jogging = navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;

            animator.SetBool("jog", jogging);
        }
    }

    // Add item on collision
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
        if (item != null)
        {
            Inventory.AddItem(item);
        }
    }
}
