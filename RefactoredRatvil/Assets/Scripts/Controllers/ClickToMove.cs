using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour {

    private Animator animator;

    private NavMeshAgent navMeshAgent;

    private bool jogging = false;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                navMeshAgent.destination = hit.point;
            }
        }

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            jogging = false;
        }
        else
        {
            jogging = true;
        }

        animator.SetBool("jog", jogging);
	}
}
