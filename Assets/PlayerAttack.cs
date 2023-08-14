using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnim;
    private PlayerMovement playerMovement;
    private bool isAttacking = false;
    [SerializeField] private GameObject attackHitPrefab;
    [SerializeField] private AttackRangePlayer attackRangePlayer;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    public void Attack(GameObject target)
    {
        if(isAttacking) return;
        playerAnim.SetTrigger("Attack");
        if(target.tag == "Chain")
        {
            StartCoroutine(AttackChainCoroutine(Random.Range(0,4)==0));
        }
        else if(target.tag == "Drone")
        {
            StartCoroutine(AttackDroneCoroutine(target));
        }
        
    }
    private IEnumerator AttackDroneCoroutine(GameObject target)
    {
        yield return new WaitForSeconds(0.20f);
        GameObject attackHit = Instantiate(attackHitPrefab, transform.position + new Vector3(0.5f,0,0), Quaternion.identity);
        Destroy(attackHit, 0.5f);

        target.GetComponent<DroneBehaviour>().Die();
    }
    private IEnumerator AttackChainCoroutine(bool doSlowMotion = true)
    {

        if(doSlowMotion)
        {
            float gravityScaleTempo = playerMovement.rb.gravityScale;
            playerMovement.rb.gravityScale = 0;
            playerMovement.speed /= 2;
            attackRangePlayer.HitSuspendedWallAnimation();
            yield return new WaitForSeconds(0.15f);
            Time.timeScale = 0.4f;
            yield return new WaitForSeconds(0.08f);
            GameObject attackHit = Instantiate(attackHitPrefab, transform.position + new Vector3(0.5f,0,0), Quaternion.identity);
            Destroy(attackHit, 0.5f);
            yield return new WaitForSeconds(0.15f);
            Time.timeScale = 1f;
            playerMovement.speed *= 2;
            playerMovement.rb.gravityScale = gravityScaleTempo;
        }
        else
        {
            attackRangePlayer.HitSuspendedWallAnimation();
            yield return new WaitForSeconds(0.20f);
            GameObject attackHit = Instantiate(attackHitPrefab, transform.position + new Vector3(0.5f,0,0), Quaternion.identity);
            Destroy(attackHit, 0.5f);
            
        }
        
    }
}