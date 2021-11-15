using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : Unlockable
{
    public MapGenerator myMap;
    public GameObject deadEnd, locked, unlocked;
    public MapLoader myMapLoader;
    public DoorLoader myLoader;

    public void LoadNewDoor(int targetX, int targetY)
    {
        unlocked.SetActive(false);
        myLoader.portal = false;
        if (myMapLoader.operatingMap[targetX, targetY] == 'X')
        {
            locked.SetActive(false);
            myLoader.enabled = false;
            deadEnd.SetActive(true);
        }
        else if (myMapLoader.operatingMap[targetX, targetY] == 'D')
        {
            locked.SetActive(true);
            myLoader.enabled = true;
            myLoader.traveled = false;
            deadEnd.SetActive(false);
        }
        if (myMapLoader.CompletedRooms[myMapLoader.currentXLoc, myMapLoader.currentYLoc])
        {
            Unlock();
        }
    }

    public override void Unlock()
    {
        if (locked.activeInHierarchy)
        {
            locked.SetActive(false);
            unlocked.SetActive(true);
        }
    }

    public void LoadPortal(MapLoader.Area targetArea)
    {
        myLoader.SetPortal(targetArea);
    }
}