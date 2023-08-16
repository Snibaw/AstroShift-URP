using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterKillerBehaviour : MonoBehaviour
{
    [SerializeField] private float timeBtwShoots;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelPosition;
    private float timeBtwShootsTempo;
    private Animator animator;
    private bool isDead;
    private Transform cameraPosition;
    public bool isTop = false;
    private GameObject player;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        timeBtwShootsTempo = 0;
        cameraPosition = Camera.main.transform;

        player = GameObject.Find("Player");
        speed = player.GetComponent<PlayerMovement>().speed/2 - 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        timeBtwShootsTempo -= Time.deltaTime;
        Shoot();

        if(cameraPosition.position.x > transform.position.x + 12)
        {
            DestroyGameObject();
        }
    }
    public void KillMonsterKiller()
    {
        if(isDead) return;
        isDead = true;
        animator.SetTrigger("Die");
        DestroyGameObject(1f);

        GameObject[] listMonsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach(GameObject monster in listMonsters)
        {
            if(monster.GetComponent<MonsterBehaviour>().isTop == isTop) monster.GetComponent<MonsterBehaviour>().BecomeHappy();
        }
    }
    private void DestroyGameObject(float time = 0f)
    {
        GameObject.Find("MonsterSpawner").GetComponent<MonsterSpawner>().MonsterKillerDied();
        Destroy(gameObject, time);
    }
    public void Shoot()
    {
        if(timeBtwShootsTempo > 0) return;
        animator.SetTrigger("Shoot");
        GameObject bullet = Instantiate(bulletPrefab, barrelPosition.position, Quaternion.identity);
        Destroy(bullet, 5f);
        timeBtwShootsTempo = timeBtwShoots;
    }
}
