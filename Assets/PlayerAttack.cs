using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnim;
    private PlayerMovement playerMovement;
    [SerializeField] private GameObject attackHitPrefab;
    [SerializeField] private AttackRangePlayer attackRangePlayer;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack()
    {
        if(playerMovement.canMove == false)
        {
            return;
        }

        playerMovement.canMove = false;
        playerAnim.SetTrigger("Attack");
        StartCoroutine(AttackCoroutine());
    }
    private IEnumerator AttackCoroutine()
    {
        playerMovement.speed /= 2;
        attackRangePlayer.AnimationLastObjectCollidedWith();
        yield return new WaitForSeconds(0.18f);
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(0.05f);
        GameObject attackHit = Instantiate(attackHitPrefab, transform.position + new Vector3(0.5f,0,0), Quaternion.identity);
        Destroy(attackHit, 0.5f);
        yield return new WaitForSeconds(0.15f);
        Time.timeScale = 1f;
        playerMovement.speed *= 2;
        playerMovement.canMove = true;
        

    }
}
