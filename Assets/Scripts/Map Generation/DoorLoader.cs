using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLoader : MonoBehaviour
{
    public string direction;
    public MapLoader myMapLoader;
    public bool traveled, portal;
    public MapLoader.Area destination;

    void Start()
    {
        myMapLoader = GameObject.Find("Map Manager").GetComponent<MapLoader>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        /*if (portal)
        {
            myMapLoader.currentArea = destination;
            myMapLoader.LoadArea(direction);
        }
        else
        {
            if (other.CompareTag("Player") && !traveled)
            {
                traveled = true;
                myMapLoader.LoadRoom(direction);
            }
        }*/
        if (other.CompareTag("Player") && !traveled)
        {
            traveled = true;
            if (portal)
            {
                myMapLoader.CompleteArea();
            }
            else
            {
                myMapLoader.Travel(direction);
            }
            
        }
    }

}
 