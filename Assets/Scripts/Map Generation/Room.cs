using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
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
        northDoor.LoadNewDoor(tempX - 1, tempY);
        eastDoor.LoadNewDoor(tempX, tempY + 1);
        southDoor.LoadNewDoor(tempX + 1, tempY);
        westDoor.LoadNewDoor(tempX, tempY - 1); 
    }
}
