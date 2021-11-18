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

    public void SpawnEnemies()
    {
        /*
        switch (myMapLoader.currentArea) //can use GetCompletionMap function
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
        //print(roomComplete);
        switch (roomComplete)
        {
            case true:
                numToSpawn = 0;
                break;
            case false:
                numToSpawn = Random.Range(2, 6);
                break;
        }*/
        switch (myMapLoader.GetAreaMap()[myMapLoader.currentXLoc, myMapLoader.currentYLoc])
        {
            case 'H':
                //numToSpawn = 0;
                FinishRoom();
                break;
            case 'D':
                if (!myMapLoader.GetCompletionMap()[myMapLoader.currentXLoc, myMapLoader.currentYLoc])
                {
                    numToSpawn = Random.Range(2, 6);
                    for (int i = 0; i < numToSpawn; i++)
                    {
                        int selection = Random.Range(0, allEnemies.Length - 1);
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
                break;
            case 'S':
                //numToSpawn = 0;
                FinishRoom();
                break;
            case 'U':
                //numToSpawn = 0;
                FinishRoom();
                break;
            case 'B':
                if (!myMapLoader.GetCompletionMap()[myMapLoader.currentXLoc, myMapLoader.currentYLoc])
                {
                    allEnemies[8].transform.position = spawnLocations[8].position;
                    allEnemies[8].SetActive(true);
                    allEnemies[8].GetComponent<ShootableEntity>().ResetHealth();
                }
                break;
        }
    }

    public void FinishRoom()
    {
        //roomComplete = true;
        myMapLoader.GetCompletionMap()[myMapLoader.currentXLoc, myMapLoader.currentYLoc] = true;
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
