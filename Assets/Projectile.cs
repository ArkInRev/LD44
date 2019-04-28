using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Projectile : MonoBehaviour
{
    public float destroyDelay = 0.1f;
    public float speed;
    public float damageInSeconds = 30;
    public float damageInHits = 1;

    public GameController gc;

    // Start is called before the first frame update
    void Awake()
    {
        gc = GameObject.Find("GameManager").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = (transform.rotation.y > 0) ? transform.right : -transform.right;
        //Debug.Log("rotation y " + transform.rotation.y);

       
        //Vector2 facing;
        //facing = (facingRight) ? transform.right : -transform.right;
        //    Vector2 dir = Vector2.left;
        //   dir = (facingRight) ? Vector2.right : -Vector2.right;

      //  if (moving)
       // {
            //transform.Translate(facing * speed * Time.deltaTime);
            transform.Translate(dir *speed * Time.deltaTime);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Iceblock")
        {
            Destroy(this.gameObject,destroyDelay);
            
        }
        if (collision.gameObject.tag == "Destructible")
        {
            Destroy(this.gameObject, destroyDelay);

        }
        if (collision.gameObject.tag == "Player")
        {
            gc.ReduceTimer(damageInSeconds);
            Destroy(this.gameObject, destroyDelay);

        }
        if (collision.gameObject.tag == "Plant")
        {
            //gc.ReduceTimer(damageInSeconds);
            collision.GetComponent<MushroomInteract>().HitThis();
            Destroy(this.gameObject, destroyDelay);

        }
        if (collision.gameObject.tag == "Lifeform")
        {
            //gc.ReduceTimer(damageInSeconds);
            collision.GetComponent<MushroomInteract>().HitThis();
            Destroy(this.gameObject, destroyDelay);

        }

    }

}
