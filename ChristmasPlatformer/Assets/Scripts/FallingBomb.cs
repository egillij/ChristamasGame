﻿using LibPDBinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBomb : MonoBehaviour
{

    public Animator Animator { get; private set; }
    private CircleCollider2D ExplosionRadius;

    // Use this for initialization
    void Start ()
    {
        Animator = GetComponent<Animator>();
        ExplosionRadius = GetComponent<CircleCollider2D>();
        ExplosionRadius.enabled = false;
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other is BoxCollider2D && other.gameObject.tag == "Player")
        {
            Animator.SetTrigger("Burn");
            LibPD.SendBang("fuse");
            TriggerExplosion();
        }
    }

    void TriggerExplosion()
    {
        Animator.SetTrigger("Explode");
        LibPD.SendBang("bombBang");
        ExplosionRadius.enabled = true;
    }

    void DestoryElement()
    {
        Destroy(gameObject);
    }
}
