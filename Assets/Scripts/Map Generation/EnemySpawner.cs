using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool roomComplete;
    public int numToSpawn;
    public MapLoader myMapLoader;
    public GameObject[] allEnemies;
    public Transform[] spawnLocations;

    public void SpawnEnemies(int roomPosX, int roomPosY)
    {
        switch (myMapLoader.currentArea)
        {
            case MapLoader.Area.Start:
                roomComplete = true;
                myMapLoader.northDoor.Unlock();
                myMapLoader.eastDoor.Unlock();
                myMapLoader.southDoor.Unlock();
                myMapLoader.westDoor.Unlock();
                break;
            case MapLoader.Area.Woods:
                roomComplete = myMapLoader.CompletedWoods[myMapLoader.currentXLoc, myMapLoader.currentYLoc];
                break;
            case MapLoader.Area.Church:
                roomComplete = myMapLoader.CompletedChurch[myMapLoader.currentXLoc, myMapLoader.currentYLoc];
                break;
            case MapLoader.Area.Market:
                roomComplete = myMapLoader.CompletedMarket[myMapLoader.currentXLoc, myMapLoader.currentYLoc];
                break;
            case MapLoader.Area.Academy:
                roomComplete = myMapLoader.CompletedAcademy[myMapLoader.currentXLoc, myMapLoader.currentYLoc];
                break;
        }
        switch (roomComplete)
        {
            case true:
                numToSpawn = 0;
                break;
            case false:
                numToSpawn = Random.Range(2, 6);
                break;
        }
        for (int i = 0; i < numToSpawn; i++)
        {
            int selection = Random.Range(0, allEnemies.Length);
            if (!allEnemies[selection].activeInHierarchy)
            {
                allEnemies[selection].transform.position = spawnLocations[selection].position;
                allEnemies[selection].SetActive(true);
                allEnemies[selection].GetComponent<ShootableEntity>().ResetHealth();
            }
            else
            {
                i--;
            }
        }
    }

    public void FinishRoom()
    {
        //roomComplete = true;
        switch (myMapLoader.currentArea)
        {
            case MapLoader.Area.Start:
                myMapLoader.CompletedRooms[myMapLoader.currentXLoc, myMapLoader.currentYLoc] = true;
                break;
            case MapLoader.Area.Woods:
                myMapLoader.CompletedWoods[myMapLoader.currentXLoc, myMapLoader.currentYLoc] = true;
                break;
            case MapLoader.Area.Church:
                myMapLoader.CompletedChurch[myMapLoader.currentXLoc, myMapLoader.currentYLoc] = true;
                break;
            case MapLoader.Area.Market:
                myMapLoader.CompletedMarket[myMapLoader.currentXLoc, myMapLoader.currentYLoc] = true;
                break;
            case MapLoader.Area.Academy:
                myMapLoader.CompletedAcademy[myMapLoader.currentXLoc, myMapLoader.currentYLoc] = true;
                break;
        }
        
        for (int i = 0; i < myMapLoader.allUnlockables.Length; i++)
        {
            myMapLoader.allUnlockables[i].Unlock();
        }
        myMapLoader.northDoor.Unlock();
        myMapLoader.eastDoor.Unlock();
        myMapLoader.southDoor.Unlock();
        myMapLoader.westDoor.Unlock();
    }

    public void CheckEnemiesAlive()
    {
        bool willEnd = true;
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].activeInHierarchy)
            {
                willEnd = false;
            }
        }
        if (willEnd)
        {
            FinishRoom();
        }
    }
}