using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSublevel: MonoBehaviour {

    public string sublevelName;

    public float levelDifficulty;
    public float levelDuration;
    

    [SerializeField]
    private Transform[] outPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask player;

    private bool sceneIsLoading = false;
    private bool isPlayerDown;
    private bool isSublevelOpen;
    private bool colorToGreen = true;
    private Vector3 returnPos;

    private BoxCollider2D ChimneyCollider;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {
        ChimneyCollider = GetComponent<BoxCollider2D>();
        isSublevelOpen = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isSublevelOpen)
        {
            //Make the chimney blink
            
            Color newColor = spriteRenderer.color;
            
            if (colorToGreen)
            {
                newColor.r -= 25;
                newColor.b -= 25;
            }
            else
            {
                newColor.r += 10;
                newColor.b += 10;
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
            if (isPlayerDown)
            {
                if (SceneManager.GetSceneAt(1).isLoaded)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
                    CaptureGame captureObject = GameObject.FindObjectOfType<CaptureGame>();
                    captureObject.playerReturnPosition = returnPos;
                    captureObject.StartNew(levelDifficulty, levelDuration);

                    GameObject[] fallingObjects = SceneManager.GetSceneAt(1).GetRootGameObjects();
                    foreach (GameObject obj in fallingObjects)
                    {
                        if (obj.tag == "PlayerStart")
                        {
                            Player.Instance.gameObject.transform.position = obj.transform.position;
                        }
                    }
                    sceneIsLoading = false;
                    isSublevelOpen = false;
                    ChimneyCollider.enabled = true;
                }
            }
            
        }
        else
        {
            if (IsInside())//Input.GetKeyDown(KeyCode.S))
            {
                
                Player.Instance.AllowedDown = true;
                Player.Instance.PlayerDown();
                if (Player.Instance.GoDown)
                {
                    returnPos = Player.Instance.gameObject.transform.position;
                    ChimneyCollider.enabled = false;
                    sceneIsLoading = true;
                    StartCoroutine(ChangeScene());
                    
                }

            }
            else
            {
                Player.Instance.AllowedDown = false;
            }

        }

    }

    private bool IsInside()
    {
        int counter = 0;

        foreach (Transform point in outPoints)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, player);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    counter++;
                }
            }
        }

        return (counter == 2);
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.3f);

        Player.Instance.GoDown = false;
        Player.Instance.AllowedDown = false;
        isPlayerDown = true;
        spriteRenderer.color = new Color(255, 255, 255, 255);
        Player.Instance.movementSpeed = 10f;
        SceneManager.LoadScene(sublevelName, LoadSceneMode.Additive);



    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player" && !sceneIsLoading && isSublevelOpen)
    //    {

    //    }
    //}
}
