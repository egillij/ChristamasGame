using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Debug.Log("inni");
            Player.Instance.AllowedDown = true;

            if (Player.Instance.GoDown)
            {
                ChimneyCollider.enabled = false;
                StartCoroutine(ChangeScene());
                //LibPD.SendFloat("banger", 1f);
                //LibPD.SendBang("banger");
            }
        }
        else
        {
            Debug.Log("Uti");
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

        SceneManager.LoadScene(sceneName);
    }
}
