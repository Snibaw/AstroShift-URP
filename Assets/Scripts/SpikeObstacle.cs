using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObstacle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Player died");
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
    }
}
