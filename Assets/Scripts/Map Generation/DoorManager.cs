using System;
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

    void Awake()
    {
        myMap = GameObject.Find("Map Generator").GetComponent<MapGenerator>();
    }

    public void LoadNewDoor(int targetX, int targetY)
    {
        unlocked.SetActive(false);
        if (myLoader.portal)
        {
            unlocked.SetActive(false);
            door.SetActive(true);
            //print("neighbor at " + targetX + ", " + targetY);
            locked.SetActive(true);
            lockSymbol.SetActive(true);
            myLoader.enabled = true;
            myLoader.traveled = false;
            deadEnd.SetActive(false);
            checkSymbol.SetActive(!myMapLoader.ComplexMap[targetX, targetY].StartsWith("*"));
            if (!myMapLoader.ComplexMap[myMapLoader.currentRank, myMapLoader.currentFile].StartsWith("*")) //if we cleared out this room, unlock this door
            {
                Unlock();
            }
        }
        else
        {
            if (targetX >= myMap.roomSizes[myMap.GetCurrentTier()] || targetX < 0 || targetY >= myMap.roomSizes[myMap.GetCurrentTier()]
                || targetY < 0 || myMapLoader.ComplexMap[targetX, targetY] == "X") //there is no neighboring room
            {
                //print("no neighbor at " + targetX + ", " + targetY);
                door.SetActive(false);
                unlocked.SetActive(false);
                lockSymbol.SetActive(false);
                checkSymbol.SetActive(false);
                locked.SetActive(false);
                myLoader.enabled = false;
                deadEnd.SetActive(true);
                checkSymbol.SetActive(false);
            }
            else
            {
                unlocked.SetActive(false);
                door.SetActive(true);
                //print("neighbor at " + targetX + ", " + targetY);
                locked.SetActive(true);
                lockSymbol.SetActive(true);
                myLoader.enabled = true;
                myLoader.traveled = false;
                deadEnd.SetActive(false);
                checkSymbol.SetActive(!myMapLoader.ComplexMap[targetX, targetY].StartsWith("*"));
                if (!myMapLoader.ComplexMap[myMapLoader.currentRank, myMapLoader.currentFile].StartsWith("*")) //if we cleared out this room, unlock this door
                {
                    Unlock();
                }
            }
        }
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
        
    }

    public override void Unlock()
    {
        //print("unlocking");
        if (locked.activeInHierarchy)
        {
            //locked.SetActive(false);
            unlocked.SetActive(true);
            lockSymbol.SetActive(false);
        }
    }

}