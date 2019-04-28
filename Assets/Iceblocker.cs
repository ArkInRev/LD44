using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceblocker : MonoBehaviour
{

    public GameObject frozenMushroom;
    public GameObject frozenLifeform;


    public void NewIceblock(int thing, Transform here)
    {
        GameObject spawnThis = frozenMushroom;
        switch (thing)
        {
            case 1:
                spawnThis = frozenMushroom;
                break;
            case 2:
                spawnThis = frozenLifeform;
                break;
            default:
                break;
        }
        

        //Debug.Log("Got the message and instantiating. "+spawnThis.name+" "+here.position);
        GameObject go = Instantiate(spawnThis, here) as GameObject;
        //Debug.Log("Instantiated: " + spawnThis.name + " " + here.position+", now what?");
    }
}
