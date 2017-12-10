using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GetHighscore : MonoBehaviour {

    [SerializeField]
    Text[] nameFields;

    [SerializeField]
    Text[] pointFields;

    SaveLoadHighscore highscoreSettings = new SaveLoadHighscore();

    void Start()
    {
        Highscore[] highscores = highscoreSettings.GetHighscoresFromLevel("Overall");

        if (highscores.Length > 0)
        {
            int count = 0;
            foreach (Highscore highscore in highscores)
            {
                nameFields[count].text = (count + 1).ToString() + ". " + highscore.name;
                pointFields[count].text = highscore.score.ToString();

                if (count == 4) break;

                count++;
            }
        }              
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
