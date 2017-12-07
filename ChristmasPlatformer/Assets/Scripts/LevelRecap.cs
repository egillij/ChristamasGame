using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelRecap : MonoBehaviour {

    private int giftCount;
    private int enemyCount;
    private float sublevelScore;
    private int levelNumber;

    // Use this for initialization
	void Start () {
        InitializeRecap(5, 3, 100, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeRecap(int giftCount, int enemyCount, int sublevelScore, int levelNumber)
    {
        this.giftCount = giftCount;
        this.enemyCount = enemyCount;
        this.sublevelScore = sublevelScore;
        this.levelNumber = levelNumber;
        
        
    }

    private void OnGUI()
    {

        GUIStyle headerStyle = new GUIStyle()
        {
            fontSize = 30,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            richText = true,
            normal = new GUIStyleState()
            {
                textColor = Color.black
            },
            overflow = new RectOffset(0, 300, 0, 300)
        };
        GUI.Label(new Rect(0f, 200f, Screen.width, 30), string.Format("Level {0} Completed", levelNumber), headerStyle);


        GUIStyle giftStyle = new GUIStyle()
        {
            fontSize = 24,
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold,
            richText = true,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        GUI.Label(new Rect(300f, 275f, 200, 30), "Gifts Collected:", giftStyle);
        GUI.Label(new Rect(600f, 275f, 200, 30), string.Format("{0} x 100", giftCount), giftStyle);

        GUIStyle enemyStyle = new GUIStyle()
        {
            fontSize = 24,
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold,
            richText = true,
            normal = new GUIStyleState()
            {
                textColor = new Color(79f/255f, 0f, 0f)
            }
        };
        GUI.Label(new Rect(300f, 350f, 200, 30), "Enemies defeated:", enemyStyle);
        GUI.Label(new Rect(600f, 350f, 200, 30), string.Format("{0} x 50", enemyCount), enemyStyle);

        GUIStyle sublevelStyle = new GUIStyle()
        {
            fontSize = 24,
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold,
            richText = true,
            normal = new GUIStyleState()
            {
                textColor = new Color(25f/255, 159f/255, 0f),
            }
        };
        GUI.Label(new Rect(300f, 425f, 200, 30), "Hidden Level", sublevelStyle);
        GUI.Label(new Rect(600f, 425f, 200, 30), string.Format("{0}", sublevelScore), sublevelStyle);
    }
}
