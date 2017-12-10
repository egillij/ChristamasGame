using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hell : MonoBehaviour
{
    [SerializeField]
    private Vector2 startPoint;
        
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(ChangeScene());
        }        
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.5f);

        Player.Instance.Rbody.MovePosition(startPoint);     
    }
}
