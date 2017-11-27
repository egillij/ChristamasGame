using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int score;

    public Texture gift;

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
        if (SceneManager.GetActiveScene().name != "Menu")// && SceneManager.GetActiveScene().name != "LevelSelect")
        {
            GUIContent giftImage = new GUIContent(gift);
            GUIStyle giftStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(5, 0, 0, 0)
                
            };
            GUI.Box(new Rect(0, 0, 100, 70), giftImage, giftStyle);
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
        }        
    }
    
}
