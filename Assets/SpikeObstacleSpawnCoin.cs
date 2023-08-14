using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObstacleSpawnCoin : MonoBehaviour
{
    public bool isSuspended = false;
    [SerializeField] private GameObject[] coinPrefab;
    [SerializeField] private float[] coinTransformY;
    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0,3) == 0)
            return;
        if(isSuspended)
            SpawnCoinOnTop();
        else
        {
            if(transform.position.y < 0)
                SpawnCoinOnTop();
            else
                SpawnCoinOnBottom();
        }

    }
    private void SpawnCoinOnTop()
    {
        GameObject coin = Instantiate(coinPrefab[Random.Range(0,coinPrefab.Length)],transform.position + new Vector3(0,coinTransformY[0],0),Quaternion.identity);
        coin.transform.parent = transform;
    }
    private void SpawnCoinOnBottom()
    {
        GameObject coin = Instantiate(coinPrefab[Random.Range(0,coinPrefab.Length)],transform.position + new Vector3(0,coinTransformY[1],0),Quaternion.identity);
        coin.transform.parent = transform;
    }
}
