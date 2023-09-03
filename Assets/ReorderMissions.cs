using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReorderMissions : MonoBehaviour
{
    private GameObject[] missions;
    // Start is called before the first frame update
    void Start()
    {
        missions = GameObject.FindGameObjectsWithTag("Mission");

        //Order missions depending on their progress
        for(int i = 0; i < missions.Length; i++)
        {
            for(int j = i+1; j < missions.Length; j++)
            {
                if(GetPercentageDone(missions[i].name) < GetPercentageDone(missions[j].name))
                {
                    GameObject temp = missions[i];
                    missions[i] = missions[j];
                    missions[j] = temp;
                }
            }
        }
    }
    private float GetPercentageDone(string Key)
    {
        int startValue = PlayerPrefs.GetInt(Key + "StartValue", 0);
        int maxValue = PlayerPrefs.GetInt(Key + "MaxValue", 0);
        int currentValue = Mathf.Min(PlayerPrefs.GetInt(Key + "Value", 0) - startValue, maxValue);
        return (float)currentValue / (float)maxValue;
    }
}
