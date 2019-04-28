using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Projectile : MonoBehaviour
{

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
