using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawbotController : MonoBehaviour
{
    public static int health = 3;
    private int healthLeft = health;
    ///public Tilemap myTilemap;
    ///

    

    //public SpriteRenderer sr;

    //   [Header("Damage Colors")]
    //   public Color startColor = Color.white;
    //  public Color endColor = Color.red;

    //   Gradient gradient;
    //   GradientColorKey[] colorKey;
    //   GradientAlphaKey[] alphaKey;


    public void Awake()
    {
        //myTilemap = GameObject.Find("Terrain").GetComponent<Tilemap>();

    }

    public void Start()
    {
        /* colorKey = new GradientColorKey[2];
         colorKey[0].color = startColor;
         colorKey[0].time = 0.0f;
         colorKey[1].color = endColor;
         colorKey[1].time = 1.0f;

         alphaKey = new GradientAlphaKey[2];
         alphaKey[0].alpha = 1.0f;
         alphaKey[0].time = 0.0f;
         alphaKey[1].alpha = 0.0f;
         alphaKey[1].time = 1.0f;

         gradient.SetKeys(colorKey, alphaKey);*/
    }

    // Destroy(this.gameObject);


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
}
