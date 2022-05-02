using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Shambler_pf, Baneling_pf, Knifer_pf, Duelist_pf, Rifleman_pf, Breacher_pf, Gunner_pf,  boss1_pf;
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
                allEnemies.Add(Instantiate(Shambler_pf, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
            }
            else if (toSpawn.spawnOptions[spawnSelection].enemyTypeArray[i] == 'R')
            {
                allEnemies.Add(Instantiate(Rifleman_pf, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
            }
            else if (toSpawn.spawnOptions[spawnSelection].enemyTypeArray[i] == 'B')
            {
                allEnemies.Add(Instantiate(boss1_pf, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
            }
            else if (toSpawn.spawnOptions[spawnSelection].enemyTypeArray[i] == 'X')
            {
                allEnemies.Add(Instantiate(Baneling_pf, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
            }
            else if (toSpawn.spawnOptions[spawnSelection].enemyTypeArray[i] == 'D')
            {
                allEnemies.Add(Instantiate(Duelist_pf, toSpawn.spawnOptions[spawnSelection].spawnLocationsArray[i]));
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
