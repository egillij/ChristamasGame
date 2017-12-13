using LibPDBinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInScene : MonoBehaviour {

    [SerializeField]
    private string pdFileName;
    
	void Awake()
    {
        LibPD.SendBang(pdFileName);
    }
}
