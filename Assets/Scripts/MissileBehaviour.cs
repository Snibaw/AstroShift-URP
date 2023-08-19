using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour
{
    private GameObject player;
    private float speed;
    [SerializeField] private GameObject explosionPrefab;

    void Start()
    {
        player = GameObject.Find("Player");
        speed = player.GetComponent<PlayerMovement>().speed*2f;
    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if(transform.position.x > player.transform.position.x + 15)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.CompareTag("Chain"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().DoAnimationSuspendedWall(other.gameObject.transform.parent.gameObject, giveBonus: false);
            SpawnExplosionAndDestroy();
        }
        else if(other.gameObject.CompareTag("Obstacle"))
        {
            if(other.gameObject.transform.parent.name == "SquarePart")
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().DoAnimationSuspendedWall(other.gameObject.transform.parent.transform.parent.gameObject, giveBonus: false);
                SpawnExplosionAndDestroy();
            }
            else
            {
                GameObject objectToDestroy = other.gameObject;
                if(other.gameObject.transform.parent != null)
                    objectToDestroy = other.gameObject.transform.parent.gameObject;
                
                GameObject.Find("GameManager").GetComponent<GameManager>().DestroySquareGroup(objectToDestroy);
                SpawnExplosionAndDestroy();
            }   
            
        }
    }
    private void SpawnExplosionAndDestroy()
    {
        GameObject Explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(Explosion, 1.5f);
        Destroy(gameObject);
    }
}
