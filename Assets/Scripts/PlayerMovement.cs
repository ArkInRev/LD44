using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Manage the movement states and animations to keep them in sync

    public PlayerController controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    public Rigidbody2D rb2d;

    [SerializeField]
    bool jump = false;

    [SerializeField]
    bool isJumping = false;
    [SerializeField]
    bool isFalling = false;
    [SerializeField]
    private bool isWhip, isFrost, isFlame = false;

    public Animator animator;


    void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }


    void Update()
    {


        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            isWhip = true;
        }
 //       if (Input.GetButtonUp("Fire1"))
 //       {
 //           isWhip = false;
 //       }
        if (Input.GetButtonDown("Fire2"))
        {
            isFrost = true;
        }
        if (Input.GetButtonDown("Fire3"))
        {
            isFlame = true;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            isFrost = false;
            OnAttackFrost(false);
        }
        if (Input.GetButtonUp("Fire3"))
        {
            isFlame = false;
            OnAttackFire(isFlame);
        }


    }

    void FixedUpdate()
    {

        //move the character
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;

        // set whether the character is jumping or falling
        if (rb2d.velocity.y < -0.2f)
        {
            OnFalling(true);
        }
        else
        {
            OnFalling(false);
        }

        if (rb2d.velocity.y > 0.2f)
        {
            OnJumping(true);
        }
        else
        {
            OnJumping(false);
        }

        //handle shooting inputs. 
        int attackType = 0;
        if (isWhip)
        {
            // do the whip animation
            OnAttackWhip(true);
            attackType = 1;
            isWhip = false;
            isFlame = false;
            isFrost = false;
        } else if (isFrost)
        {
            // do the frost animation
            OnAttackFrost(true);
            attackType = 2;
            isWhip = false;
            isFlame = false;

        } else if (isFlame)
        {
            // do the fire animation
            OnAttackFire(true);
            attackType = 3;
            isWhip = false;
            isFrost = false;
        } else
        {
            attackType = 0; //in case of anything else, no attack
            isWhip = false;
            isFrost = false;
            isFlame = false;
        }
        controller.Attack(attackType);
       // OnAttackWhip(false);

    }

    public void OnLanding()
    {
        //animator.SetBool("IsGrounded", true);
    }


    public void OnJumping(bool isJumpingNow)
    {
        //animator.SetBool("IsJumping", isJumpingNow);
    }

    public void OnFalling(bool isFallingNow)
    {
        //animator.SetBool("IsFalling", isFallingNow);
    }

    public void OnAttackFrost(bool state)
    {
        animator.SetBool("Firing_frost", state);
    }

    public void OnAttackFire(bool state)
    {
        animator.SetBool("Firing_flame", state);
    }

    public void OnAttackWhip(bool state)
    {
        //animator.Play("Alien_Whip");
    }


    void OnTriggerEnter2D(Collider2D other)
    {

    }


}
