using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxScoreMultiplier = 30;
    [SerializeField] private GameObject highScoreFlagPrefab;
    [SerializeField] private GameObject[] removeWhenStart;
    [SerializeField] private GameObject[] showWhenStart;
    [SerializeField] private GameObject[] showWhenGameOver;
    private TMP_Text scoreMultiplierText;
    private TMP_Text ratioMultiplierText;
    private GameObject backGroundScoreMultiplier;
    public int scoreMultiplier = 0;
    private TMP_Text scoreText;
    private TMP_Text coinText;
    private TMP_Text highScoreText;
    private int highScore;
    private GameObject player;
    private int coin = 0;
    private bool isStarted;
    private float startSpeed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        scoreMultiplier = PlayerPrefs.GetInt("scoreMultiplier", 0);
        scoreMultiplierText = GameObject.Find("ScoreMultiplierText").GetComponent<TMP_Text>();
        scoreMultiplierText.text = scoreMultiplier.ToString("00");

        ratioMultiplierText = GameObject.Find("RatioMultiplier").GetComponent<TMP_Text>();
        ratioMultiplierText.text = (scoreMultiplier).ToString("00")+"/"+maxScoreMultiplier.ToString("00");

        backGroundScoreMultiplier = GameObject.Find("BackGroundScoreMultiplier");
        backGroundScoreMultiplier.SetActive(false);



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
        startSpeed = player.GetComponent<PlayerMovement>().speed;


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
        if(isStarted) scoreText.text = (player.transform.position.x*scoreMultiplier).ToString("0") ;
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
        player.GetComponent<PlayerMovement>().speed = startSpeed;
        Camera.main.transform.position = new Vector3(2, 0, -10);
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void PickScoreMultiplier()
    {
        if(scoreMultiplier >= maxScoreMultiplier) return;
        StartCoroutine(AddScoreMultiplier());
    }
    private IEnumerator AddScoreMultiplier()
    {
        scoreMultiplier++;
        PlayerPrefs.SetInt("scoreMultiplier", scoreMultiplier);
        yield return new WaitForSeconds(1f);
        backGroundScoreMultiplier.SetActive(true);
        backGroundScoreMultiplier.GetComponent<Animator>().SetTrigger("On");
        yield return new WaitForSeconds(2f);
        ratioMultiplierText.gameObject.GetComponent<Animator>().SetTrigger("On");
        yield return new WaitForSeconds(0.1f);
        ratioMultiplierText.text = (scoreMultiplier).ToString("00")+"/"+maxScoreMultiplier.ToString("00");
        yield return new WaitForSeconds(0.2f);
        scoreMultiplierText.gameObject.GetComponent<Animator>().SetTrigger("On");
        yield return new WaitForSeconds(0.3f);
        scoreMultiplierText.text = scoreMultiplier.ToString("00");
        yield return new WaitForSeconds(3f);
        backGroundScoreMultiplier.GetComponent<Animator>().SetTrigger("Off");
    }
}
