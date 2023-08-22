using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnim;
    private PlayerMovement playerMovement;
    public bool isAttacking = false;
    [SerializeField] private GameObject attackHitPrefab;
    [SerializeField] private AttackRangePlayer attackRangePlayer;
    private Rigidbody2D rb;
    private float gravityScaleTempo;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void EndOfAttack()
    {
        if(rb.gravityScale < 1 && rb.gravityScale > -1) rb.gravityScale *=25;
    }

    // Update is called once per frame
    public void Attack(GameObject target)
    {
        if(isAttacking) return;
        gameObject.transform.position += new Vector3(0,0.3f,0); //offset Attack Sprites
        isAttacking = true;
        
        if(target.tag == "Chain")
        {
            SpikeObstacleSpawnCoin SquarePartComponent = target.transform.parent.gameObject.transform.GetChild(0).GetComponent<SpikeObstacleSpawnCoin>();
            if(SquarePartComponent.HasBeenDestroyed) return;
            
            // StartCoroutine(AttackChainCoroutine(Random.Range(0,4)==0));
            StartCoroutine(AttackChainCoroutine(false));
        }
        else if(target.tag == "Drone")
        {
            StartCoroutine(AttackDroneCoroutine(target));
        }
        else if(target.tag == "MonsterKiller")
        {
            StartCoroutine(AttackMonsterKillerCoroutine(target));
        }

        playerAnim.SetTrigger("Attack");
        
    }
    private IEnumerator AttackMonsterKillerCoroutine(GameObject target)
    {
        rb.gravityScale /=25;
        yield return new WaitForSeconds(0.20f);
        GameObject attackHit = Instantiate(attackHitPrefab, transform.position + new Vector3(0.5f,0,0), Quaternion.identity);
        Destroy(attackHit, 0.5f);

        target.GetComponent<MonsterKillerBehaviour>().KillMonsterKiller();
        isAttacking = false;
    }
    private IEnumerator AttackDroneCoroutine(GameObject target)
    {
        yield return new WaitForSeconds(0.20f);
        GameObject attackHit = Instantiate(attackHitPrefab, transform.position + new Vector3(0.5f,0,0), Quaternion.identity);
        Destroy(attackHit, 0.5f);

        target.GetComponent<DroneBehaviour>().Die();
        isAttacking = false;
    }
    private IEnumerator AttackChainCoroutine(bool doSlowMotion = false)
    {

        if(doSlowMotion)
        {
            Debug.Log("Slow Motion");
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
        isAttacking = false;
        
    }
}
