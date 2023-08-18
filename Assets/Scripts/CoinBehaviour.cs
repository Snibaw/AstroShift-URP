using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    private bool isMovingTowardsPlayer = false;
    private float speed;
    private GameObject player;

    private void FixedUpdate() {
        if(isMovingTowardsPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            speed += 0.3f;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().EarnCoin();
            Destroy(gameObject,0.1f);
        }
    }
    public void MoveTowardsPlayer()
    {
        if(isMovingTowardsPlayer) return;
        isMovingTowardsPlayer = true;
        player = GameObject.Find("Player");
        speed = player.GetComponent<PlayerMovement>().speed/2;
    }
}
