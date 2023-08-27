using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBonus : MonoBehaviour
{
    private float shieldRange;
    private void Start() {
        shieldRange = PlayerPrefs.GetFloat("MA_RangeValue",0f);

        transform.localScale = new Vector3(shieldRange,shieldRange,1);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Coin")
        {
            other.gameObject.GetComponent<CoinBehaviour>().MoveTowardsPlayer();
        }
    }
}
