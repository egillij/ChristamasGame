using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CongratulationsController : MonoBehaviour {

    private bool levelEnd;
	// Use this for initialization
	void Start () {
        levelEnd = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectsWithTag("Gift").Length <= 0 && !levelEnd)
        {
            levelEnd = true;
            StartCoroutine(ChangeScene());
        }
	}

    IEnumerator ChangeScene()
    {
        SceneManager.LoadScene("LevelScore", LoadSceneMode.Additive);
        while (!SceneManager.GetSceneAt(1).isLoaded)
        {
            yield return null;
        }

        LevelRecap.Instance.InitializeRecap(GameManager.instance.score, GameManager.instance.EnemiesKilled, GameManager.instance.bonusScore, 3, true, GameManager.instance.LevelDuration);
        LevelRecap.Instance.SceneName = "LevelSelect";        
    }
}
