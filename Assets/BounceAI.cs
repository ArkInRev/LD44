using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAI : MonoBehaviour
{
    public bool randomDir = true; // by default, bounce a random direction
    public float jumpForce = 200; //jump height controller
    public float forwardForce = 100; // force horizontal

    public Transform groundCheck;
    public LayerMask lm;
    private Renderer r;

    private Rigidbody2D rb2d;

    [SerializeField] private bool isGrounded = true;

    public float timeBetweenBounce = 3;
    public float timeUntilNextBounce = 3;
    public float jumpOffset = 2;

    [SerializeField] private bool facingRight = false;
    [SerializeField] private bool hitGround = true;

    public void Awake()
    {
        timeUntilNextBounce = timeBetweenBounce + (Random.Range(0,jumpOffset+1)-(jumpOffset/2));
        rb2d = GetComponent<Rigidbody2D>();
        r = GetComponent<SpriteRenderer>();
    }

    public void FixedUpdate()
    {

        // Vector2 facing;
        // facing = (facingRight) ? transform.right : -transform.right;
        if (r.isVisible)
        {
            isGrounded = GroundedCheck();
            JumpLogic();
        }
        

    }

    private void JumpLogic()
    {

        if (isGrounded)
        {
            if (timeUntilNextBounce <= 0)
            {
                int bounceDir = 0;
                if (randomDir)
                {
                    bounceDir = Random.Range(0, 2);
                }
                else
                {
                    //bounce toward facing
                    bounceDir = 0;
                }
                // impulse a force on the thing if it is grounded and the time between jumps has elapsed
                Bounce(bounceDir);

            }
            else
            {
                timeUntilNextBounce -= (Time.deltaTime);
            }

        }

    }

    public void Bounce(int dir)
    {

        float forceX = (dir == 0) ? -1 *forwardForce : forwardForce;
        float forceY = jumpForce;
        
        Vector2 bounceForce = new Vector2(forceX, forceY);
        rb2d.AddForce(bounceForce);
        timeUntilNextBounce = timeBetweenBounce;
    }

     public bool GroundedCheck()
    { 

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, lm);
        Debug.DrawRay(groundCheck.position, Vector2.down, Color.red);

        if (hit.collider != null)
        {
            hitGround = true;
    //        Debug.Log("Bounce hit something. "+hit.collider.name );
        }
        else
       {
    //        Debug.Log("Bounce didn't see ground.");
            hitGround = false;
       }



        return hitGround;
    }

    private bool GetFacing()
    {
        if(transform.rotation.y == 180)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void Flip() // will need for wall collisions
    {
        // Switch the way the thing is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
