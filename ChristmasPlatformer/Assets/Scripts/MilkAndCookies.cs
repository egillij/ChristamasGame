using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkAndCookies : MonoBehaviour {

    int direction = 1;
    DateTime turnAroundTime;

    void Start()
    {
        turnAroundTime = DateTime.Now;
    }

	void FixedUpdate ()
    {
        MoveUpAndDown();
	}

    void MoveUpAndDown()
    {        
        if (DateTime.Now > turnAroundTime)
        {
            direction *= -1;
            turnAroundTime = turnAroundTime.AddSeconds(1);
        }
        
        transform.position = new Vector2(transform.position.x, transform.position.y + (0.02f * direction));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            LibPDBinding.LibPD.SendBang("drinkBang");
            GameManager.instance.health++;
            Destroy(gameObject);
        }        
    }
}
