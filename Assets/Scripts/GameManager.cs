using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private TMP_Text scoreText;
    private TMP_Text coinText;
    private GameObject player;
    private int coin = 0;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("scoreText").GetComponent<TMP_Text>();
        scoreText.text = "0";

        coinText = GameObject.Find("coinText").GetComponent<TMP_Text>();
        coinText.text = "0";
        
        player = GameObject.Find("Player");
    }
    private void Update() {
        scoreText.text = player.transform.position.x.ToString("0");
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0;
        Destroy(player);
    }
    public void EarnCoin(int coinValue = 1)
    {
        coin += coinValue;
        coinText.text = coin.ToString();
    }
}
