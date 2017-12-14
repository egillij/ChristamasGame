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
        
        if ((platform2DPosition - originPosition).magnitude >= range
            && ((platform2DPosition.x - originPosition.x > 0f && movingDirection.x > 0f)
            || (platform2DPosition.x - originPosition.x < 0f && movingDirection.x < 0f)
            || (platform2DPosition.y - originPosition.y > 0f && movingDirection.y > 0f)
            || (platform2DPosition.y - originPosition.y < 0f && movingDirection.y < 0f))
            ) {

            movingDirection = -1.0f * movingDirection;
            rb.velocity = Vector2.zero;
        }
        
        rb.transform.Translate(movingDirection * speed * Time.deltaTime);        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player" && !playerOnTop)
        {
            playerOnTop = true;
            Player.Instance.MovingPlatform = this.gameObject;            
            collision.gameObject.transform.parent = this.transform;
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !playerOnTop)
        {
            playerOnTop = true;
            Player.Instance.MovingPlatform = this.gameObject;            

            collision.gameObject.transform.parent = this.transform;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOnTop = false;            
            Player.Instance.MovingPlatform = null;

            collision.gameObject.transform.parent = null;
        }
    }

}
