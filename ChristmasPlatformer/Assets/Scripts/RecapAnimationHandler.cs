using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapAnimationHandler : MonoBehaviour {

	public void Death()
    {
        LevelRecap.Instance.Death();
    }
}
