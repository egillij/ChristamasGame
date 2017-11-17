using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour {
    public float speed;
    public Vector2 direction;
    public float range;

    private Rigidbody2D rb;
    private Vector2 originPosition;
    private Vector2 movingDirection;

    private bool playerOnTop;
    private Rigidbody2D pBody;

	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        originPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        movingDirection = direction;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        Vector2 platform2DPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        if ((platform2DPosition - originPosition).magnitude >= range)
        {
            movingDirection = -1.0f * movingDirection;
        }
        //rb.velocity = movingDirection * speed;
        rb.MovePosition(platform2DPosition + movingDirection * speed * Time.fixedDeltaTime);
        
        if (playerOnTop)
        {
            if (!Player.Instance.Jump)
            {
                pBody.MovePosition(pBody.position + movingDirection * speed * Time.fixedDeltaTime);
            }
                

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnTop = true;
            pBody = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnTop = false;
            pBody = null;
        }
    }
}
