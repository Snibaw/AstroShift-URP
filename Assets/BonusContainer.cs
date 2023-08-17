using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusContainer : MonoBehaviour
{
    [SerializeField] private GameObject bonusElementPrefab;
    [SerializeField] private float[] timeBonusElement;
    public List<int> bonusSpawnedIndex = new List<int>();
    // Start is called before the first frame update
    public void AddBonusElement(int bonusIndex)
    {
        ;
        
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
        }
        
    }
    public void ElementHasBeenRemoved(int bonusIndex)
    {
        bonusSpawnedIndex.Remove(bonusIndex);
    }
}
