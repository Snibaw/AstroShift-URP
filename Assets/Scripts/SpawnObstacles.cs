using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    private Transform mainCameraPos;
    private Transform playerPos;

    [Header("Phase")]
    [SerializeField] private string[] possiblePhases;
    [SerializeField] private float possiblePhaseDistance;
    [SerializeField] private float phaseCoinDistance = 0f;
    [SerializeField] private float phaseStartDistance = 0f;
    [SerializeField] private float distanceBetweenPhases = 0f;
    private float phaseDistance = 0f;
    [SerializeField] private string currentPhase = "Start";
    private float currentPhaseXPosition = 0f;
    private int currentPhaseIndex = 0;
    private bool isMessageDisplayed = false;
    
    [Header("------------------------")]
    [Header("Start")]
    [SerializeField] private string[] startMessages;
    private WriteWithCoins writeWithCoins;

    [Header("------------------------")]
    [Header("Coin")]
    [SerializeField] private string[] coinMessages;


    [Header("------------------------")]
    [Header("Square")]
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private float squareMinDistance = 10f;
    [SerializeField] private float squareMaxDistance = 20f;
    [SerializeField] private float squareStartingXPosition = 0f;
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

    [Header("------------------------")]
    [Header("Drone")]
    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private float droneMinDistance = 100f;
    [SerializeField] private float droneMaxDistance = 200f;
    [SerializeField] private float droneStartingXPosition = 0f;
    private Vector3 droneSpawnPosition;
    private float distanceSinceLastDrone = 0;
    private Vector3 cameraPosWhenLastDroneSpawned;


    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        mainCameraPos = Camera.main.transform;
        distanceSinceLastSquare = squareMinDistance;
        distanceSinceLastSuspendedWall = suspendedWallMinDistance;
        distanceSinceLastDrone = droneMinDistance;

        currentPhase = "Start";
        currentPhaseXPosition = mainCameraPos.position.x;
        phaseDistance = phaseStartDistance;

        writeWithCoins = GameObject.Find("WriteWithCoins").GetComponent<WriteWithCoins>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mainCameraPos.position.x > currentPhaseXPosition + phaseDistance)
        {
            currentPhase = "Waiting";
        }

        switch (currentPhase)
        {
            case "Waiting":
                if(mainCameraPos.position.x > currentPhaseXPosition + distanceBetweenPhases)
                {
                    ChangePhase();
                }
                break;
            case "Start":
                if(!isMessageDisplayed)
                {
                    writeWithCoins.WriteWord(startMessages[Random.Range(0, startMessages.Length)], new Vector3(mainCameraPos.position.x + 30f, 0, 0));
                    isMessageDisplayed = true;
                }
                break;
            case "Coin":
                if(!isMessageDisplayed)
                {
                    writeWithCoins.WriteWord(coinMessages[Random.Range(0, coinMessages.Length)], new Vector3(mainCameraPos.position.x + 50f, 0, 0));
                    isMessageDisplayed = true;
                }
                break;
            case "Square":
                SpawnSquare();
                break;
            case "SuspendedWall":
                SpawnSuspendedWall();
                SpawnSquare();
                break;
            case "Drone":
                SpawnDrone();
                break;
            default:
                break;

        }
    }
    private void ChangePhase()
    {
        do
        {
            currentPhaseIndex = Random.Range(0, possiblePhases.Length);
        } while (possiblePhases[currentPhaseIndex] == currentPhase);

        isMessageDisplayed = false;
        currentPhase = possiblePhases[currentPhaseIndex];
        currentPhaseXPosition = mainCameraPos.position.x;
        if(currentPhase != "Coin") phaseDistance = possiblePhaseDistance;
        else phaseDistance = phaseCoinDistance;
    }
    private void SpawnDrone()
    {
        if(mainCameraPos.position.x < droneStartingXPosition) return;

        if(distanceSinceLastDrone >= droneMinDistance)
        {
            int numberOfDrones = 2;
            StartCoroutine(SpawnDronesWithDelay(numberOfDrones));
            cameraPosWhenLastDroneSpawned = mainCameraPos.position;
        }
        distanceSinceLastDrone = mainCameraPos.position.x - cameraPosWhenLastDroneSpawned.x;
    }
    private IEnumerator SpawnDronesWithDelay(int numberOfDrones)
    {
        for(int i = 0; i < numberOfDrones; i++)
        {
            droneSpawnPosition = new Vector3(mainCameraPos.position.x, 0, 0);
            GameObject drone = Instantiate(dronePrefab, droneSpawnPosition, Quaternion.identity);
            drone.transform.parent = mainCameraPos.transform;
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
    private void SpawnSuspendedWall()
    {
        if(mainCameraPos.position.x < suspendedWallStartingXPosition) return;

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
    private void SpawnSquare()
    {
        if(mainCameraPos.position.x < squareStartingXPosition) return;
        
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
