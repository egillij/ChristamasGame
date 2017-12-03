using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSublevel: MonoBehaviour {

    public string sublevelName;

    public float levelDifficulty;
    public float levelDuration;

    private bool sceneLoaded = false;
    private bool sceneIsLoading = false;
    private bool isSublevelOpen;
    private bool colorToGreen = true;
	// Use this for initialization
	void Start () {
        isSublevelOpen = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (isSublevelOpen)
        {
            //Make the chimney glow
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            Color newColor = spriteRenderer.color;
            
            if (colorToGreen)
            {
                newColor.r -= 25;
                newColor.b -= 25;
            }
            else
            {
                newColor.r += 25;
                newColor.b += 25;
            }

            if (newColor.r <= 0)
            {
                colorToGreen = false;
            }
            if(newColor.r >= 255)
            {
                colorToGreen = true;
            }

            spriteRenderer.color = newColor;
        }


        if (sceneIsLoading)
        {
            if (SceneManager.GetSceneAt(1).isLoaded)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
                CaptureGame captureObject = GameObject.FindObjectOfType<CaptureGame>();
                captureObject.playerReturnPosition = Player.Instance.gameObject.transform.position;

                captureObject.StartNew(levelDifficulty, levelDuration);


                GameObject[] fallingObjects = SceneManager.GetSceneAt(1).GetRootGameObjects();
                foreach (GameObject obj in fallingObjects)
                {
                    if (obj.tag == "PlayerStart")
                    {
                        Player.Instance.gameObject.transform.position = obj.transform.position;
                    }
                }
                sceneLoaded = true;
                sceneIsLoading = false;
                isSublevelOpen = false;
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !sceneIsLoading && isSublevelOpen)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SceneManager.LoadScene(sublevelName, LoadSceneMode.Additive);
                sceneIsLoading = true;
            }
        }
    }
}
