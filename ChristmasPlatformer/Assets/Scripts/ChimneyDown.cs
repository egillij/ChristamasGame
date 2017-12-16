using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ChimneyDown : MonoBehaviour
{
    BoxCollider2D ChimneyCollider;

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private Transform[] outPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask player;

    void Start()
    {
        ChimneyCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (IsInside())
        {
            Player.Instance.AllowedDown = true;
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Player.Instance.GoDown = true;
            }

            if (Player.Instance.GoDown && !ChimneyCollider.isTrigger)
            {
                ChimneyCollider.isTrigger = true;
                LibPDBinding.LibPD.SendBang("chimneyMagic");
                StartCoroutine(ChangeScene());                
            }
        }
        else
        {
            Player.Instance.AllowedDown = false;
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
        yield return new WaitForSeconds(0.75f);
        
        Player.Instance.GoDown = false;
        Player.Instance.AllowedDown = false;
        int levelNr = Convert.ToInt32(SceneManager.GetSceneAt(0).name.Split('l')[1]);
        GameManager.instance.levelLock[levelNr] = true;
        SceneManager.LoadScene("LevelScore", LoadSceneMode.Additive);
        while (!SceneManager.GetSceneAt(1).isLoaded)
        {
            yield return null;
        }
        
        LevelRecap.Instance.InitializeRecap(GameManager.instance.score, GameManager.instance.EnemiesKilled, GameManager.instance.bonusScore, levelNr, true, GameManager.instance.LevelDuration);
        LevelRecap.Instance.SceneName = sceneName;        
    }
}
