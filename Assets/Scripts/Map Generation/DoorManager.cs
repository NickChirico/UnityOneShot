using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : Unlockable
{
    public MapGenerator myMap;
    public GameObject deadEnd, locked, unlocked;
    public GameObject deadEndWoods, deadEndChurch, deadEndMarket, deadEndAcademy;
    public MapLoader myMapLoader;
    public DoorLoader myLoader;
    public MapLoader.Area currentArea;

    public void LoadNewDoor(int targetX, int targetY)
    {
        unlocked.SetActive(false);
        myLoader.portal = false;
        if (myMapLoader.GetAreaMap()[targetX, targetY] == 'X')
        {
            locked.SetActive(false);
            myLoader.enabled = false;
            switch (currentArea)
            {
                case MapLoader.Area.Start:
                    deadEnd.SetActive(true);
                    break;
                case MapLoader.Area.Woods:
                    deadEndWoods.SetActive(true);
                    break;
                case MapLoader.Area.Church:
                    deadEndChurch.SetActive(true);
                    break;
                case MapLoader.Area.Market:
                    deadEndMarket.SetActive(true);
                    break;
                case MapLoader.Area.Academy:
                    deadEndAcademy.SetActive(true);
                    break;
            }
        }
        else if (myMapLoader.GetAreaMap()[targetX, targetY] == 'D' || myMapLoader.GetAreaMap()[targetX, targetY] == 'H' || myMapLoader.GetAreaMap()[targetX, targetY] == 'B')
        {
            locked.SetActive(true);
            myLoader.enabled = true;
            myLoader.traveled = false;
            deadEnd.SetActive(false);
            deadEndWoods.SetActive(false);
            deadEndChurch.SetActive(false);
            deadEndMarket.SetActive(false);
            deadEndAcademy.SetActive(false);
        }
        if (myMapLoader.GetCompletionMap()[myMapLoader.currentXLoc, myMapLoader.currentYLoc])
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
        unlocked.SetActive(true);
        myLoader.SetPortal(targetArea);
    }
}