using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        //GameManager.instance.LevelStart = Time.time;
        SceneManager.LoadScene(sceneName);
    }
}
