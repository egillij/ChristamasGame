﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.score += 10;
            Destroy(gameObject);
        }
    }
}
