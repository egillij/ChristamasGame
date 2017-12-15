using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallController : MonoBehaviour {

    public float speed;
    private Vector3 movementDirection;

	// Use this for initialization
	void Start () {
        movementDirection = new Vector3(transform.rotation.z, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = transform.position + movementDirection * speed * Time.fixedDeltaTime;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            Destroy(gameObject);
        }
        
    }
}
