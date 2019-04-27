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


    void Start()
    {

    }


    void Update()
    {


        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
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

    void OnTriggerEnter2D(Collider2D other)
    {

    }


}
