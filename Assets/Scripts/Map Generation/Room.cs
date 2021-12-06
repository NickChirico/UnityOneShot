using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public MapLoader myLoader;
    public GameObject rupturePrefab, contaminatePrefab, siphonPrefab;
    public Transform pickupSpawnLoc;
    
    public GameObject rangedPrefab, meleePrefab;

    public DoorManager northDoor, eastDoor, southDoor, westDoor;

    public SpawnArrangement[] spawnOptions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAllDoors(int tempX, int tempY)
    {
        if (myLoader.complexMap[tempX, tempY] == "*R")
        {
            Instantiate(rupturePrefab, pickupSpawnLoc.position, Quaternion.identity);
            myLoader.complexMap[tempX, tempY] = "R";
        }
        if (myLoader.complexMap[tempX, tempY] == "*C")
        {
            Instantiate(contaminatePrefab, pickupSpawnLoc.position, Quaternion.identity);
            myLoader.complexMap[tempX, tempY] = "C";
        }
        if (myLoader.complexMap[tempX, tempY] == "*S")
        {
            Instantiate(siphonPrefab, pickupSpawnLoc.position, Quaternion.identity);
            myLoader.complexMap[tempX, tempY] = "S";
        }
        northDoor.LoadNewDoor(tempX - 1, tempY);
        eastDoor.LoadNewDoor(tempX, tempY + 1);
        southDoor.LoadNewDoor(tempX + 1, tempY);
        westDoor.LoadNewDoor(tempX, tempY - 1); 
    }
}
