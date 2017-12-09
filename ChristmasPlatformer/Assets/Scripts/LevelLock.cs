using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLock : MonoBehaviour {

	public void UnlockAll()
    {
        for (int i = 0; i < GameManager.instance.levelLock.Length; i++)
        {
            GameManager.instance.levelLock[i] = true;
        }
    }
}
