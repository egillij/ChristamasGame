using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CaptureGame : MonoBehaviour {

    public GameObject gift;
    public GameObject bomb;

    public float levelDuration = 10.0f;
    public float levelDifficulty = 1f;

    public float score;

    public Vector3 playerReturnPosition;

    [SerializeField]
    private Text startingText;
    [SerializeField]
    private Text closingText;

    private float levelStart;
    private float spawnRate;

    private float lastSpawn;

    private float badRate;
	// Use this for initialization
	void Start () {
        levelStart = Time.time;
        spawnRate = 3.0f/levelDifficulty;
        badRate = 0.5f * Mathf.Exp(-1f / levelDifficulty);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > 4f + levelStart)
        {
            startingText.enabled = false;
            if (Time.time >= levelStart + levelDuration)
            {
                //Player.Instance.Sleeping(3.0f);
                closingText.enabled = true;

                if (Time.time >= levelStart + levelDuration + 3.0f)
                {
                    closingText.enabled = false;
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
                    GameObject.FindGameObjectWithTag("Player").transform.position = playerReturnPosition;
                    //Freeze player for 2 or 3 seconds
                    Player.Instance.Sleeping(1.0f);
                    Player.Instance.BonusScore += score;
                    SceneManager.UnloadSceneAsync(this.gameObject.scene);
                }

            }
            else
            {
                if (Time.time > lastSpawn + spawnRate)
                {
                    lastSpawn = Time.time;
                    float spawnType = Random.value;

                    if (Random.value <= badRate)
                    {
                        //Spawn bomb
                        Debug.Log("Bomb");
                    }
                    else
                    {
                        Camera cam = this.gameObject.GetComponent<Camera>();
                        var vertExtent = cam.orthographicSize;
                        var horzExtent = cam.orthographicSize * Screen.width / Screen.height;
                        Vector3 position = new Vector3((Random.value * 2 - 1) * horzExtent + this.transform.position.x, vertExtent + this.transform.position.y, 0.0f);
                        GameObject newGift = Instantiate(gift, position, new Quaternion()) as GameObject;
                        switch(levelDifficulty.ToString())
                        {
                            case "1":
                                newGift.GetComponent<Rigidbody2D>().gravityScale = 0.4f;
                                break;

                            case "2":
                                newGift.GetComponent<Rigidbody2D>().gravityScale = 0.6f;
                                break;

                            case "3":

                                float scale = Random.Range(0.4f, 1.0f);
                                newGift.GetComponent<Rigidbody2D>().gravityScale = scale;
                                break;
                        }
                        
                    }
                }
            }
        }
        else
        {
            startingText.enabled = true;
            startingText.text = string.Format("{0:F0}", Mathf.Ceil(3.0f -(Time.time-levelStart)));
        }
	}

    public void StartNew(float diff, float dur)
    {
        levelDifficulty = diff;
        levelDuration = dur;
        levelStart = Time.time;
        spawnRate = 3.0f / levelDifficulty;
        badRate = 0.5f * Mathf.Exp(-1f / levelDifficulty);
    }
}
