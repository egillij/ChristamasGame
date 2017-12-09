using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadHighscore
{
    private string gameDataProjectFilePath = "highscore.json";

    private string[] highscoreTypes = new string[] {"Overall", "Level1", "Level2", "Level3" };

    public Highscore[] GetHighscoresFromLevel(string levelName)
    {
        string filePath = Path.Combine(Application.dataPath, gameDataProjectFilePath);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            Highscore[] highscores = JsonHelper.FromJson<Highscore>(dataAsJson, levelName);

            return highscores;            
        }
        else
        {
            return new Highscore[0];
        }
    }

    public Highscore CreateHighscore(string name, int score)
    {
        Highscore highScore = new Highscore();
        highScore.name = name;
        highScore.score = score;

        return highScore;
    }

    public void SaveToJson(Highscore[] highscoreInstance, string updatedHighscore)
    {
        string filePath = Path.Combine(Application.dataPath, gameDataProjectFilePath);

        Dictionary<string, Highscore[]> highscoreDictionary = GetAllOtherHigscores(updatedHighscore);

        highscoreDictionary.Add(updatedHighscore, highscoreInstance);
        

        string highScoreToJson = JsonHelper.ToJson(
            highscoreDictionary["Overall"], 
            highscoreDictionary["Level1"],
            highscoreDictionary["Level2"],
            highscoreDictionary["Level3"],
            true
        );

        File.WriteAllText(filePath, highScoreToJson);
    }

    private Dictionary<string, Highscore[]> GetAllOtherHigscores(string notGet)
    {
        Dictionary<string, Highscore[]> allOtherHighscores = new Dictionary<string, Highscore[]>();
        
        foreach (string type in highscoreTypes)
        {
            if (notGet == type)
            {
                continue;
            }

            Highscore[] highscores = GetHighscoresFromLevel(type);

            allOtherHighscores.Add(type, highscores);            
        }

        return allOtherHighscores;
    }
}
