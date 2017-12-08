using System.Collections;
using System.Collections.Generic;
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

    // Use this for initialization
	void Start () {
        InitializeRecap(2, 1, 10, 1, true, 65.3f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeRecap(int giftCount, int enemyCount, int sublevelScore, int levelNumber, bool winning, float time)
    {
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

                                if (GUI.Button(new Rect(Screen.width / 4 - 60, 650f, 150, 60), "Continue", continueStyle))
                                {
                                    SceneManager.LoadScene(SceneName);
                                }

                                if (GUI.Button(new Rect(Screen.width * 3 / 4 - 150, 650f, 210, 60), "Submit Score", continueStyle))
                                {
                                    SceneManager.LoadScene("Finished");
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
        for (int i = 0; i <= giftCount; i++)
        {
            currGiftCount = i;
            yield return new WaitForSeconds(0.75f);
        }
        yield return new WaitForSeconds(0.25f);
        showGiftMultiplier = true;

        yield return new WaitForSeconds(1.0f);

        showEnemy = true;
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i <= enemyCount; i++)
        {
            currEnemyCount = i;
            yield return new WaitForSeconds(0.75f);
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
        //GameManager.instance.finalScore = totalScore;

        yield return new WaitForSeconds(1.0f);
        showButton = true;
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
}
