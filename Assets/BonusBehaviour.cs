using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBehaviour : MonoBehaviour
{
    [SerializeField] private float bonusSpeed = 10f;
    public bool doMoveToPlayer = false;
    private Transform playerTransform;

    private void Start() {
        playerTransform = GameObject.Find("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if(doMoveToPlayer)
        {
            transform.parent = playerTransform;
            //Smoothly move the bonus to the player
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, bonusSpeed * Time.deltaTime);
        }
    }
    public void PickUpBonusCoroutine()
    {
        if(doMoveToPlayer) return;
        doMoveToPlayer = true;
        gameObject.GetComponent<Animator>().SetTrigger("Move");
        Destroy(gameObject, 5f);
    }
}
