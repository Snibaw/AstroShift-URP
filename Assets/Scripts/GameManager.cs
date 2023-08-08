using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        Destroy(GameObject.Find("Player"));
    }
}
