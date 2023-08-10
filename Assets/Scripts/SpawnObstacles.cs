using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    private Transform mainCameraPos;
    private Transform playerPos;

    [Header("Square")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private float squareMinDistance = 10f;
    [SerializeField] private float squareMaxDistance = 20f;
    [SerializeField] private float[] possibleYPositions;
    private Vector3 squareSpawnPosition;
    private float distanceSinceLastSquare = 0;
    private Vector3 cameraPosWhenLastSquareSpawned;

    [Header("------------------------")]
    [Header("Suspended Wall")]
    [SerializeField] private GameObject[] suspendedWallPrefab;
    [SerializeField] private float suspendedWallMinDistance = 10f;
    [SerializeField] private float suspendedWallMaxDistance = 20f;
    [SerializeField] private float suspendedWallStartingXPosition = 0f; // The x position of the first suspended wall
    private Vector3 suspendedWallSpawnPosition;
    private float distanceSinceLastSuspendedWall = 0;
    private Vector3 cameraPosWhenLastSuspendedWallSpawned;


    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        mainCameraPos = Camera.main.transform;
        distanceSinceLastSquare = squareMinDistance;
        distanceSinceLastSuspendedWall = suspendedWallMinDistance;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnSquare();
        SpawnSuspendedWall();
    }

    private void SpawnSuspendedWall()
    {
        if(mainCameraPos.position.x >= suspendedWallStartingXPosition)
        {
            if(distanceSinceLastSuspendedWall >= suspendedWallMinDistance)
            {
                int indexWall = Random.Range(0, suspendedWallPrefab.Length);
                suspendedWallSpawnPosition = new Vector3(mainCameraPos.position.x + Random.Range(suspendedWallMinDistance, suspendedWallMaxDistance), 0, 0);
                GameObject suspendedWall = Instantiate(suspendedWallPrefab[indexWall], suspendedWallSpawnPosition, Quaternion.identity);
                suspendedWall.transform.parent = transform;
                cameraPosWhenLastSuspendedWallSpawned = mainCameraPos.position;
            }
            distanceSinceLastSuspendedWall = mainCameraPos.position.x - cameraPosWhenLastSuspendedWallSpawned.x;
        }
    }
    private void SpawnSquare()
    {
        if(distanceSinceLastSquare >= squareMinDistance)
        {
            int indexYPosition = DetermineYPositionSquare();
            squareSpawnPosition = new Vector3(mainCameraPos.position.x + Random.Range(squareMinDistance, squareMaxDistance), possibleYPositions[indexYPosition], 0);
            GameObject square = Instantiate(squarePrefab, squareSpawnPosition, Quaternion.identity);
            square.transform.parent = transform;
            cameraPosWhenLastSquareSpawned = mainCameraPos.position;
        }
        distanceSinceLastSquare = mainCameraPos.position.x - cameraPosWhenLastSquareSpawned.x;
    }
    private int DetermineYPositionSquare()
    {
        if(Random.Range(0, 20) != 0) // More probability to spawn square on player side
        {
            return playerPos.position.y > 0 ? 0 : 1;
        }
        else
        {
            return playerPos.position.y > 0 ? 1 : 0;
        }
    }
}
