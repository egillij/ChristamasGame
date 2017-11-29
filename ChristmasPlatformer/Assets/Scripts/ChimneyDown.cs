using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChimneyDown : MonoBehaviour
{

    [SerializeField]
    private string sceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //LibPD.SendFloat("banger", 1f);
            //LibPD.SendBang("banger");

            SceneManager.LoadScene(sceneName);
        }
    }
}
