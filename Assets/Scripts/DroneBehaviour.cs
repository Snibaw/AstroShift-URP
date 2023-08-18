using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBehaviour : MonoBehaviour
{
    private Transform playerPos;
    public bool isShooting = false;
    private SpriteGlow.SpriteGlowEffect spriteGlow;
    [SerializeField] private float movementSpeed = 0.01f;
    private Animator droneAnimator;
    private Transform camPos;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D;

    [Header("Shooting")]
    [SerializeField] private float sightRange = 0.5f;
    [SerializeField] private Color[] glowColors;
    [SerializeField] private Color endGlowColor;
    [SerializeField] private float timeBtwShootColorChange = 0.5f;
    [SerializeField] private float timeBtwShoot = 3f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float timeInFrontOfPlayerBeforeShooting = 0.5f;
    [SerializeField] private float offsetBtwDroneAndCamera = 7f;
    [SerializeField] private float offsetStartBtwDroneAndCamera = 15f;
    [SerializeField] private float timeBeforeLeavingScreen = 15f;
    private float timeInFrontOfPlayerBeforeShootingTempo;
    private float timeBtwShootTempo;
    public bool isStarting = true;
    public bool isLeaving = false;
    public bool prepareToLeave = false;
    public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        camPos = Camera.main.transform;
        spriteGlow = GetComponent<SpriteGlow.SpriteGlowEffect>();
        spriteGlow.GlowColor = new Color(0,0,0,0);

        droneAnimator = GetComponent<Animator>();
        timeBtwShootTempo = 0;
        timeInFrontOfPlayerBeforeShootingTempo = timeInFrontOfPlayerBeforeShooting;

        transform.position = new Vector3(camPos.position.x + offsetStartBtwDroneAndCamera, Random.Range(-3.5f,3.5f), transform.position.z);
        StartCoroutine(StartLeavingCoroutine());

        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void FixedUpdate()
    {
        if(isDead) return;
        if(prepareToLeave)
        {
            if(!isShooting) 
            {
                prepareToLeave = false;
                isLeaving = true;
                spriteGlow.GlowBrightness = 1;
                spriteGlow.GlowColor = endGlowColor;
            }
        }
        if(isLeaving)
        {
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
            return;
        }

        if(isStarting) 
        {
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);

            if(transform.position.x > camPos.position.x + offsetBtwDroneAndCamera - 0.1f && transform.position.x < camPos.position.x + offsetBtwDroneAndCamera + 0.1f)
            {
                isStarting = false;
                spriteGlow.GlowColor = glowColors[0];
            }
            return;
        }


        if(!isShooting) FollowPlayerYAxis();
        CheckIfPlayerIsInSight();

        timeBtwShootTempo -= Time.deltaTime;
    }
    private IEnumerator StartLeavingCoroutine()
    {
        yield return new WaitForSeconds(timeBeforeLeavingScreen);
        prepareToLeave = true; 
        if(!isShooting) 
        {
            prepareToLeave = false;
            isLeaving = true;
            spriteGlow.GlowBrightness = 1;
            spriteGlow.GlowColor = endGlowColor;
        }
    }
    private void FollowPlayerYAxis()
    {
        //Follow smoothly the player on the y axis
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, playerPos.position.y, movementSpeed), transform.position.z);
    }
    private void CheckIfPlayerIsInSight()
    {
        //Check if the player is in sight
        if(playerPos.position.y > transform.position.y - sightRange && playerPos.position.y < transform.position.y + sightRange)
        {
            timeInFrontOfPlayerBeforeShootingTempo -= Time.deltaTime;
            if(!isShooting && timeBtwShootTempo < 0) StartCoroutine(PrepareShooting());
        }
        else
        {
            timeInFrontOfPlayerBeforeShootingTempo = timeInFrontOfPlayerBeforeShooting;
        }
    }
    private IEnumerator PrepareShooting()
    {
        float glowBrightnessTempo = spriteGlow.GlowBrightness;
        isShooting = true;
        for(int i = 1; i < glowColors.Length; i++)
        {
            spriteGlow.GlowColor = glowColors[i];
            if(i == glowColors.Length - 1)
            {
                spriteGlow.GlowBrightness += 3f;
                yield return new WaitForSeconds(timeBtwShootColorChange/3);
            }
            else
            {   
                spriteGlow.GlowBrightness += 1f;
                yield return new WaitForSeconds(timeBtwShootColorChange);
            }
            
        }
        droneAnimator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.5f);
        timeBtwShootTempo = timeBtwShoot;
        isShooting = false;
        spriteGlow.GlowBrightness = glowBrightnessTempo;
        spriteGlow.GlowColor = glowColors[0];
    }
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position - new Vector3(0.5f,0,0), Quaternion.identity);
        bullet.GetComponent<DroneBulletBehaviour>().speed = bulletSpeed;
    }
    public void Die()
    {
        if(isDead) return;
        isDead = true;
        droneAnimator.SetTrigger("Die");
        rb.isKinematic = false;
        gameObject.transform.parent = null;
        circleCollider2D.isTrigger = false;
        Destroy(gameObject, 1.5f);
    }
}
