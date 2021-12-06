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
        myLoader.portal = false;
        if (targetX >= myMap.roomSizes[myMap.GetCurrentTier()] || targetX < 0 || targetY >= myMap.roomSizes[myMap.GetCurrentTier()]
            || targetY < 0 || myMapLoader.complexMap[targetX, targetY] == "X") //there is no neighboring room
        {
            door.SetActive(false);
            unlocked.SetActive(false);
            lockSymbol.SetActive(false);
            checkSymbol.SetActive(false);
            myLoader.enabled = false;
        }
        else
        {
            locked.SetActive(true);
            lockSymbol.SetActive(true);
            myLoader.enabled = true;
            myLoader.traveled = false;
            deadEnd.SetActive(false);
        }
        checkSymbol.SetActive(myMapLoader.complexMap[targetX, targetY].StartsWith("*")); //check mark appears for completed rooms
        /*
        if (myMapLoader.complexMap[targetX, targetY].Contains("D") || myMapLoader.complexMap[targetX, targetY].Contains("H") || myMapLoader.complexMap[targetX, targetY].Contains("B")
            || myMapLoader.complexMap[targetX, targetY].Contains("S") || myMapLoader.complexMap[targetX, targetY].Contains("R") || myMapLoader.complexMap[targetX, targetY].Contains("C"))
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
        */
        if (!myMapLoader.complexMap[myMapLoader.currentXLoc, myMapLoader.currentYLoc].StartsWith("*") || myMapLoader.complexMap[targetX, targetY].Contains("S") || myMapLoader.complexMap[targetX, targetY].Contains("H")
            || myMapLoader.complexMap[targetX, targetY].Contains("R") || myMapLoader.complexMap[targetX, targetY].Contains("C")) //if we cleared out this room, unlock this door
        {
            Unlock();
        }
    }

    public override void Unlock()
    {
        print("unlocking");
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