using LibPDBinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public Animator Animator { get; private set; }
    private CircleCollider2D ExplosionRadius;
    private BoxCollider2D bombTrigger;

    // Use this for initialization
    void Start ()
    {
        Animator = GetComponent<Animator>();
        ExplosionRadius = GetComponent<CircleCollider2D>();
        ExplosionRadius.enabled = false;

        bombTrigger = GetComponent<BoxCollider2D>();
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other is BoxCollider2D && other.gameObject.tag == "Player" && bombTrigger.enabled)
        {
            Animator.SetTrigger("Burn");            
            LibPD.SendBang("fuse");

            bombTrigger.enabled = false;            
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
