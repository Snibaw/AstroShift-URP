using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBulletBehaviour : MonoBehaviour
{
    public float speed = 5f;
    private Animator bulletAnimator;

    private void Start() {
        bulletAnimator = GetComponent<Animator>();
        Destroy(gameObject, 5f);
    }
    void FixedUpdate()
    {
        //Move the bullet in -x axis
        transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().PlayerTakeDamage();
        }
        if(other.CompareTag("Player") || other.CompareTag("Chain") || other.CompareTag("Obstacle"))
        {
            bulletAnimator.SetTrigger("Hit");
            speed = 0;
            Destroy(gameObject, 1f);
        }
        
    }
}
