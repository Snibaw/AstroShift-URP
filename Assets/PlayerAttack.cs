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
    public void Attack(GameObject target)
    {
        if(playerMovement.canMove == false) return;

        playerMovement.canMove = false;
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
        playerMovement.canMove = true;
        yield return new WaitForSeconds(0.20f);
        GameObject attackHit = Instantiate(attackHitPrefab, transform.position + new Vector3(0.5f,0,0), Quaternion.identity);
        Destroy(attackHit, 0.5f);

        target.GetComponent<DroneBehaviour>().Die();
    }
    private IEnumerator AttackChainCoroutine(bool doSlowMotion = true)
    {
        float gravityScaleTempo = playerMovement.rb.gravityScale;
        playerMovement.rb.gravityScale = 0;
        if(doSlowMotion)
        {
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
            playerMovement.canMove = true;
        }
        else
        {
            playerMovement.canMove = true;
            attackRangePlayer.HitSuspendedWallAnimation();
            yield return new WaitForSeconds(0.20f);
            GameObject attackHit = Instantiate(attackHitPrefab, transform.position + new Vector3(0.5f,0,0), Quaternion.identity);
            Destroy(attackHit, 0.5f);
            
        }
        playerMovement.rb.gravityScale = gravityScaleTempo;
    }
}
