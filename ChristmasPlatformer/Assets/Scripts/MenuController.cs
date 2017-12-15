using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private GameObject inputPanel;

    [SerializeField]
    private InputField playerInput;

    public void CloseInputWindow()
    {
        inputPanel.SetActive(false);
    }

    public void OpenInputWindow()
    {
        inputPanel.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        if (sceneName == "LevelSelect")
        {
            string playerName = playerInput.text;
            if (!string.IsNullOrEmpty(playerName))
            {
                GameManager.instance.playerName = playerName;
                GameManager.instance.levelLock = new bool[] { true, false, false };
                GameManager.instance.finalScore = 0;
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
