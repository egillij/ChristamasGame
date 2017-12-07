using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ThrowAttack(int value)
    {
        if (tag.Contains("Player"))
        {
            Player.Instance.ThrowAttack(value);
        }
        else if (tag.Contains("Enemy"))
        {
            GetComponentInParent<Character>().ThrowAttack(value);
        }
        
    }
}
