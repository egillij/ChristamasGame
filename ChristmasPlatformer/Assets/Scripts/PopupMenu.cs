using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : MonoBehaviour {

    private Rect position;
    [SerializeField]
    private Texture popupbackground;
    private bool open;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    public void OpenPopup(string sceneName)
    {
        open = true;
    }
}
