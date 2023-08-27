using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusContainer : MonoBehaviour
{
    [SerializeField] private GameObject bonusElementPrefab;
    [SerializeField] private float[] timeBonusElement;
    public List<int> bonusSpawnedIndex = new List<int>();
    private GameObject player;
    private GameManager gameManager;

    private void Start() {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        timeBonusElement[1] = PlayerPrefs.GetFloat("SH_DurationValue",0f);
        timeBonusElement[2] = PlayerPrefs.GetFloat("MA_DurationValue",0f);
        timeBonusElement[3] = PlayerPrefs.GetFloat("MI_DurationValue",0f);
    }
    public void AddBonusElement(int bonusIndex)
    {        
        //Check if the bonus is already spawned
        if(bonusSpawnedIndex.Contains(bonusIndex))
        {
            //Find the bonus element and reset the time
            foreach(Transform child in transform)
            {
                if(child.GetComponent<BonusElement>().bonusIndex == bonusIndex)
                {
                    child.GetComponent<BonusElement>().SetTime(timeBonusElement[bonusIndex]);
                }
            }
        }
        else
        {
            bonusSpawnedIndex.Add(bonusIndex);
            GameObject bonusElement = Instantiate(bonusElementPrefab, transform.position, Quaternion.identity);
            bonusElement.transform.SetParent(transform);
            bonusElement.GetComponent<BonusElement>().InitBonusElement(bonusIndex, timeBonusElement[bonusIndex]);
            //Add shield corresponding to bonus (missile has no shield)
            player.GetComponent<PlayerBonus>().ActivateShield(bonusIndex);
        }
        
    }
    public void ElementHasBeenRemoved(int bonusIndex)
    {
        bonusSpawnedIndex.Remove(bonusIndex);
        //Remove shield corresponding to bonus (missile has no shield)
        player.GetComponent<PlayerBonus>().DesactivateShield(bonusIndex);

        gameManager.DesactivateShieldBonus(bonusIndex);
    }
    public void RemoveElement(int bonusIndex)
    {
        foreach(Transform child in transform)
        {
            if(child.GetComponent<BonusElement>().bonusIndex == bonusIndex)
            {
                Destroy(child.gameObject);
            }
        }
        ElementHasBeenRemoved(bonusIndex);
    }
}
