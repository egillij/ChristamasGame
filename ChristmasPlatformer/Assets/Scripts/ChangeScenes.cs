using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ChangeScenes : MonoBehaviour
{

    private Vector3 position;
    private Button btnClicked;
    [SerializeField]
    private Texture popupbackground;
    private bool open;
    private string sceneName;

    public void LoadScene(string sceneName)
    {
        
        //GameManager.instance.LevelStart = Time.time;
        //SceneManager.LoadScene(sceneName);
        if (!open)
        {
            open = true;
            this.sceneName = sceneName;
            GameObject btn = EventSystem.current.currentSelectedGameObject;
            position =  new Vector3(btn.transform.position.x, Screen.height-btn.transform.position.y, 0);
            
        }
        else
        {
            open = false;
        }
        
    }

    private void OnGUI()
    {
        if (open)
        {
            GUIStyle middle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter
            };
            GUIStyle middleBtn = new GUIStyle("button")
            {
                alignment = TextAnchor.MiddleCenter
            };

            float boxheight = 400 * popupbackground.height / popupbackground.width;
            if (position.y < 0)
                position.y = 10;

            if (position.y + boxheight > Screen.height)
                position.y = Screen.height - boxheight - 10;

            if (position.x < 0)
                position.x = 410;

            if (position.x + 400 > Screen.width)
                position.x = Screen.width - 410;

            GUI.Box(new Rect(position.x, position.y, 400, boxheight), popupbackground, middle);

            if (GUI.Button(new Rect(position.x + 200, position.y + boxheight*3/4, 100, 20), "High Scores", middleBtn))
            {
                Debug.Log("OPEN HIGH SCORE LIST");
            }
            if (GUI.Button(new Rect(position.x+100, position.y+boxheight*3/4, 40, 20), "Play", middleBtn))
            {
                SceneManager.LoadScene(sceneName);
            }
            
        }
    }
}
