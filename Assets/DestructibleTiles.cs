using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTiles : MonoBehaviour
{
    public float timeToBreak = 3f;
    [SerializeField] private float timeFireRay = 0f;
    public Tilemap myTilemap;
    public Tilemap gooTilemap;


    public Color meltRed;
    [SerializeField] private Vector3Int thisTilepos;
    //public Tile thisTile;

   

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

        //thisTile = (Tile)myTilemap.GetTile(thisTilepos);


    }

    public void Start()
    {

        thisTilepos = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
        myTilemap.SetTileFlags(thisTilepos, TileFlags.None);
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
        meltSpriteColor(timeFireRay / timeToBreak);
        if (timeFireRay > timeToBreak)
        {
            breakThis();

        }
    }

    private void breakThis()
    {
        
        
        myTilemap.SetTile(thisTilepos, null);
       gooTilemap.SetTile(thisTilepos, null);
    }

    public void meltSpriteColor(float t)
    {
        Color lerpedColor = Color.Lerp(Color.white, meltRed, t);
        myTilemap.SetColor(thisTilepos, lerpedColor);
       //thisTile.color = lerpedColor;

    }

}
