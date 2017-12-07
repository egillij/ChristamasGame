using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Kunai : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Rigidbody2D rb;

    private Vector2 direction;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (gameObject.name.Contains("Snowball"))
        {
            GameObject[] enemySights = GameObject.FindGameObjectsWithTag("EnemySight");
            foreach(GameObject es in enemySights)
            {
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), es.GetComponent<BoxCollider2D>());
            }
        }
	}

    private void FixedUpdate()
    {
        
        rb.velocity = direction * speed;
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
