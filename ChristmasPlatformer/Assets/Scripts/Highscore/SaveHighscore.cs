using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveHighscore : MonoBehaviour {
    private Highscore[] highscores;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private InputField nameInput;

    private SaveLoadHighscore highscoreSettings = new SaveLoadHighscore();

    // Use this for initialization
    void Start ()
    {
        highscores = highscoreSettings.GetHighscoresFromLevel("Overall");        
    }
	
    public void SaveToJson()
    {
        if (highscores.Length > 0)
        {
            Highscore[] highScoreInstance = new Highscore[highscores.Length+1];

            int count = 0;
            bool saved = false;
            foreach (Highscore highscore in highscores)
            {
                if (!saved && GameManager.instance.finalScore >= highscore.score)
                {
                    Highscore newHighscore = highscoreSettings.CreateHighscore(nameInput.name, GameManager.instance.finalScore);
                    highScoreInstance[count] = newHighscore;
                    
                    count++;
                    saved = true;
                }

                highScoreInstance[count] = highscore;

                count++;
            }

            highscoreSettings.SaveToJson(highScoreInstance, "Overall");
        }
        else
        {
            Highscore[] highScoreInstance = new Highscore[1];

            Highscore newHighscore = highscoreSettings.CreateHighscore(nameInput.name, GameManager.instance.finalScore);
            highScoreInstance[0] = newHighscore;

            highscoreSettings.SaveToJson(highScoreInstance, "Overall");  
        }

        //Destroy(GameManager.instance.gameObject);

        SceneManager.LoadScene("Menu");
    }

    public void BackToMenu()
    {
        //Destroy(GameManager.instance.gameObject);

        SceneManager.LoadScene("Menu");
    }
}
