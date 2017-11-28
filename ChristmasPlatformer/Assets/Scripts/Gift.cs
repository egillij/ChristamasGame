using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LibPDBinding;

public class Gift : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //LibPD.SendFloat("banger", 1f);
            LibPD.SendBang("banger");
            GameManager.instance.score += 1;
            Destroy(gameObject);
        }
    }
}
