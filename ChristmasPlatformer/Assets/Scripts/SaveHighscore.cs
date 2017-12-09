using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveHighscore : MonoBehaviour {

    private string gameDataProjectFilePath = "highscore.json";

    private Highscore[] highscores;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private InputField nameInput;

    // Use this for initialization
    void Start ()
    {
        scoreText.text = "Score: " + GameManager.instance.finalScore.ToString();

        string filePath = Path.Combine(Application.dataPath, gameDataProjectFilePath);
        
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            highscores = JsonHelper.FromJson<Highscore>(dataAsJson);            
        }
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
                    highScoreInstance[count] = new Highscore();
                    highScoreInstance[count].name = nameInput.text;
                    highScoreInstance[count].score = GameManager.instance.finalScore;

                    count++;
                    saved = true;
                }

                highScoreInstance[count] = new Highscore();
                highScoreInstance[count].name = highscore.name;
                highScoreInstance[count].score = highscore.score;

                count++;
            }

            string highScoreToJson = JsonHelper.ToJson(highScoreInstance, true);
            string filePath = Path.Combine(Application.dataPath, gameDataProjectFilePath);

            File.WriteAllText(filePath, highScoreToJson);
        }
        else
        {
            Highscore[] highScoreInstance = new Highscore[1];

            highScoreInstance[0] = new Highscore();
            highScoreInstance[0].name = nameInput.text;
            highScoreInstance[0].score = GameManager.instance.score;

            string highScoreToJson = JsonHelper.ToJson(highScoreInstance, true);
            string filePath = Path.Combine(Application.dataPath, gameDataProjectFilePath);

            File.WriteAllText(filePath, highScoreToJson);      
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
