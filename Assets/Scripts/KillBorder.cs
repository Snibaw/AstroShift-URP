using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBorder : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.name == "Player")
            gameManager.GameOver();
        else if(other.gameObject.tag == "Obstacle")
            Destroy(other.gameObject,2f);
            
    }
}
