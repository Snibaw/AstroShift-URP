using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject monsterKillerPrefab;
    [SerializeField] private float ySpawnMonster;
    [SerializeField] private float ySpawnMonsterKiller;
    [SerializeField] private float[] timeBtwSpawns;
    [SerializeField] private float timeBtwSpawnKiller;
    private Transform cameraPos;
    private int monsterCountBot = 0;
    private int monsterCountTop = 0;
    [SerializeField] private int monsterKillerCount = 0;
    private float timeBtwSpawnKillerTempo;
    private float timeBtwSpawnsTempoBot;
    private float timeBtwSpawnsTempoTop;
    private string currentPhase;
    [SerializeField] private string[] possiblePhases;

    private void Start() {
        cameraPos = Camera.main.transform;
        timeBtwSpawnsTempoBot = Random.Range(timeBtwSpawns[0], timeBtwSpawns[1]);
        timeBtwSpawnsTempoTop = Random.Range(timeBtwSpawns[0], timeBtwSpawns[1]);
        timeBtwSpawnKillerTempo = timeBtwSpawnKiller;
    }

    private void FixedUpdate() {
        timeBtwSpawnKillerTempo -= Time.deltaTime;

        if(timeBtwSpawnsTempoBot > 0) timeBtwSpawnsTempoBot -= Time.deltaTime;
        else SpawnMonsterBot();

        if(timeBtwSpawnsTempoTop > 0) timeBtwSpawnsTempoTop -= Time.deltaTime;
        else SpawnMonsterTop();

        currentPhase = GameObject.Find("Obstacles") != null ? GameObject.Find("Obstacles").GetComponent<SpawnObstacles>().currentPhase : "Start";
        foreach(string phase in possiblePhases)
        {
            if(currentPhase == phase)
            {
                if(monsterKillerCount == 0 && timeBtwSpawnKillerTempo <=0)
                {
                    if(Random.Range(0, 2) == 0) SpawnMonsterKiller(false);
                    else SpawnMonsterKiller(true);
                }
            }
        
        }
    }

    public void MonsterKillerDied()
    {
        monsterKillerCount = 0;
    }

    public void MonsterDied()
    {
        monsterCountBot --;
    }

    private void SpawnMonsterBot()
    {
        GameObject botMonster =Instantiate(monsterPrefab, new Vector3(cameraPos.position.x + 13, ySpawnMonster, 0), Quaternion.identity);
        botMonster.transform.parent = transform;

        monsterCountBot++;
        timeBtwSpawnsTempoBot = Random.Range(timeBtwSpawns[0], timeBtwSpawns[1]);
    }
    private void SpawnMonsterTop()
    {
        GameObject topMonster = Instantiate(monsterPrefab, new Vector3(cameraPos.position.x + 13, -ySpawnMonster, 0), Quaternion.identity);
        topMonster.transform.localScale = new Vector3(topMonster.transform.localScale.x, -topMonster.transform.localScale.y, topMonster.transform.localScale.z);
        topMonster.GetComponent<MonsterBehaviour>().isTop = true;
        topMonster.transform.parent = transform;

        monsterCountTop++;
        timeBtwSpawnsTempoTop = Random.Range(timeBtwSpawns[0], timeBtwSpawns[1]);
    }
    private void SpawnMonsterKiller(bool isTop)
    {
        GameObject MonsterKiller;
        if(isTop) 
        {
            MonsterKiller = Instantiate(monsterKillerPrefab, new Vector3(cameraPos.position.x + 12, -ySpawnMonsterKiller, 0), Quaternion.identity);
            MonsterKiller.transform.localScale = new Vector3(MonsterKiller.transform.localScale.x, -MonsterKiller.transform.localScale.y, MonsterKiller.transform.localScale.z);
            MonsterKiller.GetComponent<MonsterKillerBehaviour>().isTop = true;
        }
        else MonsterKiller = Instantiate(monsterKillerPrefab, new Vector3(cameraPos.position.x + 12, ySpawnMonsterKiller, 0), Quaternion.identity);
        MonsterKiller.transform.parent = transform;
        monsterKillerCount++;
        timeBtwSpawnKillerTempo = timeBtwSpawnKiller;
    }
}
