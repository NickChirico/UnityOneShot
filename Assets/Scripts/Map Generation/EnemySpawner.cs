using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject meleePrefab, rangedPrefab, bossPrefab;
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

    public void SpawnEnemies(Room toSpawn, bool spawningBoss)
    {
        int spawnSelection;
        if (spawningBoss)
        {
            for (int i = 0; i < toSpawn.spawnOptions[0].spawnLocationsArray.Length; i++)
            {
                if (toSpawn.spawnOptions[0].enemyTypeArray[i] == 'M')
                {
                    allEnemies.Add(Instantiate(meleePrefab, toSpawn.spawnOptions[0].spawnLocationsArray[i]));
                }
                else if (toSpawn.spawnOptions[0].enemyTypeArray[i] == 'R')
                {
                    allEnemies.Add(Instantiate(rangedPrefab, toSpawn.spawnOptions[0].spawnLocationsArray[i]));
                }
                else if (toSpawn.spawnOptions[0].enemyTypeArray[i] == 'B')
                {
                    allEnemies.Add(Instantiate(bossPrefab, toSpawn.spawnOptions[0].spawnLocationsArray[i]));
                }
            }
        }
        else
        {
            spawnSelection = Random.Range(1, toSpawn.spawnOptions.Length);
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
            }
        }
        
    }

    public void FinishRoom()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        print("finished");
        roomComplete = true;
        myMapLoader.ComplexMap[myMapLoader.currentXLoc, myMapLoader.currentYLoc] = myMapLoader.ComplexMap[myMapLoader.currentXLoc, myMapLoader.currentYLoc].TrimStart('*');
        //myMapLoader.GetCompletionMap()[myMapLoader.currentXLoc, myMapLoader.currentYLoc] = true;
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
        print("Checking enemies alive");
        if (allEnemies.Count <= 0)
        {
            FinishRoom();
        }
    }
}
