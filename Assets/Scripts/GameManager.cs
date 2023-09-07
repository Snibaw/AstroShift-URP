using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("ShowWhenMissionFinished")]
    [SerializeField] private GameObject redDot;
    private GameObject[] missions;
    [SerializeField] private GameObject ShowWhenMissionFinished;
    [SerializeField] private GameObject missionMenu;
    private int testIndex = 0;
    private bool isMissionShown = false;

    [Header("Gameplay")]
    [SerializeField] private int maxScoreMultiplier = 30;
    [SerializeField] private GameObject highScoreFlagPrefab;
    [SerializeField] private GameObject[] removeWhenStart;
    [SerializeField] private GameObject[] showWhenStart;
    [SerializeField] private GameObject[] showWhenGameOver;
    [SerializeField] private float invincibleTimer = 1.5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject poufPrefab;
    [SerializeField] private float distanceBetweenBonus = 300f;
    [SerializeField] private GameObject quitPannel;
    [Header("Shop")]
    [SerializeField] private GameObject shopScrollRect;
    [SerializeField] private GameObject shopPannel;
    [SerializeField] private GameObject[] shopContent;
    [SerializeField] private GameObject pauseMenu;
    private GPGSManager gpgsManager;
    private float lastBonusSpawnedPosition = 0f;
    private TMP_Text scoreMultiplierDisplayText;
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
    private BonusContainer bonusContainer;
    private bool isShieldTimeActive = false;
    private bool isShieldOnceActive = false;
    private bool isInvincible = false;
    public bool canSpawnBonus = true;
    public bool canSpawnScoreMultiplier = true;
    private bool canUpdateScoreMultiplier = true;

    [Header("Music")]
    [SerializeField] private GameObject crossMusicImage;
    [SerializeField] private GameObject crossSoundImage;
    private AudioManager audioManager;
    private bool isMusicOn = true;
    private bool isSoundEffectOn = true;

    [Header("Sounds")]
    [SerializeField] private AudioClip clickButtonSound;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Screen.SetResolution(1280,720,true);
        // Camera.main.aspect = 960/640f;

        missions = GameObject.FindGameObjectsWithTag("Mission");

        redDot.SetActive(false);
        foreach(GameObject mission in missions)
        {
            if(CheckIfMissionIsFinished(mission.name))
            {
                redDot.SetActive(true);
                break;
            }
        }


        player = GameObject.Find("Player");
        startSpeed = player.GetComponent<PlayerMovement>().speed;
        player.GetComponent<PlayerMovement>().canMove = false;

        if(PlayerPrefs.GetInt("scoreMultiplier", 0) >= maxScoreMultiplier)
        {
            canUpdateScoreMultiplier = false;
        }
        gpgsManager = GameObject.Find("GPGSManager").GetComponent<GPGSManager>();

        

        bonusContainer = GameObject.Find("BonusContainer").GetComponent<BonusContainer>();

        scoreMultiplier = PlayerPrefs.GetInt("scoreMultiplier", 0);
        scoreMultiplierText = GameObject.Find("ScoreMultiplierText").GetComponent<TMP_Text>();
        scoreMultiplierText.text = scoreMultiplier.ToString("00");

        scoreMultiplierDisplayText = GameObject.Find("ScoreMultiplierDisplayText").GetComponent<TMP_Text>();
        scoreMultiplierDisplayText.text = "x"+Mathf.Min(30,(scoreMultiplier)).ToString("00");

        ratioMultiplierText = GameObject.Find("RatioMultiplier").GetComponent<TMP_Text>();
        ratioMultiplierText.text = (scoreMultiplier).ToString("00")+"/"+maxScoreMultiplier.ToString("00");

        backGroundScoreMultiplier = GameObject.Find("BackGroundScoreMultiplier");
        backGroundScoreMultiplier.SetActive(false);



        scoreText = GameObject.Find("scoreText").GetComponent<TMP_Text>();
        scoreText.text = "0";

        coinText = GameObject.Find("coinText").GetComponent<TMP_Text>();
        coinText.text = PlayerPrefs.GetString("Coin", "0");

        highScoreText = GameObject.Find("highScoreText").GetComponent<TMP_Text>();
        highScore = PlayerPrefs.GetInt("highScore", 0);
        highScoreText.text = highScore.ToString("0");
        if(highScoreText.text != "0")
        {
            Instantiate(highScoreFlagPrefab, new Vector3(highScore, -3.38f, 0), Quaternion.identity);
        }


        foreach(GameObject obj in showWhenGameOver)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in showWhenStart)
        {
            obj.SetActive(false);
        }
        isStarted = false;
        
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        isMusicOn = PlayerPrefs.GetInt("musicOn", 1) == 1 ? true : false;
        SetMusic();

        isSoundEffectOn = PlayerPrefs.GetInt("soundEffectOn", 1) == 1 ? true : false;
        SetSoundEffect();

        quitPannel.SetActive(false);

        ShowContent(0);
        shopPannel.SetActive(false);

        pauseMenu.SetActive(false);
        missionMenu.SetActive(false);
    }
    private void FixedUpdate() {
        if(isStarted) scoreText.text = (player.transform.position.x*(1+scoreMultiplier)).ToString("0") ;

        if(lastBonusSpawnedPosition + distanceBetweenBonus < player.transform.position.x)
        {
            canSpawnBonus = true;
        }
        if(Application.isEditor)
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                // PlayerPrefs.SetString("Coin", (long.Parse(PlayerPrefs.GetString("Coin", "0")) + 10000000).ToString());
                // coinText.text = PlayerPrefs.GetString("Coin", "0");
                TestingMissions(missions[testIndex]);
                testIndex++;
            }
        }

    }
    public void ShowPauseMenu()
    {
        PlayButtonSound();
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    public void ClosePauseMenu()
    {
        PlayButtonSound();
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void BonusHasBeenSpawned()
    {
        if( player == null) player = GameObject.Find("Player");
        lastBonusSpawnedPosition = player.transform.position.x;
        canSpawnBonus = false;
    }

    public void PlayerTakeDamage()
    {
        if(isInvincible) return;
        if(isShieldOnceActive)
        {
            isShieldOnceActive = false;
            bonusContainer.RemoveElement(0);
            StartCoroutine(PlayerInvincible());
            GameObject Explosion = Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
            Destroy(Explosion, 1.5f);
            DestroyEveryObstacles();
            return;
        }
        else if(isShieldTimeActive)
        {
            isShieldTimeActive = false;
            bonusContainer.RemoveElement(1);
            StartCoroutine(PlayerInvincible());
            GameObject Explosion = Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
            Destroy(Explosion, 1.5f);
            DestroyEveryObstacles();
            return;
        }
        else
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        gpgsManager.ReportHighScore(int.Parse(scoreText.text));
        if(int.Parse(scoreText.text) > highScore)
        {
            PlayerPrefs.SetInt("highScore", int.Parse(scoreText.text));
        }
        Time.timeScale = 0;
        foreach(GameObject obj in showWhenGameOver)
        {
            obj.SetActive(true);
        }
        PlayerPrefs.SetString("Coin", (long.Parse(PlayerPrefs.GetString("Coin", "0")) + Mathf.Ceil(coin*PlayerPrefs.GetFloat("IT_MoneyMultiplierValue",1f))).ToString());

        AddValueToPlayerPrefs("M_Play", 1);
        AddValueToPlayerPrefs("M_Score", int.Parse(scoreText.text));
    }
    public void EarnCoin(int coinValue = 1)
    {
        coin += coinValue;
        coinText.text = coin.ToString();

        AddValueToPlayerPrefs("M_Coin", coinValue);
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

        player.GetComponent<PlayerMovement>().canMove = true;

        coinText.text = "0";
    }
    public void RestartGame()
    {
        PlayButtonSound();
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void OpenQuitPannel()
    {
        PlayButtonSound();
        quitPannel.SetActive(true);
    }
    public void CloseQuitPannel()
    {
        PlayButtonSound();
        quitPannel.SetActive(false);
    }
    public void QuitGame()
    {
        PlayButtonSound();
        Application.Quit();
    }
    public void PickScoreMultiplier()
    {
        if(scoreMultiplier >= maxScoreMultiplier) return;
        StartCoroutine(AddScoreMultiplier());
        AddValueToPlayerPrefs("M_Free", 1);
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
        scoreMultiplierDisplayText.text = "x"+Mathf.Min(30,(scoreMultiplier)).ToString("00");
        yield return new WaitForSeconds(3f);
        backGroundScoreMultiplier.GetComponent<Animator>().SetTrigger("Off");

    }
    public IEnumerator PickUpBonus(int bonusIndex, BonusBehaviour bonus)
    {
        if(bonus != null) bonus.GetComponent<BonusBehaviour>().PickUpBonusCoroutine();
        yield return new WaitForSeconds(0.2f);
        //Add bonus in UI (top left corner of the screen)
        bonusContainer.AddBonusElement(bonusIndex);
        //Add bonus in boolean to avoid death when hit
        if(bonusIndex == 0) isShieldOnceActive = true;
        else if(bonusIndex == 1) isShieldTimeActive = true;

        AddValueToPlayerPrefs("M_Bonus", 1);
    }
    public void DesactivateShieldBonus(int bonusIndex)
    {
        if(bonusIndex == 0) isShieldOnceActive = false;
        else if(bonusIndex == 1) isShieldTimeActive = false;
    }
    private void DestroyEveryObstacles()
    {
        //DestroyChainObstacles();
        GameObject[] chainObstacles = GameObject.FindGameObjectsWithTag("Chain");
        foreach(GameObject obstacle in chainObstacles)
        {
            
            DoAnimationSuspendedWall(obstacle.transform.parent.gameObject, giveBonus: true);
        }

        //Search obstacles where the first 11 caracters are : "Square Group"
        GameObject[] squareObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach(GameObject obstacle in squareObstacles)
        {
            DestroySquareGroup(obstacle);
        }

        //Destroy Laser Obstacles
        GameObject[] laserObstacles = GameObject.FindGameObjectsWithTag("Laser");
        foreach(GameObject obstacle in laserObstacles)
        {
            Destroy(obstacle);
        }
    }
    public void DestroySquareGroup(GameObject obstacle = null)
    {
        if(obstacle.name.Length >= 11 && obstacle.name.Substring(0,11) == "SquareGroup")
        {
            Instantiate(poufPrefab, obstacle.transform.position, Quaternion.identity);
            Destroy(obstacle);
        }
    }
    public void DoAnimationSuspendedWall(GameObject parentObject, bool giveBonus = true)
    {
        //Parent is SuspendedWall2
        SpikeObstacleSpawnCoin SquarePartComponent = parentObject.transform.GetChild(0).GetComponent<SpikeObstacleSpawnCoin>();

        if(SquarePartComponent.HasBeenDestroyed) return;
        SquarePartComponent.HasBeenDestroyed = true;

        if(!SquarePartComponent.HasBonusBeenPickedUp) 
        {
            SquarePartComponent.CheckIfBonus();
        }
        else if(!SquarePartComponent.HasScoreMultiplierBeenPickedUp)
        {
            PickScoreMultiplier();
            SquarePartComponent.HasScoreMultiplierBeenPickedUp = true;
        }
        foreach(Rigidbody2D rb2D in parentObject.GetComponentsInChildren<Rigidbody2D>())
        {
            if(rb2D.gameObject.name == "Triangle") rb2D.gameObject.GetComponent<SpikeObstacle>().isDestroyed = true;

            rb2D.constraints = RigidbodyConstraints2D.None;
            rb2D.gravityScale = 1;
            //Add random force to every object
            rb2D.AddForce(new Vector2(Random.Range(-5f,5f),Random.Range(-5f,5f)),ForceMode2D.Impulse);
            //Add force to make them rotate
            rb2D.AddTorque(Random.Range(-2f,2f),ForceMode2D.Impulse);
            Destroy(rb2D.gameObject, 1f);
        }
    }
    private IEnumerator PlayerInvincible()
    {
        isInvincible = true;
        //Player can't attack (create bugs)
        player.GetComponent<PlayerAttack>().isAttacking = true;

        float massTempo = player.GetComponent<Rigidbody2D>().mass;
        player.GetComponent<Rigidbody2D>().mass = 10000;
        for(int i=0; i<2; i++)
        {
            player.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.1f);
            yield return new WaitForSeconds(invincibleTimer/4);
            player.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
            yield return new WaitForSeconds(invincibleTimer/4);
        }
        player.GetComponent<Rigidbody2D>().mass = massTempo;
        isInvincible = false;
        player.GetComponent<PlayerAttack>().isAttacking = false;
        
    }
    public void PlayButtonSound()
    {
        audioManager.PlaySoundEffect(clickButtonSound);
    }
    public void ClickOnMusicButton()
    {
        
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("musicOn", isMusicOn ? 1 : 0);
        SetMusic();
        PlayButtonSound();
    }
    private void SetMusic()
    {
        crossMusicImage.SetActive(!isMusicOn);

        if(isMusicOn)
        {
            if(!audioManager.isPlaying)
            {
                audioManager.OnOffMusic(true);
            }
            
        }
        else
        {
            audioManager.OnOffMusic(false);
        }
    }
    public void ClickOnSoundEffectButton()
    {
        
        isSoundEffectOn = !isSoundEffectOn;
        PlayerPrefs.SetInt("soundEffectOn", isSoundEffectOn ? 1 : 0);
        SetSoundEffect();
        PlayButtonSound();
    }
    private void SetSoundEffect()
    {
        audioManager.isSoundEffectOn = isSoundEffectOn;
        crossSoundImage.SetActive(!isSoundEffectOn);
    }
    public void OpenShopPannel()
    {
        PlayButtonSound();
        

        shopPannel.SetActive(true);
        shopPannel.GetComponent<Animator>().SetTrigger("Open");
    }
    public void CloseShopPannel()
    {
        PlayButtonSound();
        shopPannel.GetComponent<Animator>().SetTrigger("Close");
        StartCoroutine(DesactiveShopPannelAfterAnimation(shopPannel, 0.75f));
    }
    public void ShowMissionMenu()
    {
        PlayButtonSound();

        missionMenu.SetActive(true);
        missionMenu.GetComponent<Animator>().SetTrigger("Open");
    }
    public void CloseMissionMenu()
    {
        PlayButtonSound();
        missionMenu.GetComponent<Animator>().SetTrigger("Close");
        StartCoroutine(DesactiveShopPannelAfterAnimation(missionMenu, 0.75f));

    }
    private IEnumerator DesactiveShopPannelAfterAnimation(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
    public void ShowContent(int i)
    {
        PlayButtonSound();
        foreach(GameObject obj in shopContent)
        {
            obj.SetActive(false);
        }
        shopContent[i].SetActive(true);
        shopScrollRect.GetComponent<UnityEngine.UI.ScrollRect>().content = shopContent[i].GetComponent<RectTransform>();
    }
    public bool BuyUpgrade(long price)
    {
        long totalCoin = long.Parse(coinText.text);
        if(totalCoin >= price)
        {
            AddValueToPlayerPrefs("M_BuyUpgrade", 1);
            totalCoin -= price;
            coinText.text = totalCoin.ToString();
            PlayerPrefs.SetString("Coin", totalCoin.ToString());
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AddValueToPlayerPrefs(string key, int value)
    {
        bool isAlreadyFinished = CheckIfMissionIsFinished(key);

        PlayerPrefs.SetInt(key+"Value", PlayerPrefs.GetInt(key+"Value", 0) + value);

        if(!isAlreadyFinished)
        {
            if(CheckIfMissionIsFinished(key))
            {
                redDot.SetActive(true);
                foreach(GameObject mission in missions)
                {
                    if(mission.name == key)
                    {
                        if(isMissionShown)
                            StartCoroutine(WaitBeforeSpawnMissionOnScreen(mission));
                        else
                            StartCoroutine(SpawnMissionOnScreen(mission));
                    }
                }
            }
        }
    }
    private void TestingMissions(GameObject mission)
    {
        if(isMissionShown)
            StartCoroutine(WaitBeforeSpawnMissionOnScreen(mission));
        else
            StartCoroutine(SpawnMissionOnScreen(mission));
    }
    private IEnumerator WaitBeforeSpawnMissionOnScreen(GameObject mission)
    {
        while(isMissionShown)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(SpawnMissionOnScreen(mission));
    }
    private IEnumerator SpawnMissionOnScreen(GameObject mission)
    {
        isMissionShown = true;
        GameObject missionObject = Instantiate(mission, ShowWhenMissionFinished.transform);
        missionObject.transform.position += new Vector3(0,120,0);
        missionObject.SetActive(true);
        missionObject.transform.parent = ShowWhenMissionFinished.transform;
        missionObject.transform.GetChild(3).GetComponent<UnityEngine.UI.Button>().enabled = false;
        Destroy(missionObject,2.5f);
        ShowWhenMissionFinished.GetComponent<Animator>().SetTrigger("Show");
        yield return new WaitForSeconds(2.5f);
        isMissionShown = false;
    }
    private bool CheckIfMissionIsFinished(string key)
    {
        // Debug.Log("StartValue : " + PlayerPrefs.GetInt(key + "StartValue", 0) + " + Value : " + PlayerPrefs.GetInt(key + "Value", 0) + " >= MaxValue : " + PlayerPrefs.GetInt(key + "MaxValue", 0));
        return PlayerPrefs.GetInt(key + "StartValue", 0) + PlayerPrefs.GetInt(key + "Value", 0) >= PlayerPrefs.GetInt(key + "MaxValue", 0);
    }
    public void ReportABug()
    {
        PlayButtonSound();
        Application.OpenURL("mailto:snibaw@gmail.com");
    }
}
