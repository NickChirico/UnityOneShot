using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public MapLoader myLoader;
    public DoorManager northDoor, eastDoor, southDoor, westDoor;
    public Transform northSpawn, eastSpawn, southSpawn, westSpawn;
    public string roomTag;
    public SpawnArrangement[] spawnOptions;
    public Transform[] allPickupSpawnLocs;
    
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
        /*
        if (myLoader.ComplexMap[tempX, tempY].Contains("*R"))
        {
            print("rupture room");
            Instantiate(rupturePrefab, pickupSpawnLoc.position, Quaternion.identity);
            myLoader.ComplexMap[tempX, tempY] = "R";
        }
        if (myLoader.ComplexMap[tempX, tempY].Contains("*C"))
        {
            print("contaminate room");
            Instantiate(contaminatePrefab, pickupSpawnLoc.position, Quaternion.identity);
            myLoader.ComplexMap[tempX, tempY] = "C";
        }
        if (myLoader.ComplexMap[tempX, tempY].Contains("*S"))
        {
            print("siphon room");
            Instantiate(siphonPrefab, pickupSpawnLoc.position, Quaternion.identity);
            myLoader.ComplexMap[tempX, tempY] = "S";
        }
        */
        northDoor.LoadNewDoor(tempX - 1, tempY);
        eastDoor.LoadNewDoor(tempX, tempY + 1);
        southDoor.LoadNewDoor(tempX + 1, tempY);
        westDoor.LoadNewDoor(tempX, tempY - 1); 
    }
}
