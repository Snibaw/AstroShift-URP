using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObstacleSpawnCoin : MonoBehaviour
{
    public bool isSuspended = false;
    [SerializeField] private GameObject[] coinPrefab;
    [SerializeField] private float[] coinTransformY;
    [SerializeField] private GameObject monsterScoreMultiplier;
    private float probabilityToSpawnMonsterScoreMultiplier;
    private float probabilityToSpawnBonus;
    [SerializeField] private GameObject[] bonusList;
    [SerializeField] private GameObject bonusParent;
    private int bonusSpawnedIndex = 0;
    public bool canSpawnBonus = false;
    private BonusBehaviour currentBonusBehaviour;
    private bool doCurrentBonusMoveToPlayer = false;
    private bool isBonusSpawned = false;
    private GameManager gameManager;

    public bool HasScoreMultiplierBeenPickedUp = true;  
    public bool HasBonusBeenPickedUp = true;
    public bool HasBeenDestroyed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        probabilityToSpawnMonsterScoreMultiplier = PlayerPrefs.GetFloat("SC_SpawnRateValue",0f);
        probabilityToSpawnBonus = PlayerPrefs.GetFloat("IT_SpawnRateValue",0f);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(canSpawnBonus)
        {
            SpawnBonus();
            if(gameManager.canSpawnScoreMultiplier) SpawnMonsterScoreMultiplier();
        }
            

        if(Random.Range(0,3) == 0)
            return;
        if(isSuspended)
            SpawnCoinOnTop(coinTransformY[0]);
        else
        {
            if(transform.position.y < 0)
                SpawnCoinOnTop(coinTransformY[1]);
            else
                SpawnCoinOnBottom(coinTransformY[2]);
        }

    }
    private void SpawnBonus()
    {
        float probability = gameManager.canSpawnBonus ? probabilityToSpawnBonus*3 : probabilityToSpawnBonus;
        if(Random.Range(0,100) < probability)
        {
            
            gameManager.BonusHasBeenSpawned();
            bonusParent.SetActive(true);
            bonusSpawnedIndex = Random.Range(0,bonusList.Length);
            foreach(GameObject bonus in bonusList)
            {
                bonus.SetActive(false);
            }
            bonusList[bonusSpawnedIndex].SetActive(true);
            currentBonusBehaviour = bonusList[bonusSpawnedIndex].GetComponent<BonusBehaviour>();
            isBonusSpawned = true;
            HasBonusBeenPickedUp = false;
        }
        else 
        {
            bonusParent.SetActive(false);
        }
    }
    private void SpawnMonsterScoreMultiplier()
    {
        if(PlayerPrefs.GetInt("scoreMultiplier", 0) >= 30) return;

        if(Random.Range(0,100) < probabilityToSpawnMonsterScoreMultiplier)
            {
                gameManager.canSpawnScoreMultiplier = false;
                monsterScoreMultiplier.SetActive(true);
                HasScoreMultiplierBeenPickedUp = false;
            }
            else
            {
                monsterScoreMultiplier.SetActive(false);
            }
    }
    private void SpawnCoinOnTop(float yPosition)
    {
        GameObject coin = Instantiate(coinPrefab[Random.Range(0,coinPrefab.Length)],transform.position + new Vector3(0,yPosition,0),Quaternion.identity);
        coin.transform.parent = transform;
    }
    private void SpawnCoinOnBottom(float yPosition)
    {
        GameObject coin = Instantiate(coinPrefab[Random.Range(0,coinPrefab.Length)],transform.position + new Vector3(0,yPosition,0),Quaternion.identity);
        coin.transform.parent = transform;
    }
    public void CheckIfBonus()
    {
        if(isBonusSpawned && !HasBonusBeenPickedUp)
        {
            StartCoroutine(gameManager.PickUpBonus(bonusSpawnedIndex, currentBonusBehaviour));
            HasBonusBeenPickedUp = true;
        }
    }
}
