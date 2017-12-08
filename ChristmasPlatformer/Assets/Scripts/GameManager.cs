using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int score;

    public int finalScore;

    public Texture gift;
    public Texture heart;

    public float LevelStart;
    public float LevelDuration;
    public int health;

    void Awake()
    {
        MakeSingleton();
    }

    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnGUI()
    {
        if (SceneManager.GetActiveScene().name != "Menu" && SceneManager.GetActiveScene().name != "LevelSelect" && SceneManager.GetActiveScene().name != "Finished")
        {
            // Gift and gift count
            GUIStyle giftStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(5, 0, 0, 0)
                
            };
            GUI.Box(new Rect(0, 0, 100, 70), gift, giftStyle);
            GUIStyle xStyle = new GUIStyle()
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold
            };
            GUI.Label(new Rect(60, 30, 80, 80), "x", xStyle);
            GUIStyle scoreStyle = new GUIStyle()
            {
                fontSize = 24,
                fontStyle = FontStyle.Bold
            };
            GUI.Label(new Rect(72, 27, 80, 80), score.ToString(), scoreStyle);

            //Time counter in seconds
            GUIStyle timeStyle = new GUIStyle()
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold
            };
            GUI.Label(new Rect(Screen.width/2, 0, 100, 100), string.Format("{0:F2}",LevelDuration), timeStyle);

            // Add player health to GUI
            GUIStyle healthStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(5, 0, 0, 0)

            };
            for (int i = 0; i < health; i++)
            {
                GUI.Box(new Rect(Screen.width-250+i*50, 0, 100, 70), heart, healthStyle);
            }
        }        
    }

    private void FixedUpdate()
    {
        LevelDuration += Time.fixedDeltaTime;
    }

}
