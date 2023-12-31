using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private float timeBetweenMissiles;
    private float timeBeforeNextMissile;
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextMissile = 0;
        timeBetweenMissiles = PlayerPrefs.GetFloat("MI_FireRateValue",2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBeforeNextMissile <= 0)
        {
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            timeBeforeNextMissile = timeBetweenMissiles;
        }
        else
        {
            timeBeforeNextMissile -= Time.deltaTime;
        }
    }
}
