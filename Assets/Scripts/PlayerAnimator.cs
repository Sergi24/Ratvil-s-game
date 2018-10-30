using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = gameObject.GetComponent<Animator>();

    }
	
	public void Run()
    {
        animator.SetTrigger("Jogging");
    }

    public void Stop()
    {
        animator.SetTrigger("Idle");
    }
}
