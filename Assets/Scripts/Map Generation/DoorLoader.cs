using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLoader : MonoBehaviour
{
    public string direction;
    public MapLoader myMapLoader;
    public bool traveled, portal;

    private MapLoader.Area destination;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !traveled)
        {
            traveled = true;
            myMapLoader.LoadRoom(direction);
        }
    }

    public void SetPortal(MapLoader.Area portalDestination)
    {
        portal = true;
        destination = portalDestination;
    }
}
