﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShield : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spell")
        {
            Debug.Log("Shield Trigger");
            animator.SetTrigger("Shield");
        }
    }
}
