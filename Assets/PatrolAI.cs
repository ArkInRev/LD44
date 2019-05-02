using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{

    public Transform edgeDetect;
    public float speed;
    public bool facingRight=true;
    public LayerMask lm; //looking for ground
    public LayerMask shootAt; //looking for targets
    private bool moving = true;
    [SerializeField] private bool hitGround = false;
    private Renderer r;
    


    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (r.isVisible)
        {
            Vector2 facing;
            facing = (facingRight) ? transform.right : -transform.right;
            //    Vector2 dir = Vector2.left;
            //   dir = (facingRight) ? Vector2.right : -Vector2.right;

            if (moving)
            {
                transform.Translate(facing * speed * Time.deltaTime);

            }


            RaycastHit2D hit;
            hit = Physics2D.Raycast(edgeDetect.position, Vector2.down, 1, lm);
            Debug.DrawRay(edgeDetect.position, Vector2.down, Color.red);
            //Debug.Log(transform.name );
            //Debug.Log("Just hit " + hit);
            if (hit.collider != null)
            {
                hitGround = true;
                //Debug.Log("Robot hit something.");
            }
            else
            {
                // Debug.Log("Robot didn't see ground.");
                if (facingRight == true)
                {
                    Flip();

                }
                else
                {
                    Flip();
                }
                hitGround = false;
            }

            facing = (facingRight) ? transform.right : transform.right;
            hit = Physics2D.Raycast(edgeDetect.position, facing, .25f, lm);
            Debug.DrawRay(edgeDetect.position, facing, Color.yellow);
            //Debug.Log(transform.name );
            //Debug.Log("Just hit " + hit);
            if (hit.collider != null)
            {
                Flip();
            }
            else
            {
                //there is nothing in your way
            }

            facing = (facingRight) ? transform.right : transform.right;
            hit = Physics2D.Raycast(edgeDetect.position, facing, 10, shootAt);
            Debug.DrawRay(edgeDetect.position, facing, Color.green);
            if (hit.collider != null)
            {

                if (hit.collider.CompareTag("Plant"))
                {
                    //Debug.Log("robot sees a plant;");
                    moving = false;
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    //Debug.Log("Robot sees a player.");
                    moving = false;
                }
                else if (hit.collider.CompareTag("Lifeform"))
                {
                    //Debug.Log("robot sees a lifeform;");
                    moving = false;
                }
                else
                {
                    moving = true;
                }

                //Debug.Log("Robot hit something.");
            }
            else
            {
                //Debug.Log("Robot didn't see ground.");
                moving = true;
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Iceblock")
        {
            Flip();
            //Debug.Log("hit an iceblock");
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    public bool HasTarget()
    {
        return !moving;
    }
}
