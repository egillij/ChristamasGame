using LibPDBinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInScene : MonoBehaviour {

    [SerializeField]
    private string pdFileName;
    private bool isPlayed = false;

    [SerializeField]
    private bool stopOnDestroy;
    
	void Update()
    {

        if(!isPlayed)
        {
            
            LibPD.SendBang(pdFileName);
            isPlayed = true;
        }
    }

    private void OnDestroy()
    {
        if (stopOnDestroy)
            LibPD.SendBang(pdFileName + "Stop");
    }
}
