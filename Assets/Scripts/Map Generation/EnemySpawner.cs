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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!myMapLoader.CompletedRooms[myMapLoader.currentXLoc, myMapLoader.currentYLoc])
        {
            
        }
    }

    public void SpawnEnemies(int roomPosX, int roomPosY)
    {
        roomComplete = myMapLoader.CompletedRooms[roomPosX, roomPosY];
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
        myMapLoader.CompletedRooms[myMapLoader.currentXLoc, myMapLoader.currentYLoc] = true;
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
