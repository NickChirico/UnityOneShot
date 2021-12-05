using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : Unlockable
{
    public GameObject checkSymbol, lockSymbol;
    public MapGenerator myMap;
    public GameObject deadEnd, locked, unlocked, door;
    //public GameObject deadEndWoods, deadEndChurch, deadEndMarket, deadEndAcademy;
    public MapLoader myMapLoader;
    public DoorLoader myLoader;
    public MapLoader.Area currentArea;

    public void LoadNewDoor(int targetX, int targetY)
    {
        unlocked.SetActive(false);
        myLoader.portal = false;
        checkSymbol.SetActive(myMapLoader.GetCompletionMap()[targetX, targetY]);
        if (myMapLoader.GetAreaMap()[targetX, targetY] == 'X')
        {
            locked.SetActive(false);
            myLoader.enabled = false;
            deadEnd.SetActive(true);
        }
        else if (myMapLoader.GetAreaMap()[targetX, targetY] == 'D' || myMapLoader.GetAreaMap()[targetX, targetY] == 'H' ||
                 myMapLoader.GetAreaMap()[targetX, targetY] == 'B' ||myMapLoader.GetAreaMap()[targetX, targetY] == 'S' ||myMapLoader.GetAreaMap()[targetX, targetY] == 'U')
        {
            locked.SetActive(true);
            lockSymbol.SetActive(true);
            myLoader.enabled = true;
            myLoader.traveled = false;
            deadEnd.SetActive(false);
            //deadEndWoods.SetActive(false);
            //deadEndChurch.SetActive(false);
            //deadEndMarket.SetActive(false);
            //deadEndAcademy.SetActive(false);
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
            //locked.SetActive(false);
            unlocked.SetActive(true);
            lockSymbol.SetActive(false);
        }
    }

    public void LoadPortal(MapLoader.Area targetArea)
    {
        unlocked.SetActive(true);
        myLoader.SetPortal(targetArea);
    }
}