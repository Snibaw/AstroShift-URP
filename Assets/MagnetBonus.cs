using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBonus : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Coin")
        {
            other.gameObject.GetComponent<CoinBehaviour>().MoveTowardsPlayer();
        }
    }
}
