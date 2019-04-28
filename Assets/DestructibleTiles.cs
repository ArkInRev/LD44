using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTiles : MonoBehaviour
{
    public float timeToBreak = 3f;
    private float timeFireRay = 0f;
    public Tilemap myTilemap;
    public Tilemap gooTilemap;


    public SpriteRenderer sr;

 //   [Header("Damage Colors")]
 //   public Color startColor = Color.white;
  //  public Color endColor = Color.red;

 //   Gradient gradient;
 //   GradientColorKey[] colorKey;
 //   GradientAlphaKey[] alphaKey;


    public void Awake()
    {
        myTilemap = GameObject.Find("Terrain").GetComponent<Tilemap>();
        gooTilemap = GameObject.Find("Goo").GetComponent<Tilemap>();

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


    public void HitWithFireRay(float t)
    {
        timeFireRay += t;
        if (timeFireRay > timeToBreak)
        {
            breakThis();

        }
    }

    private void breakThis()
    {
        Vector3Int thisTile = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y),Mathf.FloorToInt(transform.position.z));
        
        myTilemap.SetTile(thisTile, null);
       gooTilemap.SetTile(thisTile, null);
    }

}
