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

    public bool playerOnTop;
    //private Rigidbody2D pBody;

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
            rb.velocity = Vector2.zero;
            //if (playerOnTop)
            //{
            //    pBody.velocity = Vector2.zero;
            //}
        }
        //rb.velocity = movingDirection * speed;
        rb.transform.Translate(movingDirection * speed * Time.deltaTime);
        //rb.MovePosition(platform2DPosition + movingDirection * speed * Time.fixedDeltaTime);
        //rb.AddForce(movingDirection * speed);

        //if (playerOnTop)
        //{
        //    if (!Player.Instance.Jump)
        //    {
        //        //pBody.MovePosition(pBody.position + movingDirection * speed * Time.fixedDeltaTime);
        //        pBody.transform.Translate(movingDirection * speed * Time.fixedDeltaTime);
        //    }
        //}
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player" && !playerOnTop)
        {
            playerOnTop = true;
            Player.Instance.MovingPlatform = this.gameObject;
            //pBody = collision.gameObject.GetComponent<Rigidbody2D>();
            //pBody.velocity = Vector2.zero;
            collision.gameObject.transform.parent = this.transform;
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !playerOnTop)
        {
            playerOnTop = true;
            Player.Instance.MovingPlatform = this.gameObject;
            //pBody = collision.gameObject.GetComponent<Rigidbody2D>();
            //pBody.velocity = Vector2.zero;

            collision.gameObject.transform.parent = this.transform;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnTop = false;
            //pBody = null;
            Player.Instance.MovingPlatform = null;

            collision.gameObject.transform.parent = null;
        }
    }

}
