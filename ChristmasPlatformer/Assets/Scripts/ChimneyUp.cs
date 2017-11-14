using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneyUp : MonoBehaviour {

    [SerializeField]
    private ParticleSystem smokeSystem;

    //private BoxCollider2D playerCollider;
    private Rigidbody2D pBody;

    [SerializeField]
    private BoxCollider2D effectCollider;

    [SerializeField]
    private float smokeDuration;

    [SerializeField]
    private float smokeInterval;

    [SerializeField]
    private float forceMag;

    [SerializeField]
    private float updraftDecay;

    private float nextSmoke;

    private bool smokeOn;
    private float smokeStart;

	// Use this for initialization
	void Start () {
        pBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        //playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        nextSmoke = Time.time;
        if (smokeSystem.isPlaying)
        {
            smokeSystem.Stop();
            effectCollider.enabled = false;
            smokeOn = false;
        }
        
        
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextSmoke)
        {
            smokeSystem.Play();
            //effectCollider.enabled = true;
            nextSmoke = Time.time + smokeDuration + smokeInterval;
            smokeStart = Time.time;
            smokeOn = true;
        }

        if (Time.time > nextSmoke - smokeInterval)
        {
            smokeSystem.Stop();
            effectCollider.enabled = false;
            smokeOn = false;
        }

        if (smokeOn && Time.time >= smokeStart + 1.0f)
        {
            effectCollider.enabled = true;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        if (collision.tag == "Player")
        {
            if (smokeOn)
            {
                float updraft = Mathf.Exp(updraftDecay * (Time.time - smokeStart)) * forceMag;
                pBody.AddForce(new Vector2(0.0f, updraft));
            }
        }
    }


}
