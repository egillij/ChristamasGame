using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour {

    [SerializeField]
    private string sceneName;

    private float playerEnterTime;
    private bool playerCanEnter;

    void Update () {
        if (playerCanEnter && Time.time > playerEnterTime + 2.0f)
        {
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.75f);

        Player.Instance.GoDown = false;
        Player.Instance.AllowedDown = false;

        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerCanEnter = true;
            playerEnterTime = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerCanEnter = false;
        }
    }
}
