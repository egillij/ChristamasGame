using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibPDBinding;

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
    [SerializeField]
    private float updraftDelay;

    private float nextSmoke;

    private bool smokeOn;
    private float smokeStart;

    private bool forceApplied = false;
    private bool isStopping = false;

    private bool isVisible;

	// Use this for initialization
	void Start () {
        pBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        //playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        nextSmoke = Time.time;
        if (smokeSystem.isPlaying)
        {
            smokeSystem.Stop();
            isStopping = true;
            effectCollider.enabled = false;
            smokeOn = false;
        }
        
        
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextSmoke)
        {
            smokeSystem.Play();

            //LibPD.SendFloat("smokeAttack", 0.3f);
            //LibPD.SendFloat("smokeDecay", -0.3f);
            LibPD.SendFloat("smokeVolume", 0.2f);
            LibPD.SendBang("smokebang");
            if (!isVisible)
                StartCoroutine(FadeSmoke());

            //effectCollider.enabled = true;
            nextSmoke = Time.time + smokeDuration + smokeInterval;
            smokeStart = Time.time;
            smokeOn = true;
            forceApplied = false;
            isStopping = false;
        }

        if (Time.time > nextSmoke - smokeInterval)
        {
            if (!isStopping)
            {
                isStopping = true;
                Debug.Log("STOP");
                smokeSystem.Stop();
                StartCoroutine(WaitForSmoke());

                effectCollider.enabled = false;
                smokeOn = false;
            }
            
        }

        if (smokeOn && Time.time >= smokeStart + updraftDelay)
        {
            effectCollider.enabled = true;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (smokeOn)
            {
                if (!forceApplied)
                {
                    forceApplied = !forceApplied;
                    //Uses time to decay the updraft force
                    //float updraft = Mathf.Exp(updraftDecay * (Time.time - smokeStart)) * forceMag;
                    //pBody.AddForce(new Vector2(0.0f, updraft));

                    //Uses distance to decay the updraft force
                    if (pBody.velocity.y < -5.0)
                        pBody.velocity = new Vector2(pBody.velocity.x, 0.0f);
                    float distance = (collision.gameObject.transform.position - this.gameObject.transform.position).magnitude;
                    float updraft = Mathf.Exp(updraftDecay * distance) * forceMag;
                    pBody.AddForce(new Vector2(0.0f, updraft), ForceMode2D.Impulse);
                }
                
            }
        }
        
    }

    IEnumerator WaitForSmoke()
    {
        yield return new WaitForSeconds(1.0f);
        LibPD.SendBang("stopbang");
    }

    private void OnBecameVisible()
    {
        isVisible = true;
        StopCoroutine(FadeSmoke());
        LibPD.SendFloat("smokeScale", 1.0f);
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        StartCoroutine(FadeSmoke());
    }

    IEnumerator FadeSmoke()
    {
        float dist = (Player.Instance.transform.position - transform.position).magnitude;
        yield return LibPD.SendFloat("smokeScale", 1 /(1.5f*dist));
    }

    


}
