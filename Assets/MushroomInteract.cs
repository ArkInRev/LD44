using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomInteract : MonoBehaviour
{

    public float timeToFreeze = 1.5f;
    private float timeFrozenRay = 0f;
    public GameObject IceBlockVersion;
    Rigidbody2D rb2d;
    public Collider2D c2d;
    public Transform myTilemap;
    public int healthInHits = 2;
    //private IEnumerator coroutine;


    public void Awake()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        myTilemap=  GameObject.Find("Others").transform;

    }

           // Destroy(this.gameObject);


    public void HitWithFrozenRay(float t)
    {
        timeFrozenRay += t;
        if (timeFrozenRay > timeToFreeze)
        {
            IceblockThis();
            
        }
    }

    private void IceblockThis()
    {
        c2d.enabled = false; //disable the collider
        //spawn the new thing
        NewIceblock(IceBlockVersion, transform);
        Debug.Log("Should have called for a new iceblock.");
        //StartCoroutine(coroutine);

    }

    public void NewIceblock(GameObject ice, Transform here)
    {

        GameObject go = Instantiate(ice, here) as GameObject;
        go.transform.SetParent(myTilemap);
        Destroy(this.gameObject);

    }

    public void HitThis()
    {
        healthInHits -= 1;
        if (healthInHits <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
