using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject meleePrefab, rangedPrefab, bossPrefab, duelistPrefab, banelingPrefab;
    public bool roomComplete;
    public int numToSpawn;
    public MapLoader myMapLoader;
    public List<GameObject> allEnemies;
    public Transform[] spawnLocation;
    public bool bossSpawned;

    void Start()
    {
        bossSpawned = false;
    }

    void Update()
    {

    }

    public void SpawnEnemies(Room toSpawn, int spawnSelection)
    {
        print(spawnSelection);
        for (int i = 0; i < toSpawn.spawnOptions[spawnSelection].spawnLocationsArray.Length; i++)
        {
            if (toSpawn.spawnOptions[spawnSelection].enemyTypeArray[i] == 'M')
            {
                allEnemies.Add(Instantiate(meleePrefab, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
            }
            else if (toSpawn.spawnOptions[spawnSelection].enemyTypeArray[i] == 'R')
            {
                allEnemies.Add(Instantiate(rangedPrefab, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
            }
            else if (toSpawn.spawnOptions[spawnSelection].enemyTypeArray[i] == 'B')
            {
                allEnemies.Add(Instantiate(bossPrefab, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
            }
            else if (toSpawn.spawnOptions[spawnSelection].enemyTypeArray[i] == 'X')
            {
                allEnemies.Add(Instantiate(banelingPrefab, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
            }
            else if (toSpawn.spawnOptions[spawnSelection].enemyTypeArray[i] == 'D')
            {
                allEnemies.Add(Instantiate(duelistPrefab, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
            }
        }

    }

    public void FinishRoom()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        print("finished");
        roomComplete = true;
        myMapLoader.ComplexMap[myMapLoader.currentRank, myMapLoader.currentFile] = myMapLoader.ComplexMap[myMapLoader.currentRank, myMapLoader.currentFile].TrimStart('*');
        //myMapLoader.GetCompletionMap()[myMapLoader.currentRank, myMapLoader.currentFile] = true;
        for (int i = 0; i < myMapLoader.allUnlockables.Length; i++)
        {
            myMapLoader.allUnlockables[i].Unlock();
        }
        print("Unlocking Now");
        myMapLoader.loadedRoom.northDoor.Unlock();
        myMapLoader.loadedRoom.eastDoor.Unlock();
        myMapLoader.loadedRoom.southDoor.Unlock();
        myMapLoader.loadedRoom.westDoor.Unlock();
    }

    public void CheckEnemiesAlive()
    {
        print("Checking enemies alive");
        if (allEnemies.Count <= 0)
        {
            FinishRoom();
        }
    }
}
