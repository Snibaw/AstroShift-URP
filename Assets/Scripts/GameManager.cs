using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject highScoreFlagPrefab;
    [SerializeField] private GameObject[] removeWhenStart;
    [SerializeField] private GameObject[] showWhenStart;
    [SerializeField] private GameObject[] showWhenGameOver;
    private TMP_Text scoreText;
    private TMP_Text coinText;
    private TMP_Text highScoreText;
    private int highScore;
    private GameObject player;
    private int coin = 0;
    private bool isStarted;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("scoreText").GetComponent<TMP_Text>();
        scoreText.text = "0";

        coinText = GameObject.Find("coinText").GetComponent<TMP_Text>();
        coinText.text = "0";

        highScoreText = GameObject.Find("highScoreText").GetComponent<TMP_Text>();
        highScore = PlayerPrefs.GetInt("highScore", 0);
        highScoreText.text = highScore.ToString("0");
        if(highScoreText.text != "0")
        {
            Instantiate(highScoreFlagPrefab, new Vector3(highScore, -3.38f, 0), Quaternion.identity);
        }
        
        player = GameObject.Find("Player");


        foreach(GameObject obj in showWhenGameOver)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in showWhenStart)
        {
            obj.SetActive(false);
        }
        isStarted = false;
    }
    private void Update() {
        if(isStarted) scoreText.text = player.transform.position.x.ToString("0");
    }

    public void GameOver()
    {
        if(int.Parse(scoreText.text) > highScore)
        {
            PlayerPrefs.SetInt("highScore", int.Parse(scoreText.text));
        }
        Time.timeScale = 0;
        foreach(GameObject obj in showWhenGameOver)
        {
            obj.SetActive(true);
        }
    }
    public void EarnCoin(int coinValue = 1)
    {
        coin += coinValue;
        coinText.text = coin.ToString();
    }
    public void StartGame()
    {
        isStarted = true;
        foreach(GameObject obj in removeWhenStart)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in showWhenStart)
        {
            obj.SetActive(true);
        }
        player.transform.position = new Vector3(0, -4f, 0);
        Camera.main.transform.position = new Vector3(2, 0, -10);
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
