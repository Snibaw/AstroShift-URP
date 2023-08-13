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

    [Header("Shooting")]
    [SerializeField] private float sightRange = 0.5f;
    [SerializeField] private Color[] glowColors;
    [SerializeField] private float timeBtwShootColorChange = 0.5f;
    [SerializeField] private float timeBtwShoot = 3f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float timeInFrontOfPlayerBeforeShooting = 0.5f;
    private float timeInFrontOfPlayerBeforeShootingTempo;
    private float timeBtwShootTempo;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        spriteGlow = GetComponent<SpriteGlow.SpriteGlowEffect>();
        spriteGlow.GlowColor = glowColors[0];

        droneAnimator = GetComponent<Animator>();
        timeBtwShootTempo = timeBtwShoot;
        timeInFrontOfPlayerBeforeShootingTempo = timeInFrontOfPlayerBeforeShooting;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isShooting) FollowPlayerYAxis();
        CheckIfPlayerIsInSight();

        timeBtwShootTempo -= Time.deltaTime;
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
}
