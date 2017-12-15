using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMenu : MonoBehaviour {

    private Rect position;
    [SerializeField]
    private Texture popupbackground;
    private bool open;

    public void OpenPopup(string sceneName)
    {
        open = true;
    }
}
