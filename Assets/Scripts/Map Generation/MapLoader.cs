using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    public CameraController myCam;
    public GameObject rupturePickup, contaminatePickup, siphonPickup;
    public PlayerLoader playerLoader;
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
    public int currentRank, currentFile, bossXLoc, bossYLoc;
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
    public int testStartX, testStartY;
    public DoorManager northDoor, eastDoor, southDoor, westDoor;
    public Player myPlayer;
    public Unlockable[] allUnlockables;
    public GameObject entranceRoom;
    public RoomCollections grasslands, desert, volcano;
    //woodsRoom, churchRoom, marketRoom, academyRoom;

    void Awake()
    {
        myMap = GameObject.Find("Map Generator").GetComponent<MapGenerator>();
        playerLoader = GameObject.Find("Player Loader").GetComponent<PlayerLoader>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //currentRank = 7;
        //currentFile = 7;
        //myMap = GameObject.Find("Map Generator").GetComponent<MapGenerator>();
        //currentArea = Area.Test;
        ComplexMap = myMap.roomArray;
        AssignStartPositions(ComplexMap);
        for (int i = 0; i < myMap.roomSizes[myMap.GetCurrentTier()]; i++)
        {
            for (int j = 0; j < myMap.roomSizes[myMap.GetCurrentTier()]; j++)
            {
                if (ComplexMap[i, j].Contains("B"))
                {
                    bossXLoc = i;
                    bossYLoc = j;
                }
            }
        }
        currentRank = testStartX;
        currentFile = testStartY;
        LoadRoom(ComplexMap[currentRank, currentFile]);
        loadedRoom.LoadAllDoors(currentRank, currentFile);
        myPlayer = playerLoader.LoadIntoRoom(loadedRoom.southSpawn.position);
        myCam.SetTarget(myPlayer.transform);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space) && GetAreaMap()[currentRank, currentFile] != 'B' && mySpawner.roomComplete)
        {
            currentRank = bossXLoc;
            currentFile = bossYLoc;
            northDoor.LoadNewDoor(currentRank - 1, currentFile);
            eastDoor.LoadNewDoor(currentRank, currentFile + 1);
            southDoor.LoadNewDoor(currentRank + 1, currentFile);
            westDoor.LoadNewDoor(currentRank, currentFile - 1);
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

    public GameObject Instructions;
    public void Travel(string targetDirection)
    {
        Instructions.SetActive(false);

        print("loading room");
        loadedRoom.ClearChests();

        Transform targetSpawn = myPlayer.transform;
        int targetRank = currentRank;
        int targetFile = currentFile;
        switch (targetDirection)
        {
            case "north":
                if (ComplexMap[targetRank - 1, targetFile] != "X")
                {
                    //delete all pickups in the room HERE
                    ComplexMap[currentRank, currentFile].Replace('!', '?');
                    currentRank -= 1;
                    LoadRoom(ComplexMap[currentRank, currentFile]);
                    //loadedRoom.LoadAllDoors(currentRank, currentFile);
                    targetSpawn = loadedRoom.southSpawn;
                }
                break;
            case "east":
                if (ComplexMap[targetRank, targetFile + 1] != "X")
                {
                    //delete all pickups in the room HERE
                    ComplexMap[currentRank, currentFile].Replace('!', '?');
                    currentFile += 1;
                    LoadRoom(ComplexMap[currentRank, currentFile]);
                    //loadedRoom.LoadAllDoors(currentRank, currentFile);
                    targetSpawn = loadedRoom.westSpawn;
                }
                break;
            case "south":
                if (ComplexMap[targetRank + 1, targetFile] != "X")
                {
                    //delete all pickups in the room HERE
                    ComplexMap[currentRank, currentFile].Replace('!', '?');
                    currentRank += 1;
                    LoadRoom(ComplexMap[currentRank, currentFile]);
                    //loadedRoom.LoadAllDoors(currentRank, currentFile);
                    targetSpawn = loadedRoom.northSpawn;
                }
                break;
            case "west":
                if (ComplexMap[targetRank, targetFile - 1] != "X")
                {
                    //delete all pickups in the room HERE
                    ComplexMap[currentRank, currentFile].Replace('!', '?');
                    currentFile -= 1;
                    LoadRoom(ComplexMap[currentRank, currentFile]);
                    //loadedRoom.LoadAllDoors(currentRank, currentFile);
                    targetSpawn = loadedRoom.eastSpawn;
                }
                break;
        }

        //myPlayer.gameObject.transform.position = loadedRoom.allPickupSpawnLocs[0].position;
        myPlayer.gameObject.transform.position = targetSpawn.position;

        Camera.main.GetComponent<CameraController>().JumpToPlayer();

        ShowMap();
    }

    public void CompleteArea()
    {
        GameObject.Find("SeraphController").GetComponent<SeraphController>().UploadSeraphs();
        GameObject.Find("Player Loader").GetComponent<PlayerLoader>().currentPathLevel += 1;
        GameObject.Find("Path Manager").GetComponent<PathManager>().EnterMapScene();
    }


    
    public void LoadRoom(string loadingRoomCode)
    {
        grasslands.SetAllInactive();
        desert.SetAllInactive();
        volcano.SetAllInactive();
        string[] roomAttributes = loadingRoomCode.Split('.'); //1st the status of the room, then the type, then the biome, then the layout
        switch (roomAttributes[2])
        {
            case "G":
                loadedRoom = grasslands.ActivateRoom(int.Parse(roomAttributes[3]));
                break;
            case "D":
                loadedRoom = desert.ActivateRoom(int.Parse(roomAttributes[3]));
                break;
            case "V":
                loadedRoom = volcano.ActivateRoom(int.Parse(roomAttributes[3]));
                break;
            default:
                print("invalid biome type");
                break;
        }
        loadedRoom.LoadAllDoors(currentRank, currentFile);
        if (roomAttributes[0] == "*") //* = unvisited, ? = visited, ! = currently here
        {
            int spawnChoice = int.Parse(ComplexMap[currentRank, currentFile].Split('.')[4]);
            mySpawner.SpawnEnemies(loadedRoom, spawnChoice);
            ComplexMap[currentRank, currentFile].Replace('*', '!');
        }
        if (roomAttributes[0] == "?")
        {
            ComplexMap[currentRank, currentFile].Replace('?', '!');
        }

        // CHEST CHEST CHEST
        if (roomAttributes.Length > 5) // CHEST SPAWNS
        {
            switch (roomAttributes[5])
            {
                case "A":
                    mySpawner.SpawnChests(loadedRoom, 0);
                    break;
                case "B":
                    mySpawner.SpawnChests(loadedRoom, 1);
                    break;
                case "C":
                    mySpawner.SpawnChests(loadedRoom, 2);
                    break;
                case "D":
                    mySpawner.SpawnChests(loadedRoom, 3);
                    break;

                default:
                    break;
            }
        }

        // DESTROY WEAPON DROPS UPON ENTERING NEW ROOM
        WeaponDrop[] drops = FindObjectsOfType<WeaponDrop>();
        foreach (WeaponDrop weapDrop in drops)
            Destroy(weapDrop.gameObject);

        SeraphDROP[] serDrops = FindObjectsOfType<SeraphDROP>();
        foreach (SeraphDROP serD in serDrops)
            Destroy(serD.gameObject);

    }


    public void AssignStartPositions(string[,] tempMap)
    {
        int tempX = 0;
        int tempY = 0;
        for (int i = 0; i < myMap.roomSizes[myMap.GetCurrentTier()]; i++)
        {
            for (int j = 0; j < myMap.roomSizes[myMap.GetCurrentTier()]; j++)
            {
                if (tempMap[i, j].Contains("E"))
                {
                    tempX = i;
                    tempY = j;
                }
            }
        }
        testStartX = tempX;
        testStartY = tempY;
    }

    public void ShowMap()
    {
        string willPrint = "";
        for (int i = 0; i < myMap.roomSizes[myMap.GetCurrentTier()]; i++)
        {
            for (int j = 0; j < myMap.roomSizes[myMap.GetCurrentTier()]; j++)
            {
                willPrint += ComplexMap[i, j] + " ";
            }

            willPrint += "\n";
        }
        print(willPrint);
    }
}
