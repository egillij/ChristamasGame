using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour {

    public Rigidbody2D Rbody { get; set; }

    public Animator Animator { get; private set; }


    // Use this for initialization
    void Start () {
        Rbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }
	
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rbody.gravityScale = 1.0f;            
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Icicle" && Rbody.gravityScale > 0f)
        {
            Animator.SetTrigger("break");
        }
    }

    void DestoryElement()
    {
        Destroy(gameObject);
    }
}
