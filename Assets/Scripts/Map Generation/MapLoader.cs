using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GameObject rupturePickup, contaminatePickup, siphonPickup;
    public Room loadedRoom;
    public Room[] allRooms, bossRooms;
    public EnemySpawner mySpawner;
    public char[,] operatingMap;
    public string[,] ComplexMap, pickupMap;
    public char[,] woodsMap, churchMap, marketMap, academyMap;
    public char[,] startMap =
    {
        {'H'}
    };

    public HealingSpring mySpring;
    public MapGenerator myMap;
    //public bool[,] CompletedRooms = new bool[1,1];
    //public bool[,] CompletedWoods = new bool[15,15];
    //public bool[,] CompletedChurch = new bool[15,15];
    //public bool[,] CompletedMarket = new bool[15,15];
    //public bool[,] CompletedAcademy = new bool[15,15];
    public int currentXLoc, currentYLoc, bossXLoc, bossYLoc;
    public enum Area
    {
        Start,
        Woods,
        Church,
        Market,
        Academy,
        Test
    }
    public Area currentArea;
    public int woodsStartX,
        woodsStartY,
        churchStartX,
        churchStartY,
        marketStartX,
        marketStartY,
        academyStartX,
        academyStartY,
        testStartX,
        testStartY;
    public DoorManager northDoor, eastDoor, southDoor, westDoor;
    public Player myPlayer;
    public Unlockable[] allUnlockables;
    public GameObject entranceRoom;

    public RoomCollections grasslands, desert, mountain, dirt, currentCollection;
    //woodsRoom, churchRoom, marketRoom, academyRoom;

    void Awake()
    {
        myMap = GameObject.Find("Map Generator").GetComponent<MapGenerator>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //currentXLoc = 7;
        //currentYLoc = 7;
        currentArea = Area.Test;
        ComplexMap = myMap.roomArray;
        AssignStartPositions(ComplexMap, Area.Test);
        for (int i = 0; i < myMap.roomSizes[myMap.GetCurrentTier()]; i++)
        {
            for (int j = 0; j < myMap.roomSizes[myMap.GetCurrentTier()]; j++)
            {
                if (ComplexMap[i, j].Contains("*B"))
                {
                    bossXLoc = i;
                    bossYLoc = j;
                }
                //CompletedRooms[i, j] = false;
                //CompletedWoods[i, j] = false;
                //CompletedChurch[i, j] = false;
                //CompletedMarket[i, j] = false;
                //CompletedAcademy[i, j] = false;
            }
        }
        currentXLoc = testStartX;
        currentYLoc = testStartY;
        loadedRoom.LoadAllDoors(currentXLoc, currentYLoc);
        /*
        loadedRoom.northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
        loadedRoom.eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
        loadedRoom.southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
        loadedRoom.westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
        loadedRoom.northDoor.Unlock();
        loadedRoom.eastDoor.Unlock();
        loadedRoom.southDoor.Unlock();
        loadedRoom.westDoor.Unlock();
        */
        //woodsMap = myMap.GenerateMap("North");
        //churchMap = myMap.GenerateMap("West"); //west
        //academyMap = myMap.GenerateMap("East"); //east
        //marketMap = myMap.GenerateMap("South"); //south
        //ShowMap(woodsMap);
        //ShowMap(churchMap);
        //ShowMap(marketMap);
        //ShowMap(academyMap);
        //AssignStartPositions(woodsMap, Area.Woods);
        //AssignStartPositions(churchMap, Area.Church);
        //AssignStartPositions(marketMap, Area.Market);
        //AssignStartPositions(academyMap, Area.Academy);
        //northDoor.LoadPortal(Area.Woods);
        //eastDoor.LoadPortal(Area.Church);
        //southDoor.LoadPortal(Area.Market);
        //westDoor.LoadPortal(Area.Academy);
        //myMap.roomArray[currentXLoc, currentYLoc] = 'H';
        //mySpawner.SpawnEnemies(currentXLoc, currentYLoc);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space) && GetAreaMap()[currentXLoc, currentYLoc] != 'B' && mySpawner.roomComplete)
        {
            currentXLoc = bossXLoc;
            currentYLoc = bossYLoc;
            northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
            eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
            southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
            westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
            myPlayer.gameObject.transform.position = southSpawn.position;
            mySpawner.SpawnEnemies();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            northDoor.Unlock();
            eastDoor.Unlock();
            southDoor.Unlock();
            westDoor.Unlock();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LoadRoom("north");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            LoadRoom("south");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LoadRoom("west");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LoadRoom("east");
        }
        */
    }

    public void LoadRoom(string targetDirection)
    {
        print("loading room");
        Transform targetSpawn = myPlayer.transform;
        int targetX = currentXLoc;
        int targetY = currentYLoc;
        
        switch (targetDirection)
        {
            case "north":
                currentXLoc -= 1;
                //change which room is loaded based on the path code, if it's a boss room, do the boss version
                //turn the old room off, turn the new one on
                loadedRoom.LoadAllDoors(currentXLoc, currentYLoc);
                targetSpawn = loadedRoom.southSpawn;
                break;
            case "east":
                currentYLoc += 1;
                loadedRoom.LoadAllDoors(currentXLoc, currentYLoc);
                targetSpawn = loadedRoom.westSpawn;
                break;
            case "south":
                currentXLoc += 1;
                loadedRoom.LoadAllDoors(currentXLoc, currentYLoc);
                targetSpawn = loadedRoom.northSpawn;
                break;
            case "west":
                currentYLoc -= 1;
                loadedRoom.LoadAllDoors(currentXLoc, currentYLoc);
                targetSpawn = loadedRoom.eastSpawn;
                break;
        }
        if (ComplexMap[targetX, targetY].Contains("D") || ComplexMap[targetX, targetY].Contains("H") ||
            ComplexMap[targetX, targetY].Contains("B") || ComplexMap[targetX, targetY].Contains("R") || ComplexMap[targetX, targetY].Contains("C") || 
            ComplexMap[targetX, targetY].Contains("S"))
        {
            //operatingMap[targetX, targetY] = 'H';
            //operatingMap[currentXLoc, currentYLoc] = 'D';
            myPlayer.gameObject.transform.position = targetSpawn.position;
            if (ComplexMap[currentXLoc, currentYLoc].StartsWith("*"))
            {
                if (ComplexMap[currentXLoc, currentYLoc].Contains("*B"))
                {
                    mySpawner.SpawnEnemies(loadedRoom, true);
                }
                else
                {
                    mySpawner.SpawnEnemies(loadedRoom, false);
                }
            }
            //myMap.ShowMapOnScreen();
            //print(CompletedRooms[currentXLoc, currentYLoc]);
            mySpring.LoadSpring();
        }
        else
        {
            print("Invalid Room!");
        }
    }


    public void AssignStartPositions(string[,] tempMap, Area whichArea)
    {
        int tempX = 0;
        int tempY = 0;
        for (int i = 0; i < myMap.roomSizes[myMap.GetCurrentTier()]; i++)
        {
            for (int j = 0; j < myMap.roomSizes[myMap.GetCurrentTier()]; j++)
            {
                if (tempMap[i, j].Contains("*H") || tempMap[i, j].Contains("*R") || tempMap[i, j].Contains("*C") || tempMap[i, j].Contains("*S"))
                {
                    tempX = i;
                    tempY = j;
                }
            }
        }
        testStartX = tempX;
        testStartY = tempY;
    }
}
