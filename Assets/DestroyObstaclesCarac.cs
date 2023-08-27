using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObstaclesCarac : MonoBehaviour
{
    private GameManager gameManager;
    private float probabilityToDestroyDrone;
    private float probabilityToDestroySuspendedWall;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        probabilityToDestroyDrone = PlayerPrefs.GetFloat("OB_DestroyDRValue",0f);
        probabilityToDestroySuspendedWall = PlayerPrefs.GetFloat("OB_DestroySWValue",0f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {

        if(other.gameObject.tag == "Chain")
        {
            if(Random.Range(0,100) < probabilityToDestroySuspendedWall)
                gameManager.DoAnimationSuspendedWall(other.transform.parent.gameObject, giveBonus: true);
        }
        // else if(other.gameObject.tag == "Laser")
        //     DestroyLaser(other.gameObject);
        else if(other.gameObject.tag == "Drone")
        {
            if(Random.Range(0,100) < probabilityToDestroyDrone)
                other.gameObject.GetComponent<DroneBehaviour>().Die();
        }
    }
    private void DestroyLaser(GameObject laser)
    {
        GameObject parent = laser.transform.parent.gameObject;
        foreach(Transform child in parent.transform)
        {
            if(child.gameObject.tag == "Laser")
                Destroy(child.gameObject);
        }
    }
}
