using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipControl : MonoBehaviour
{
    public float whipForce = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 /*   void OnTriggerEnter2D(Collision2D collider)
    {
        //Debug.Log(collider.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        
            if (collider.gameObject.CompareTag("Robot"))
            {
                Debug.Log("Hit a Robot.");
            //push it back



                
            } else if (collider.gameObject.CompareTag("Lifeform"))
            {
                Debug.Log("Hit a Lifeform.");
                // push it back
            }
            else if (collider.gameObject.CompareTag("Plant"))
            {
                Debug.Log("Hit a Plant.");
                // do nothing
            }
            else if (collider.gameObject.CompareTag("Pickup"))
            {
                Debug.Log("Hit a Pickup.");
                //collect the pickup
            }
            else if (collider.gameObject.CompareTag("Iceblock"))
            {
                Debug.Log("Hit an Iceblock.");
                // break the iceblock
                //drop the pickups

            }
            else
            {

            }


    }*/
}
