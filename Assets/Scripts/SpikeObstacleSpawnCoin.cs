using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObstacleSpawnCoin : MonoBehaviour
{
    public bool isSuspended = false;
    [SerializeField] private GameObject[] coinPrefab;
    [SerializeField] private float[] coinTransformY;
    [SerializeField] private GameObject monsterScoreMultiplier;
    [SerializeField] private float probabilityToSpawnMonsterScoreMultiplier;
    [SerializeField] private float probabilityToSpawnBonus;
    [SerializeField] private GameObject[] bonusList;
    [SerializeField] private GameObject bonusParent;
    public int bonusSpawnedIndex = 0;
    public bool canSpawnBonus = false;
    private BonusBehaviour currentBonusBehaviour;
    private bool doCurrentBonusMoveToPlayer = false;
    private bool isBonusSpawned = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if(canSpawnBonus)
        {
            SpawnMonsterScoreMultiplier();
            SpawnBonus();
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
        if(Random.Range(0,100) < probabilityToSpawnBonus)
        {
            bonusParent.SetActive(true);
            bonusSpawnedIndex = Random.Range(0,bonusList.Length);
            foreach(GameObject bonus in bonusList)
            {
                bonus.SetActive(false);
            }
            bonusList[bonusSpawnedIndex].SetActive(true);
            currentBonusBehaviour = bonusList[bonusSpawnedIndex].GetComponent<BonusBehaviour>();
            isBonusSpawned = true;
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
                monsterScoreMultiplier.SetActive(true);
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
        if(isBonusSpawned)
        {
            StartCoroutine(GameObject.Find("GameManager").GetComponent<GameManager>().PickUpBonus(bonusSpawnedIndex, currentBonusBehaviour));
        }
    }
}
