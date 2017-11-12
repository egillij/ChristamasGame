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

    [SerializeField]
    private BoxCollider2D[] edgeColliders;

    private bool playerOnTop;

	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        originPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        movingDirection = direction;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), edgeColliders[0], true);
        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), edgeColliders[1], true);
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 platform2DPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        if ((platform2DPosition - originPosition).magnitude >= range)
        {
            movingDirection = -1.0f * movingDirection;
        }
        rb.MovePosition(platform2DPosition + movingDirection * speed * Time.fixedDeltaTime);

        //if (playerOnTop)
        //{
        //    playerObject.transform.position = new Vector3(rb.transform.position.x, playerObject.transform.position.y, playerObject.transform.position.z);
        //    //Vector2 player2DPosition = new Vector2(playerObject.transform.position.x, playerObject.transform.position.y);
        //    //Rigidbody2D playerBody = playerObject.GetComponent<Rigidbody2D>();
        //    //playerBody.MovePosition(player2DPosition + movingDirection * speed * Time.fixedDeltaTime);
        //    //playerBody.velocity = playerBody.velocity + movingDirection * speed;
        //}
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnTop = true;
            Physics2D.IgnoreCollision(collision.collider, edgeColliders[0], false);
            Physics2D.IgnoreCollision(collision.collider, edgeColliders[1], false);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnTop = false;
            bool isEdgeCollider = false;
            foreach (BoxCollider2D boxCol in edgeColliders)
            {
                if (collision.otherCollider == boxCol)
                {
                    isEdgeCollider = true;
                }
            }
            if (!isEdgeCollider)
            {
                Physics2D.IgnoreCollision(collision.collider, edgeColliders[0], true);
                Physics2D.IgnoreCollision(collision.collider, edgeColliders[1], true);
            }

        }
    }
}
