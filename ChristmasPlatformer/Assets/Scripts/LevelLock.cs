using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLock : MonoBehaviour {

    [SerializeField]
    private Button[] levelBtns;

    private void Start()
    {
        ApplyLevelLock();
    }

    public void UnlockAll()
    {
        for (int i = 0; i < GameManager.instance.levelLock.Length; i++)
        {
            GameManager.instance.levelLock[i] = true;
        }
        ApplyLevelLock();
    }

    public void ApplyLevelLock()
    {
        bool[] levelOpen = GameManager.instance.levelLock;
        for (int i = 0; i < levelOpen.Length; i++)
        {
            if (levelOpen[i])
            {
                if (!levelBtns[i].enabled)
                    levelBtns[i].enabled = true;
            }
            else
            {
                if (levelBtns[i].enabled)
                    levelBtns[i].enabled = false;
            }
        }
    }
}
