using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour {

    public float visibleTime;

    public float disappearTime;

    public bool disappearOnStart;

    private float nextDisappear;
    private float nextVisible;
    private BoxCollider2D boxCol;
    private SpriteRenderer[] renderers;
    private bool visible;

    // Use this for initialization
    void Start () {
        boxCol = this.gameObject.GetComponent<BoxCollider2D>();
        renderers = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        if (disappearOnStart)
        {
            foreach (SpriteRenderer sr in renderers)
            {
                sr.enabled = false;
            }
            boxCol.enabled = false;
            nextVisible = Time.time + disappearTime;
            nextDisappear = nextVisible + visibleTime;
            visible = false;
        }		
        else
        {
            nextDisappear = Time.time + visibleTime;
            nextVisible = nextDisappear + disappearTime;
            visible = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= nextDisappear && visible)
        {
            foreach (SpriteRenderer sr in renderers)
            {
                sr.enabled = false;
            }
            boxCol.enabled = false;
            nextVisible = Time.time + disappearTime;
            visible = false;
        }

        if (Time.time >= nextVisible && !visible)
        {
            foreach (SpriteRenderer sr in renderers)
            {
                sr.enabled = true;
            }
            boxCol.enabled = true;
            nextDisappear = Time.time + visibleTime;
            visible = true;
        }
	}
}
