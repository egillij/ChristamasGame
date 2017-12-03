using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public Animator Animator { get; private set; }
    private CircleCollider2D ExplosionRadius;
    private bool hasHurtPlayer = false;

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
        }
    }

    void TriggerExplosion()
    {
        Animator.SetTrigger("Explode");
        ExplosionRadius.enabled = true;
    }

    void DestoryElement()
    {
        Destroy(gameObject);
    }
}
