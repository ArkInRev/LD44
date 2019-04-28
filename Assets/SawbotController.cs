using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SawbotController : MonoBehaviour
{
    [SerializeField] public int health = 8;
    private int healthLeft = 3;
    public PatrolAI pat;
    [SerializeField] public float timeBetweenShots = 1.5f;
    public float timeUntilNextShot = 1.5f;
    public GameObject sawblade;
    public Transform ProjecTilemap;

    public void Awake()
    {
        ProjecTilemap = GameObject.Find("ProjectileTilemap").transform;
        timeUntilNextShot = timeBetweenShots;
        healthLeft = health;
    }

    public void Start()
    {

    }


    private void FixedUpdate()
    {
        timeUntilNextShot -= Time.deltaTime;
        ShootCheck();  
        
    }

    public void HitWithWhip()
    {
        healthLeft -= 1;
        if (healthLeft <= 0)
        {
            breakThis();

        }
    }

    private void breakThis()
    {
        // create something bad here

        //then destroy
        Destroy(this.gameObject);


    }

    private void ShootCheck()
    {
        if (timeUntilNextShot <= 0)
        {
            timeUntilNextShot = 0;
            if (pat.HasTarget())
            {
                GameObject go = Instantiate(sawblade, transform) as GameObject;
                timeUntilNextShot = timeBetweenShots;
                go.transform.SetParent(ProjecTilemap);
                Destroy(go, 4);
            }
        }
    }

    

}
