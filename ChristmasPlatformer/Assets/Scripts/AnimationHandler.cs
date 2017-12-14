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

    public void Death()
    {
        Player.Instance.Death();
    }

    public void DeathSound()
    {
        LibPDBinding.LibPD.SendBang("deathBang");
    }

    public void ThrowSound(int value)
    {
        Debug.Log(value);
        if ((Player.Instance.OnGround && value == 0) || (!Player.Instance.OnGround && value ==1))
        {
            LibPDBinding.LibPD.SendFloat("throwVol", 1.0f);
            LibPDBinding.LibPD.SendBang("throwBang");
        }
        
    }

    public void WalkSound()
    {
        if (Player.Instance.OnGround)
            LibPDBinding.LibPD.SendBang("snowCrunch");
    }

    public void MeleeAttack()
    {
        if (tag.Contains("Enemy"))
        {
            GetComponentInParent<Enemy>().MeleeAttack();
        }
    }
}
