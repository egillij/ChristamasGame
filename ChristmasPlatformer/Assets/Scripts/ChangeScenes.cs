using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class ChangeScenes : MonoBehaviour
{

    private Vector3 position;
    private Button btnClicked;
    [SerializeField]
    private Texture popupbackground;
    private bool open;
    private string sceneName;
    private string submenuName;
    private SaveLoadHighscore highscoreSettings = new SaveLoadHighscore();

    public void LoadScene(string sceneName)
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        if (btn.name != submenuName)
        {
            open = false;
        }
        if (!open)
        {
            open = true;
            this.sceneName = sceneName;
            submenuName = btn.name;
            position =  new Vector3(btn.transform.position.x, Screen.height-btn.transform.position.y, 0);
            
        }
        else
        {
            open = false;
            submenuName = "";
        }
        
    }

    private void OnGUI()
    {
        if (open)
        {
            //Force position to be within the viewport
            float boxheight = 400 * popupbackground.height / popupbackground.width;
            if (position.y < 0)
                position.y = 10;

            if (position.y + boxheight > Screen.height)
                position.y = Screen.height - boxheight - 10;

            if (position.x < 0)
                position.x = 410;

            if (position.x + 400 > Screen.width)
                position.x = Screen.width - 410;


            //Set the background of the popup
            GUIStyle middle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Box(new Rect(position.x, position.y, 400, boxheight), popupbackground, middle);


            //High Score
            GUIStyle titleStyle = new GUIStyle()
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState()
                {
                    textColor = Color.white
                }
            };
            GUI.Box(new Rect(position.x + 73, position.y + 10, 250, 150), "");
            GUI.Label(new Rect(position.x + 80, position.y + 5, 250, 30), "Highscores", titleStyle);

            
            Rect namePosition = new Rect(position.x + 80, position.y + 20, 160, 30);
            Rect scorePosition = new Rect(position.x + 230, position.y + 20, 160, 30);
            GUIStyle headerStyle = new GUIStyle()
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
            GUI.Label(namePosition, "Player Name", headerStyle);
            GUI.Label(scorePosition, "Player Score", headerStyle);

            //Score list
            GUIStyle playerNameStyle = new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    textColor = Color.red
                },
                alignment = TextAnchor.MiddleLeft
            };
            namePosition.y += 5;
            scorePosition.y += 5;
            
            Highscore[] highscores = highscoreSettings.GetHighscoresFromLevel(Regex.Replace(submenuName, @"\s+", ""));

            int count = 1;

            foreach (Highscore highscore in highscores)
            {
                namePosition.y += 20;
                scorePosition.y += 20;
                
                GUI.Label(namePosition, string.Format("{0}. {1}", count, highscore.name), playerNameStyle);                
                GUI.Label(scorePosition, highscore.score.ToString(), playerNameStyle);

                if (count == 4) break;

                count++;
            }

            //Buttons
            GUIStyle middleBtn = new GUIStyle("button")
            {
                alignment = TextAnchor.MiddleCenter
            };
            
            if (GUI.Button(new Rect(position.x+180, position.y+boxheight*3/4, 40, 20), "Play", middleBtn))
            {
                GameManager.instance.LevelStart = Time.time;
                GameManager.instance.LevelDuration = 0.0f;
                GameManager.instance.countTime = true;
                GameManager.instance.bonusScore = 0;
                SceneManager.LoadScene(sceneName);
            }
            
        }
    }

    public void LoadSceneSimple(string sceneName)
    {
        if (sceneName == "Menu" && GameManager.instance != null)
        {
            Destroy(GameManager.instance.gameObject);
        }
        SceneManager.LoadScene(sceneName);
    }
}
