using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField] private Color angryColor;
    [SerializeField] private Color happyColor;
    private bool isJumping = false;
    private Transform cameraPosition;
    [SerializeField] private float movementSpeedMultiplier = 0.5f;
    [SerializeField] private GameObject deathAnimationPrefab;
    private GameObject player;
    private float speed;
    private Animator animator;
    public bool isAngry = false;
    public bool isHappy = false;
    private bool isDead = false;
    private float playerSpeed;
    private SpriteGlow.SpriteGlowEffect spriteGlow;
    private GameObject heart;
    public bool isTop = false;

    private void Start() 
    {
        cameraPosition = Camera.main.transform;
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerSpeed = player.GetComponent<PlayerMovement>().speed;
        speed = playerSpeed * movementSpeedMultiplier;
        heart = transform.GetChild(0).gameObject;
        heart.SetActive(false);
    }
    void Update()
    {
        if(isDead) return;

        if(isAngry) speed = playerSpeed-3;
        transform.position += Vector3.right * speed * Time.deltaTime;

        if(cameraPosition.position.x > transform.position.x + 12)
        {
            DestroyGameObject();
        }
    }
    public void StopJumping()
    {
        isJumping = false;
    }
    public void DestroyGameObject()
    {
        GameObject.Find("MonsterSpawner").GetComponent<MonsterSpawner>().MonsterDied();
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(isDead) return;

        if(other.CompareTag("Player"))
        {
            if(isTop)
            {
                if(!player.GetComponent<PlayerMovement>().isGrounded && player.GetComponent<Rigidbody2D>().velocity.y > 4)
                {
                    KillMonster(true);
                }
            }
            else
            {
                if(!player.GetComponent<PlayerMovement>().isGrounded && player.GetComponent<Rigidbody2D>().velocity.y < -4)
                {
                    KillMonster(true);
                }
            }
            
            if(isAngry)
            {
                animator.SetTrigger("Attack");
            }
        }
    }
    public void KillMonster(bool isJumpKill)
    {
        if(isDead) return;
        isDead = true;
        if(animator != null) animator.SetTrigger("Die");
        if(isJumpKill)
        {
            Instantiate(deathAnimationPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            
            GameObject[] listMonsters = GameObject.FindGameObjectsWithTag("Monster");

            foreach(GameObject monster in listMonsters)
            {
                if(monster != gameObject && monster.GetComponent<MonsterBehaviour>().isTop == isTop) 
                {
                    monster.GetComponent<MonsterBehaviour>().BecomeAngry();
                }
            }
        }
    }
    public void BecomeAngry()
    {
        if(isDead)  return;
        if(isAngry) return;
        heart.SetActive(false);
        isAngry = true;
        spriteGlow = GetComponent<SpriteGlow.SpriteGlowEffect>();
        spriteGlow.GlowColor = angryColor;
        spriteGlow.OutlineWidth = 1;
        animator.SetBool("isRunning", true);
    }
    public void BecomeHappy()
    {
        if(isAngry) return;
        if(isDead)  return;
        isHappy = true;
        animator.SetTrigger("Jump");
        heart.SetActive(true);
        spriteGlow = GetComponent<SpriteGlow.SpriteGlowEffect>();
        spriteGlow.GlowColor = happyColor;
        spriteGlow.OutlineWidth = 1;
    }
}
