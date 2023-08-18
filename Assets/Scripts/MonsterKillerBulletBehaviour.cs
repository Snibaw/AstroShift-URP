using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKillerBulletBehaviour : MonoBehaviour
{
    private Animator animator;
    private bool hasHit = false;
    private void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.left * 5 * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if(hasHit) return;
        if(other.CompareTag("MonsterKiller") ||other.CompareTag("Untagged")) return;
        hasHit = true;
        if(other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterBehaviour>().KillMonster(false);
        }
        animator.SetTrigger("Hit");
    }
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
