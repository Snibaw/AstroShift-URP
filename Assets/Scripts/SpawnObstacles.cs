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
    [SerializeField] private float phaseLaserDistance = 0f;
    [SerializeField] private float phaseChangeColorDistance = 0f;
    [SerializeField] private float distanceBetweenPhases = 0f;
    [SerializeField] private string startPhase;
    private float phaseDistance = 0f;
    public string currentPhase = "Start";
    private float currentPhaseXPosition = 0f;
    private int currentPhaseIndex = 0;
    private bool isMessageDisplayed = false;
    
    [Header("------------------------")]
    [Header("Start")]
    [SerializeField] private string[] startMessages;
    private WriteWithCoins writeWithCoins;

    [Header("------------------------")]
    [Header("ChangeColor")]
    [SerializeField] private float YPortalPosition = 2.3f;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float distanceBtwArrowAndPortal= 30f;
    [SerializeField] private GameObject portalPrefab;
    private bool isPortalSpawned = false;
    public int colorIndex = 0;
    public Color[] colors;

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

    [Header("------------------------")]
    [Header("Laser")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserMinDistance = 25f;
    [SerializeField] private float laserMaxDistance = 50f;
    [SerializeField] private float laserStartingXPosition = 0f;
    private Vector3 laserSpawnPosition;
    private float distanceUntilNextLaser = 0;
    private float distanceSinceLastLaser = 0;
    private Vector3 cameraPosWhenLastLaserSpawned;


    // Start is called before the first frame update
    void Start()
    {
        colorIndex = 0;

        playerPos = GameObject.Find("Player").transform;
        mainCameraPos = Camera.main.transform;
        distanceSinceLastSquare = squareMinDistance;
        distanceSinceLastSuspendedWall = suspendedWallMinDistance;
        distanceSinceLastDrone = droneMinDistance;
        distanceSinceLastLaser = laserMinDistance;

        currentPhase = startPhase;
        currentPhaseXPosition = mainCameraPos.position.x;
        UpdatePhaseDistance();

        writeWithCoins = GameObject.Find("WriteWithCoins").GetComponent<WriteWithCoins>();
    }

    // Update is called once per frame
    void FixedUpdate()
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
            case "ChangeColor":
                if(!isPortalSpawned)
                {
                    SpawnPortal();
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
            case "Laser":
                SpawnLaser();
                break;
            default:
                break;

        }
    }
    private void UpdatePhaseDistance()
    {
        if(currentPhase == "Start") phaseDistance = phaseStartDistance;
        else if(currentPhase == "Coin") phaseDistance = phaseCoinDistance;
        else if(currentPhase == "Laser") phaseDistance = phaseLaserDistance;
        else if(currentPhase == "ChangeColor") phaseDistance = phaseChangeColorDistance;
        else phaseDistance = possiblePhaseDistance;
    }
    private void ChangePhase()
    {
        do
        {
            currentPhaseIndex = Random.Range(0, possiblePhases.Length);
        } while (possiblePhases[currentPhaseIndex] == currentPhase);

        isMessageDisplayed = false;
        isPortalSpawned = false;
        currentPhase = possiblePhases[currentPhaseIndex];
        currentPhaseXPosition = mainCameraPos.position.x;
        UpdatePhaseDistance();
    }
    private void SpawnPortal()
    {
        isPortalSpawned = true;
        //Choose 2 colors different from the current one
        Color[] colorsToChooseFrom = new Color[colors.Length - 1];
        int index = 0;
        for(int i = 0; i < colors.Length; i++)
        {
            if(colors[i] != colors[colorIndex])
            {
                colorsToChooseFrom[index] = colors[i];
                index++;
            }
        }
        //Choose the first color
        Color[] colorsToUse = new Color[2];
        colorsToUse[0] = colorsToChooseFrom[Random.Range(0, colorsToChooseFrom.Length)];
        
        //Choose the second color
        colorsToChooseFrom = new Color[colors.Length - 2];
        index = 0;
        for(int i = 0; i < colors.Length; i++)
        {
            if(colors[i] != colors[colorIndex] && colors[i] != colorsToUse[0])
            {
                colorsToChooseFrom[index] = colors[i];
                index++;
            }
        }
        colorsToUse[1] = colorsToChooseFrom[Random.Range(0, colorsToChooseFrom.Length)];

        //Spawn the arrow
        Vector3 arrowSpawnPosition1 = new Vector3(mainCameraPos.position.x + 15, YPortalPosition, 0);
        Vector3 arrowSpawnPosition2 = new Vector3(mainCameraPos.position.x + 15, -YPortalPosition, 0);

        GameObject arrow1 = Instantiate(arrowPrefab, arrowSpawnPosition1, Quaternion.identity);
        GameObject arrow2 = Instantiate(arrowPrefab, arrowSpawnPosition2, Quaternion.identity);

        arrow1.GetComponent<ArrowBehaviour>().ChangeGlowColor(colorsToUse[0]);
        arrow2.GetComponent<ArrowBehaviour>().ChangeGlowColor(colorsToUse[1]);

        //Spawn Portals
        GameObject portal1 = Instantiate(portalPrefab, arrowSpawnPosition1 + new Vector3(distanceBtwArrowAndPortal, 0, 0), Quaternion.identity);
        GameObject portal2 = Instantiate(portalPrefab, arrowSpawnPosition2 + new Vector3(distanceBtwArrowAndPortal, 0, 0), Quaternion.identity);

        portal1.GetComponent<PortalBehaviour>().ChangeColor(colorsToUse[0]);
        portal2.GetComponent<PortalBehaviour>().ChangeColor(colorsToUse[1]);

        
    }
    private void SpawnLaser()
    {
        if(mainCameraPos.position.x < laserStartingXPosition) return;

        if(distanceSinceLastLaser >= laserMinDistance)
        {
            laserSpawnPosition = new Vector3(Mathf.Min(currentPhaseXPosition + phaseDistance + 12f , mainCameraPos.position.x + Random.Range(laserMinDistance, laserMaxDistance)), 0, 0);
            GameObject laser = Instantiate(laserPrefab, laserSpawnPosition, Quaternion.identity);
            laser.transform.parent = transform;
            cameraPosWhenLastLaserSpawned = mainCameraPos.position;
        }
        distanceSinceLastLaser = mainCameraPos.position.x - cameraPosWhenLastLaserSpawned.x;
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
            droneSpawnPosition = new Vector3(mainCameraPos.position.x+20, 0, 0);
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
                suspendedWallSpawnPosition = new Vector3(Mathf.Min(currentPhaseXPosition + phaseDistance + 12f, mainCameraPos.position.x + Random.Range(suspendedWallMinDistance, suspendedWallMaxDistance)), 0, 0);
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
            squareSpawnPosition = new Vector3(Mathf.Min(currentPhaseXPosition + phaseDistance + 12f, mainCameraPos.position.x + Random.Range(squareMinDistance, squareMaxDistance)), possibleYPositions[indexYPosition], 0);
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
