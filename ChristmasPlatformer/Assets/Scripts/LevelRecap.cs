using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelRecap : MonoBehaviour {

    private static LevelRecap instance;

    public static LevelRecap Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelRecap>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject winningSreen;
    [SerializeField]
    private GameObject dyingScreen;

    private float levelTime;
    private bool showTime;

    private int giftCount;
    private int enemyCount;
    private int sublevelScore;
    private int levelNumber;

    private bool showGift;
    private float currGiftCount;
    private bool showGiftMultiplier;

    private bool showEnemy;
    private float currEnemyCount;
    private bool showEnemyMultiplier;

    private bool showBonus;
    private float currBonusScore;


    private bool showTotal;
    private int totalScore;

    private bool showButton;

    public string SceneName { get; set; }

    private string gameDataProjectFilePath = "highscore{0}.json";
    private bool onHighscoreList;

    // Use this for initialization
    void Start () {
        //InitializeRecap(2, 1, 10, 1, false, 65.3f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeRecap(int giftCount, int enemyCount, int sublevelScore, int levelNumber, bool winning, float time)
    {
        GameManager.instance.countTime = false;
        Player.Instance.Rbody.Sleep();
        winningSreen.SetActive(winning);
        dyingScreen.SetActive(!winning);

        this.levelTime = time;
        this.giftCount = giftCount;
        this.enemyCount = enemyCount;
        this.sublevelScore = sublevelScore;
        this.levelNumber = levelNumber;

        showGift = false;
        currGiftCount = -1f;
        showGiftMultiplier = false;

        showEnemy = false;
        currEnemyCount = -1f;
        showEnemyMultiplier = false;

        showBonus = false;
        currBonusScore = -1f;

        showTotal = false;
        totalScore = -1;

        showButton = false;
        StartCoroutine(ScrollNumbers());

        if (!winning)
        {
            StartCoroutine(DeathTransition());
        }

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
        if (winningSreen.activeSelf)
        {  
            GUI.Label(new Rect(0f, 200f, Screen.width, 30), string.Format("Level {0} Completed", levelNumber), headerStyle);
        }
        else if (dyingScreen.activeSelf)
        {
            Texture dyingTexture = dyingScreen.GetComponent<SpriteRenderer>().sprite.texture;
            GUI.Label(new Rect(0f, 150f, Screen.width, 30), "YOU DIED!", headerStyle);
            GUI.Label(new Rect(0f, 200f, Screen.width, 30), string.Format("Level {0} Incompleted", levelNumber), headerStyle);
            GUIStyle dyingStyle = new GUIStyle()
            {
                imagePosition = ImagePosition.ImageAbove,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(Screen.width / 2 - 370, 0f, 180, 180), dyingTexture, dyingStyle);
            GUI.Label(new Rect(Screen.width / 2 - 230, 0f, 180, 180), dyingTexture, dyingStyle);
            GUI.Label(new Rect(Screen.width/2 - 90, 0f, 180, 180), dyingTexture, dyingStyle);
            GUI.Label(new Rect(Screen.width / 2 + 30, 0f, 180, 180), dyingTexture, dyingStyle);
            GUI.Label(new Rect(Screen.width / 2 + 170, 0f, 180, 180), dyingTexture, dyingStyle);
        }

        if (showTime)
        {
            GUIStyle timeStyle = new GUIStyle()
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                richText = true,
                normal = new GUIStyleState()
                {
                    textColor = Color.white
                }
            };
            GUI.Label(new Rect(0f, 250f, Screen.width, 30), string.Format("{0} minutes and {1:F2} seconds", Mathf.Floor(levelTime / 60), levelTime % 60), timeStyle);


            if (showGift)
            {
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
                GUI.Label(new Rect(300f, 325f, 200, 30), "Gifts Collected:", giftStyle);
                if (currGiftCount >= 0)
                    GUI.Label(new Rect(600f, 325f, 200, 30), string.Format("{0}", currGiftCount), giftStyle);
                if (showGiftMultiplier)
                    GUI.Label(new Rect(625f, 325f, 200, 30), "x 100", giftStyle);

                if (showEnemy)
                {
                    GUIStyle enemyStyle = new GUIStyle()
                    {
                        fontSize = 24,
                        alignment = TextAnchor.MiddleLeft,
                        fontStyle = FontStyle.Bold,
                        richText = true,
                        normal = new GUIStyleState()
                        {
                            textColor = new Color(79f / 255f, 0f, 0f)
                        }
                    };
                    GUI.Label(new Rect(300f, 400f, 200, 30), "Enemies defeated:", enemyStyle);
                    if (currEnemyCount >= 0)
                        GUI.Label(new Rect(600f, 400f, 200, 30), string.Format("{0}", currEnemyCount), enemyStyle);
                    if (showEnemyMultiplier)
                        GUI.Label(new Rect(625f, 400f, 200, 30), "x 50", enemyStyle);

                    if (showBonus)
                    {
                        GUIStyle sublevelStyle = new GUIStyle()
                        {
                            fontSize = 24,
                            alignment = TextAnchor.MiddleLeft,
                            fontStyle = FontStyle.Bold,
                            richText = true,
                            normal = new GUIStyleState()
                            {
                                textColor = new Color(25f / 255, 159f / 255, 0f)
                            }
                        };
                        GUI.Label(new Rect(300f, 475f, 200, 30), "Hidden Level:", sublevelStyle);
                        if (currBonusScore >= 0)
                            GUI.Label(new Rect(600f, 475f, 200, 30), string.Format("{0}", currBonusScore), sublevelStyle);

                        if (showTotal)
                        {
                            GUIStyle totalStyle = new GUIStyle()
                            {
                                fontSize = 35,
                                alignment = TextAnchor.MiddleLeft,
                                fontStyle = FontStyle.Bold,
                                richText = true,
                                normal = new GUIStyleState()
                                {
                                    textColor = new Color(197f / 255f, 164f / 255f, 54f / 255f)
                                }
                            };
                            GUI.Label(new Rect(300f, 575f, 200, 30), "Total Score:", totalStyle);
                            if (totalScore >= 0)
                                GUI.Label(new Rect(600f, 575f, 200, 30), string.Format("{0}", totalScore), totalStyle);

                            if (showButton)
                            {
                                GUIStyle continueStyle = new GUIStyle("button")
                                {
                                    fontSize = 26
                                };

                                if (GUI.Button(new Rect(Screen.width / 2 - 75, 650f, 150, 60), "Continue", continueStyle))
                                {
                                    
                                    GameManager.instance.score = 0;
                                    GameManager.instance.EnemiesKilled = 0;

                                    if (dyingScreen.activeSelf)
                                    {
                                        GameManager.instance.health = 6;
                                        GameManager.instance.finalScore = 0;
                                    }
                                        
                                    SceneManager.LoadScene(SceneName);
                                }

                                //if (GUI.Button(new Rect(Screen.width * 3 / 4 - 150, 650f, 210, 60), "Submit Score", continueStyle))
                                //{
                                //    SceneManager.LoadScene("Finished");
                                //}

                                if (onHighscoreList)
                                {
                                    GUIStyle newHighStyle = new GUIStyle()
                                    {
                                        fontSize = 45,
                                        fontStyle = FontStyle.Bold,
                                        normal = new GUIStyleState()
                                        {
                                            textColor = Color.white
                                        }
                                    };
                                    GUI.Label(new Rect(685f, 575f, 200, 30), "Highscore", newHighStyle);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Death()
    {
        StartCoroutine(DeathTransition());
    }

    private IEnumerator ScrollNumbers()
    {
        yield return new WaitForSeconds(1.0f);
        showTime = true;

        yield return new WaitForSeconds(1.0f);

        showGift = true;
        yield return new WaitForSeconds(1.0f);
        float giftSpeed = 0.3f;
        if(giftCount > 10)
        {
            giftSpeed = 0.1f;
        }
        for (int i = 0; i <= giftCount; i++)
        {
            currGiftCount = i;
            yield return new WaitForSeconds(giftSpeed);
        }
        yield return new WaitForSeconds(0.25f);
        showGiftMultiplier = true;

        yield return new WaitForSeconds(1.0f);

        showEnemy = true;
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i <= enemyCount; i++)
        {
            currEnemyCount = i;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.25f);
        showEnemyMultiplier = true;

        yield return new WaitForSeconds(1.0f);

        showBonus = true;
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i <= sublevelScore; i++)
        {
            currBonusScore = i;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(1.0f);
        showTotal = true;

        yield return new WaitForSeconds(1.0f);
        totalScore = giftCount * 100 + enemyCount * 50 + sublevelScore;
        GameManager.instance.finalScore += totalScore;

        yield return new WaitForSeconds(1.0f);
        showButton = true;

        Highscore[] highscoreLevel = GetHighscoreLevel(levelNumber.ToString());
        SaveToJson(highscoreLevel, levelNumber.ToString(), totalScore);
        
        if (dyingScreen.activeSelf || levelNumber == 3)
        {
            bool newLevelHigh = onHighscoreList;
            Highscore[] highscoreAll = GetHighscoreLevel("");
            SaveToJson(highscoreAll, "", GameManager.instance.finalScore);
            onHighscoreList = onHighscoreList || newLevelHigh;
        }
    }

    private IEnumerator DeathTransition()
    {
        dyingScreen.GetComponent<Animator>().ResetTrigger("death");
        yield return new WaitForSeconds(1.0f);
        dyingScreen.GetComponent<Animator>().SetTrigger("idle");
        yield return new WaitForSeconds(1.0f);
        dyingScreen.GetComponent<Animator>().ResetTrigger("idle");
        dyingScreen.GetComponent<Animator>().SetTrigger("death");
    }

    private Highscore[] GetHighscoreLevel(string level)
    {
        string filename = string.Copy(gameDataProjectFilePath);
        string filePath = Path.Combine(Application.dataPath, string.Format(filename, level));
        
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            return JsonHelper.FromJson<Highscore>(dataAsJson);
        }
        return new Highscore[0];
    }

    public void SaveToJson(Highscore[] highscores, string level, int score)
    {
        string filename = string.Copy(gameDataProjectFilePath);
        if (highscores.Length > 0)
        {
            Highscore[] highScoreInstance = new Highscore[highscores.Length + 1];
            int count = 0;
            bool saved = false;
            onHighscoreList = false;
            foreach (Highscore highscore in highscores)
            {
                if (!saved && totalScore >= highscore.score)
                {
                    highScoreInstance[count] = new Highscore();
                    highScoreInstance[count].name = GameManager.instance.playerName;
                    highScoreInstance[count].score = score;

                    count++;
                    saved = true;
                    if (count <= 5)
                    {
                        onHighscoreList = true;
                    }
                    
                }

                highScoreInstance[count] = new Highscore();
                highScoreInstance[count].name = highscore.name;
                highScoreInstance[count].score = highscore.score;

                count++;
            }

            string highScoreToJson = JsonHelper.ToJson(highScoreInstance, true);
            string filePath = Path.Combine(Application.dataPath, string.Format(filename, level));

            File.WriteAllText(filePath, highScoreToJson);
        }
        else
        {
            Highscore[] highScoreInstance = new Highscore[1];

            highScoreInstance[0] = new Highscore();
            highScoreInstance[0].name = GameManager.instance.playerName;
            highScoreInstance[0].score = score;

            string highScoreToJson = JsonHelper.ToJson(highScoreInstance, true);
            string filePath = Path.Combine(Application.dataPath, string.Format(filename, level));
            onHighscoreList = true;
            File.WriteAllText(filePath, highScoreToJson);
        }
    }
}
