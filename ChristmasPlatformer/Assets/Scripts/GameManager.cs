using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int score;

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
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            GUI.Label(new Rect(0, 0, 80, 80), "Score: " + score);
        }        
    }
    
}
