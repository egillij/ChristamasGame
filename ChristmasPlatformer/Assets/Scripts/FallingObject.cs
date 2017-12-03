using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player");
            if(this.gameObject.tag == "Gift")
            {
                Camera[] cams = GameObject.FindObjectsOfType<Camera>();
                foreach (Camera cam in cams)
                {
                    if (cam.name == "Falling Camera")
                    {
                        cam.GetComponent<CaptureGame>().score += 10;
                    }
                }
                
                Destroy(this.gameObject);
            }

            if (this.gameObject.tag == "Bomb")
            {
                Camera[] cams = GameObject.FindObjectsOfType<Camera>();
                foreach (Camera cam in cams)
                {
                    if (cam.name == "Falling Camera")
                    {
                        float currentScore = cam.GetComponent<CaptureGame>().score;
                        cam.GetComponent<CaptureGame>().score = currentScore >=15 ? cam.GetComponent<CaptureGame>().score-15 : 0.0f;
                        
                    }
                }
            }
            
        }
    }

    private void OnBecameInvisible()
    {
        //Count down gifts remaining
        Destroy(this.gameObject);
    }
}
