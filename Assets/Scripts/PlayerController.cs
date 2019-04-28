using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    public GameObject tempIdleTentacle;
    public GameObject tempSlideTentacle;
    public Animator animator;
    public Transform raypoint;
    public Transform whippoint;
    public LineRenderer lr;


    [SerializeField] private float JumpForce = 400f;
    [SerializeField] private float WallKickForce = 400f;
    [SerializeField] private float WallSlideMult = 1.0f;

    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;  
    [SerializeField] private bool AirControl = true;


    [SerializeField] private LayerMask WhatIsWhippable;
    [SerializeField] private LayerMask WhatIsFreezable;
    [SerializeField] private LayerMask WhatIsFlamable;


    [SerializeField] private LayerMask WhatIsGround;


    [SerializeField] private Transform GroundCheck;
    [SerializeField] private Transform GrabWallCheck;
    [SerializeField] private Transform WallKickCheck;
    [SerializeField] private float GroundedRadius = .05f;
    [SerializeField] private float GravityScaleNormal = 2.0f;
    [SerializeField] private float GravityScaleSlow = 0.15f;
    [SerializeField] private bool Grounded;
    [SerializeField] private bool GrabbableWall;
    [SerializeField] private float GrabWallRange = 0.15f;
    [SerializeField] private float WallKickRange = 0.25f;
    private bool WallSliding = false;
    [SerializeField] private bool KickableWall;
    private float recentlyWallSlid = 0.0f;
    private float timeBetweenWallParticle = 0.25f;
    private float timeSinceLastWallParticle = 0.0f;

    private float timeBetweenImpactParticle = 0.15f;
    private float timeUntilNextImpactParticle = 0.0f;


    private float recentlyWhipped = 0.0f;
    [SerializeField] private float timeBetweenWhips = 0.15f;
    [SerializeField] private float timeUntilNextWhip = 0.0f;
    public Collider2D whipCollider;

    private Rigidbody2D rb2d;
    private bool FacingRight = true; 
    private Vector3 Velocity = Vector3.zero;
    public GameObject light; //keep this from flipping. 
    public float lightHeight = -0.5f;

    public ParticleSystem jumpParticles;
    //public ParticleSystem slapParticles;
    public ParticleSystem freezeParticles;
    public ParticleSystem burnParticles;


    //jump logic
    [Header("Jump Logic")]
    // m_grounded - the player is near the ground currently
    [SerializeField] private bool jumpRequest = false; //PlayerMovement indicates that the jump button was pressed
    [SerializeField] private float timeBetweenJumps = 0.25f; // how long before jump can be requested again.
    [SerializeField] private float timeUntilNextJumpRequest = 0.25f; // how long until jump can be pressed again. (manage in fixed update, reset on valid jump request)
    [SerializeField] private bool beginJump = false; //the jump logic indicated that a jump should be started. Toggle with jump logic and landing logic
    [SerializeField] private bool isJumping = false; // Jump has begun. This will be used with the animator. 
    [SerializeField] private bool isLeaping = false; // jumping and has a positive vertical velocity. 
    [SerializeField] private bool isFalling = false; // jumping and has a negative vertical velocity. 
    [SerializeField] private bool beginLanding = false;// was jumping and a grounded check detected ground. 

    [Header("Laser Colors")]
    [Space]
    public Color frostStart = Color.blue;
    public Color frostEnd = Color.blue;
    public Color fireStart = Color.yellow;
    public Color fireEnd = Color.red;
    //Gradient gradient;
    //GradientColorKey[] colorKey;
    //GradientAlphaKey[] alphaKey;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

   [SerializeField] private bool newJump;

    public Tilemap world;
    public GridLayout gridLayout;
    Vector3Int myTilePosition;
    Vector3Int tilePositionBelowMe;
    TileBase tileBelowMe;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

    }
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        myTilePosition = gridLayout.WorldToCell(this.transform.position);
        tilePositionBelowMe = myTilePosition;
        tilePositionBelowMe.y = myTilePosition.y -= 1;
        tileBelowMe = world.GetTile(tilePositionBelowMe);

        bool wasGrounded = Grounded;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                Grounded = true;

               // Debug.Log("Grounded By: " + colliders[i].gameObject.name);

            }
            else
            {
                Grounded = false;
            }

        }

        //check for grabbable walls
        GrabbableWall = IsGrabbableWall();


        Wallslide(WallSliding);


        if (colliders == null || colliders.Length == 0) //if there were no colliders from the circle, then the array must be empty, so the player can't be grounded. 
        {
            Grounded = false;
        }


        timeUntilNextJumpRequest -= Time.deltaTime;
        if (timeUntilNextJumpRequest <= 0)
        {
            timeUntilNextJumpRequest = 0;
        }

        recentlyWallSlid -= Time.deltaTime;
        if (recentlyWallSlid <= 0)
        {
            recentlyWallSlid = 0;
        }

        timeSinceLastWallParticle -= Time.deltaTime;
        if (timeSinceLastWallParticle <= 0)
        {
            timeSinceLastWallParticle = 0;
        }

        timeUntilNextWhip -= Time.deltaTime;
        if (timeUntilNextWhip <= 0)
        {
            timeUntilNextWhip = 0;
        }

        timeUntilNextImpactParticle -= Time.deltaTime;
        if (timeUntilNextImpactParticle <= 0)
        {
            timeUntilNextImpactParticle = 0;
        }
    }


    public void Move(float move, bool jump)
    {
        // check jump logic and see if this is a legal jump
        newJump = checkJumpLogic(jump);


        //only control the player if grounded or airControl is turned on
        if (Grounded || AirControl)
        {

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, rb2d.velocity.y);
            // And then smoothing it out and applying it to the character
            rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, targetVelocity, ref Velocity, MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // Grabbing the wall
        if(!Grounded&&GrabbableWall)
        {
            if(rb2d.velocity.y <= 0) //player must be stationary or descending
            {
                bool prevWallSlide = WallSliding;
                //if they are pressing towards the correct wall, slide down
                if (move < 0 && !FacingRight)
                {
                    // ... slow down
                    WallSliding = true;
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move > 0 && FacingRight)
                {
                    // ... slow down
                    WallSliding = true;
                }
                //did WallSliding Just Toggle?
                if (prevWallSlide != WallSliding) //catch the wall for a moment
                {
                    rb2d.velocity = Vector2.zero;
                }


            } else // ensure that gravity is set correctly because you are going up
            {
                
                WallSliding = false;
            }


        } else // neither ground nor wall are affecting you. 
        {
            WallSliding = false;
        }


        // If the player should jump...
        if (newJump)
        {

            if (Grounded) //normal ground jump
            {

                Grounded = false;
                rb2d.AddForce(new Vector2(0f, JumpForce));
                ParticleSystem dustInstance = Instantiate(jumpParticles, GroundCheck);
            } else if (IsWallKickable() && recentlyWallSlid>0){
                WallSliding = false;
                rb2d.AddForce(new Vector2(move*WallKickForce, WallSlideMult*JumpForce));
                Debug.Log("Move: " + move.ToString() + " Force: " + (move * WallKickForce).ToString());
                ParticleSystem dustInstance = Instantiate(jumpParticles, WallKickCheck);
            }
            
            

        }


    }

    public void Attack(int attackType)
    {
        Transform origin = raypoint;
        if (attackType == 1) origin = whippoint;
        Vector3 facing;
        facing = (FacingRight) ? transform.right : transform.right;
        lr.SetPosition(0, origin.position);

        Vector3 targetpoint = origin.position;

        RaycastHit2D hit;


        switch (attackType)
        {
            case 1:
                Debug.Log("Whip Attack");
                bool whipped = TryToWhip();
                hit = Physics2D.Raycast(origin.position, facing, 2, WhatIsWhippable);
                Debug.DrawRay(origin.position, facing, Color.red);
                if(hit.collider != null)
                {
                    Debug.Log("Slapped a " + hit.collider.name);
                    ImpactEffect(attackType, hit.point);

                    if (hit.collider.CompareTag("Robot"))
                    {
                        //you hit a robot, so add time.delta time to the freeze of that plant. if it freezes long enough, it should iceblock. 
                        hit.collider.GetComponent<SawbotController>().HitWithWhip();
                    }
                    if (hit.collider.CompareTag("Iceblock"))
                    {
                        //you hit an iceblock. Slap a label on it and initiate shipment.  
                        hit.collider.GetComponent<IceblockControl>().ShipThis();
                    }
                    if (hit.collider.CompareTag("Plant"))
                    {
                        //you hit an iceblock. Slap a label on it and initiate shipment.  
                        hit.collider.GetComponent<MushroomInteract>().HitThis();
                    }
                    if (hit.collider.CompareTag("Lifeform"))
                    {
                        //you hit an iceblock. Slap a label on it and initiate shipment.  
                        hit.collider.GetComponent<MushroomInteract>().HitThis();
                    }
                }
                lr.SetPosition(1, targetpoint);
                lr.enabled = false;
                break;
                
            case 2:
                Debug.Log("Shooting Frost");
                lr.startColor = frostStart;
                lr.endColor = frostEnd;
                //bool whipped = TryToWhip();
                hit = Physics2D.Raycast(origin.position, facing, 4, WhatIsFreezable);
                Debug.DrawRay(origin.position, facing, Color.blue);
                if (hit.collider != null)
                {
                    Debug.Log("Froze a " + hit.collider.name);
                    ImpactEffect(attackType, hit.point);
                    targetpoint = hit.point;

                    //what did I hit?
                    // HitWithFrozenRay(float t)
                    if (hit.collider.CompareTag("Plant"))
                    {
                        //you hit a plant, so add time.delta time to the freeze of that plant. if it freezes long enough, it should iceblock. 
                        hit.collider.GetComponent<MushroomInteract>().HitWithFrozenRay(Time.deltaTime);
                    }

                    if (hit.collider.CompareTag("Lifeform"))
                    {
                        //you hit a lifeform, so add time.delta time to the freeze of that lifeform. if it freezes long enough, it should iceblock. 
                        hit.collider.GetComponent<MushroomInteract>().HitWithFrozenRay(Time.deltaTime);
                    }
                }
                else
                {
                    targetpoint = facing * 4 + origin.position;
                }
                lr.SetPosition(1, targetpoint);
                lr.enabled = true;
                break;
            case 3:
                Debug.Log("Shooting Fire");
                lr.startColor = fireStart;
                lr.endColor = fireEnd;
                hit = Physics2D.Raycast(origin.position, facing, 3, WhatIsFlamable);
                Debug.DrawRay(origin.position, facing, Color.yellow);
                if (hit.collider != null)
                {
                    Debug.Log("Burnt a " + hit.collider.name);
                    ImpactEffect(attackType, hit.point);
                    targetpoint = hit.point;

                    if (hit.collider.CompareTag("Destructible"))
                    {
                        //you hit a plant, so add time.delta time to the freeze of that plant. if it freezes long enough, it should iceblock. 
                        hit.collider.GetComponent<DestructibleTiles>().HitWithFireRay(Time.deltaTime);
                    }



                }
                else
                {
                    targetpoint = facing * 3 + origin.position;
                }
                lr.SetPosition(1, targetpoint);
                lr.enabled = true;
                break;
            default:
                lr.enabled = false;
                break;
        }

        
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;

        transform.Rotate(0f, 180f, 0f);
        light.transform.Rotate(0f, 180f, 0f);
    }

    private void Wallslide(bool sliding)
    {
        if (sliding)
        {
            rb2d.gravityScale = GravityScaleSlow; //grabbing the wall
            recentlyWallSlid = timeBetweenJumps;
            SpawnWallSlideParticles();
            animator.SetBool("IsSliding", true);

        } else
        {
            rb2d.gravityScale = GravityScaleNormal; //not grabbing anything
            animator.SetBool("IsSliding", false);
        }
        //TentacleSwapTemp(sliding);
    }



    private bool checkJumpLogic(bool jump)
    {
        //jump logic
        // m_grounded - the player is near the ground currently
        //private bool jumpRequest = false; //PlayerMovement indicates that the jump button was pressed
        //private float timeBetweenJumps = 0.25f; // how long before jump can be requested again.
        //private float timeUntilNextJumpRequest = 0.25f; // how long until jump can be pressed again. (manage in fixed update, reset on valid jump request)
        //private bool beginJump = false; //the jump logic indicated that a jump should be started. Toggle with jump logic and landing logic
        //private bool isJumping = false; // Jump has begun. This will be used with the animator. 
        //private bool isLeaping = false; // jumping and has a positive vertical velocity. 
        //private bool isFalling = false; // jumping and has a negative vertical velocity. 
        //private bool beginLanding = false;// was jumping and a grounded check detected ground. 

        beginJump = false; // reset just to ensure that the logic is clean
        jumpRequest = jump;

        if (jumpRequest) // movement is sending a jump
        {
            if (timeUntilNextJumpRequest <= 0) // the jump request may be eligible
            {
                if (Grounded||IsWallKickable())
                {
                    beginJump = true;
                    timeUntilNextJumpRequest = timeBetweenJumps;
                }
            }


        }
        else
        {
            beginJump = false;
        }

        return beginJump;
    }

    private bool IsGrabbableWall()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GrabWallCheck.position, GrabWallRange, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                GrabbableWall = true;

                // Debug.Log("Grounded By: " + colliders[i].gameObject.name);

            }
            else
            {
                GrabbableWall = false;
            }

        }

        if (colliders == null || colliders.Length == 0) //if there were no colliders from the circle, then the array must be empty, so the player can't be grounded. 
        {
            GrabbableWall= false;
        }


        return GrabbableWall;

    }

    private bool IsWallKickable()
    {
        bool wallKickoff = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(WallKickCheck.position, WallKickRange, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                wallKickoff = true;

                // Debug.Log("Grounded By: " + colliders[i].gameObject.name);

            }
            else
            {
                wallKickoff = false;
            }

        }

        if (colliders == null || colliders.Length == 0) //if there were no colliders from the circle, then the array must be empty, so the player can't be grounded. 
        {
            wallKickoff = false;
        }


        return wallKickoff;

    }

    private void SpawnWallSlideParticles()
    {
        if(timeSinceLastWallParticle <= 0)
        {
            Instantiate(jumpParticles, GrabWallCheck);
            timeSinceLastWallParticle = timeBetweenWallParticle;
        }
    }

    private void ImpactEffect(int type,Vector2 point)
    {
        ParticleSystem particles;
        switch (type)
        {
            case 2:
                particles = freezeParticles;
                break;
            case 3:
                particles = burnParticles;
                break;
            default:
                particles = jumpParticles;
                break;
        }

        if (timeUntilNextImpactParticle <= 0)
        {
            

            Instantiate(particles, new Vector3(point.x, point.y,0),Quaternion.identity);
            timeUntilNextImpactParticle = timeBetweenImpactParticle;
        }
    }


    private bool TryToWhip()
    {
        if (timeUntilNextWhip <= 0) //enough time has elapsed
        {
            animator.Play("Alien_Whip");
            timeUntilNextWhip = timeBetweenWhips;
            //cast a ray and detect the hit




            return true;
        }
        return false;
    }


    private void TentacleSwapTemp(bool sliding)
    {
        tempSlideTentacle.SetActive(sliding);
        tempIdleTentacle.SetActive(!sliding);

    }

}