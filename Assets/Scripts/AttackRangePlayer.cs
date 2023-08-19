using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangePlayer : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private GameObject objectParentCollidedWith;
    
    private UnityEngine.Rendering.Universal.Light2D light;
    [SerializeField] private GameObject lightPrefab;
    [SerializeField] private float lightRangeMax;
    [SerializeField] private float lightRangeStep;
    [SerializeField] private float lightRangeTime;
    [SerializeField] private float timeBtwOnOff;
    [SerializeField] private float offRatioMultiplier;

    private void Start() {
        playerAttack = GetComponentInParent<PlayerAttack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chain"))
        {
            if(collision.gameObject.transform.parent.gameObject == objectParentCollidedWith)
            {
                return;
            }
            objectParentCollidedWith = collision.gameObject.transform.parent.gameObject;
            playerAttack.Attack(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Drone"))
        {
            if(collision.gameObject == objectParentCollidedWith)
            {
                return;
            }
            objectParentCollidedWith = collision.gameObject;
            playerAttack.Attack(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("MonsterKiller"))
        {
            if(collision.gameObject == objectParentCollidedWith)
            {
                return;
            }
            objectParentCollidedWith = collision.gameObject;
            playerAttack.Attack(collision.gameObject);
        }
    }
    public void HitSuspendedWallAnimation()
    {
        //"Animation"
        StartCoroutine(AnimateHitSuspendedWall());

        //Light
        // GameObject lightObject = Instantiate(lightPrefab, objectParentCollidedWith.transform.position, Quaternion.identity);
        // Destroy(lightObject, 5f);
        // light = lightObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        // light.pointLightOuterRadius = 0f;
        // StartCoroutine(ModifyLightRange(0,lightRangeMax,lightRangeStep));
    }
    // private IEnumerator ModifyLightRange(float start, float end, float step)
    // {
    //     float current = start;
    //     while (current <= end)
    //     {
    //         current += step;
    //         light.pointLightOuterRadius = current;
    //         yield return new WaitForSeconds(lightRangeTime);
    //     }

    //     yield return new WaitForSeconds(timeBtwOnOff);

    //     FadeAwayObjectsCollidedWith();
        
    //     while (current >= start)
    //     {
    //         current -= offRatioMultiplier*step;
    //         light.pointLightOuterRadius = current;
    //         yield return new WaitForSeconds(lightRangeTime);
    //     }        
    // }
    private IEnumerator AnimateHitSuspendedWall()
    {
        yield return new WaitForSeconds(0.25f);

        objectParentCollidedWith.transform.GetChild(0).gameObject.GetComponent<SpikeObstacleSpawnCoin>().CheckIfBonus();

        GameObject.Find("GameManager").GetComponent<GameManager>().DoAnimationSuspendedWall(objectParentCollidedWith);

        FadeAwayObjectsCollidedWith();
    }
    private void FadeAwayObjectsCollidedWith()
    {
        foreach(Animator animator in objectParentCollidedWith.GetComponentsInChildren<Animator>())
        {
            animator.SetTrigger("FadeAway");
        }
    }
    

}
