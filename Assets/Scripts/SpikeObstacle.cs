using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObstacle : MonoBehaviour
{
    public bool isDestroyed = false;
    private void OnCollisionEnter2D(Collision2D other) {
        if(isDestroyed) return;
        if (other.gameObject.CompareTag("Player")) {
            GameObject.Find("GameManager").GetComponent<GameManager>().PlayerTakeDamage();
        }
    }
}
