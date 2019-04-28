using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceblockControl : MonoBehaviour
{
    public int lifeType=0;
    public int lifeValue=1;
    public Animator anim;
    public GameController gc;
    public Collider2D col;
    public ParticleSystem ps;


    public void Awake()
    {
        gc = GameObject.Find("GameManager").GetComponent<GameController>();
        ps.Stop();
    }


    public void ShipThis()
    {
        string animName;
        switch (lifeType)
        {
            case 0:
                animName = "Iceblock_Ship_Plant";
                break;
            case 1:
                animName = "Iceblock_Ship_Lifeform";
                break;
            default:
                animName = "Iceblock_Ship_Plant";
                break;

        }
        // change the animation
        anim.SetBool("ShipMe", true);
        ps.Play();
        // change to background layer
        //this.gameObject.layer = 2;
        // remove any colliders
        //col.enabled = false;

        //adjust the score
        gc.AddCurrency(lifeType, lifeValue);
        // set the destroy delay
        Destroy(this.gameObject, 3);
    }
}
