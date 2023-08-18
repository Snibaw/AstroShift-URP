using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float speedAdd = 0.01f;
    public Rigidbody2D rb;
    public bool isGrounded = false;
    public bool Jumping = false;
    private Animator playerAnimator;
    private bool dontCheckIfGrounded = false;
    public bool canMove = true;
    [SerializeField] private float antiRebondTimerMax = 0.01f;
    private float antiRebondTimer;
    private bool isTouching = false;
    private bool wasTouching = false;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        antiRebondTimer = antiRebondTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        //Move the player
        speed += speedAdd;
        rb.velocity = new Vector2(speed, rb.velocity.y);
        
        if(!canMove) return;
        //Check if the player is grounded
        CheckIfGrounded();

        if(Input.touchCount > 0)
        {
            isTouching = true;
        }
        else
        {
            isTouching = false;
        }

        //If the player press left click make the gravity change
        if ((Input.GetMouseButtonDown(0) && antiRebondTimer <= 0) || (isTouching && !wasTouching))
        {
            antiRebondTimer = antiRebondTimerMax;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.gravityScale = -rb.gravityScale;
            
            dontCheckIfGrounded = true;
            Invoke("FlipSprite",0.2f);
            // if(isGrounded) 
            // {
            //     JumpAnimation();
            //    // Invoke("FlipSprite", 0.5f);
            // }
            // else
            // {
            //     FlipSprite();
            // }
        }

        wasTouching = isTouching;

        if(!isGrounded && !Jumping)
        {
            JumpAnimation();
        }

        antiRebondTimer -= Time.deltaTime;
    }

    private void CheckIfGrounded()
    {
        if(dontCheckIfGrounded) return;
        isGrounded = Physics2D.OverlapCircle(GameObject.Find("GroundCheck").transform.position, 0.1f, LayerMask.GetMask("Ground"));

        if(isGrounded && Jumping)
        {
            Jumping = false;
            playerAnimator.SetBool("Jumping", false);
        }
    }
    private void FlipSprite()
    {
        //Flip the sprite
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        dontCheckIfGrounded = false;
    }
    private void JumpAnimation()
    {
        Jumping = true;
        playerAnimator.SetBool("Jumping", true);
    }
}
