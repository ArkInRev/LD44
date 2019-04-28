using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{

    public Transform edgeDetect;
    public float speed;
    public bool facingRight=true;
    public LayerMask lm;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.captureFramerate);
        RaycastHit2D hit;
        hit = Physics2D.Raycast(edgeDetect.position,Vector2.down,1,lm);
        Debug.DrawRay(edgeDetect.position, Vector2.down, Color.red);
        if (hit.collider == null)
        {
            if(facingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingRight = false;
            }
        }

    }
}
