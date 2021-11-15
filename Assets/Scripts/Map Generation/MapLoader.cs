using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public EnemySpawner mySpawner;
    public char[,] operatingMap;
    public char[,] woodsMap, churchMap, marketMap, academyMap;
    public MapGenerator myMap;
    public bool[,] CompletedRooms = new bool[15,15];
    public bool[,] CompletedWoods = new bool[15,15];
    public bool[,] CompletedChurch = new bool[15,15];
    public bool[,] CompletedMarket = new bool[15,15];
    public bool[,] CompletedAcademy = new bool[15,15];
    public int currentXLoc, currentYLoc;
    public enum Area
    {
        Start,
        Woods,
        Church,
        Market,
        Academy
    }
    public Area currentArea;
    public int woodsStartX,
        woodsStartY,
        churchStartX,
        churchStartY,
        marketStartX,
        marketStartY,
        academyStartX,
        academyStartY;
    public DoorManager northDoor, eastDoor, southDoor, westDoor;
    public Transform northSpawn, eastSpawn, southSpawn, westSpawn;
    public Player myPlayer;
    public Unlockable[] allUnlockables;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                CompletedRooms[i, j] = false;
                CompletedWoods[i, j] = false;
                CompletedChurch[i, j] = false;
                CompletedMarket[i, j] = false;
                CompletedAcademy[i, j] = false;
            }
        }
        currentXLoc = 7;
        currentYLoc = 7;
        currentArea = Area.Start;
        woodsMap = myMap.GenerateMap("North");
        churchMap = myMap.GenerateMap("West");
        academyMap = myMap.GenerateMap("East");
        marketMap = myMap.GenerateMap("South");
        myMap.roomArray[currentXLoc, currentYLoc] = 'H';
        
        mySpawner.SpawnEnemies(currentXLoc, currentYLoc);
    }

    // Update is called once per frame
    void Update()
    {
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
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LoadRoom("east");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            LoadRoom("south");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LoadRoom("west");
        }
    }

    public void LoadRoom(string targetDirection)
    {
        
        Transform targetSpawn = myPlayer.transform;
        int targetX = currentXLoc;
        int targetY = currentYLoc;
        switch (targetDirection)
        {
            case "north":
                targetX = currentXLoc - 1;
                targetY = currentYLoc;
                targetSpawn = southSpawn;
                break;
            case "east":
                targetX = currentXLoc;
                targetY = currentYLoc + 1;
                targetSpawn = westSpawn;
                break;
            case "south":
                targetX = currentXLoc + 1;
                targetY = currentYLoc;
                targetSpawn = northSpawn;
                break;
            case "west":
                targetX = currentXLoc;
                targetY = currentYLoc - 1;
                targetSpawn = eastSpawn;
                break;
        }
        if (operatingMap[targetX, targetY] == 'D')
        {
            operatingMap[targetX, targetY] = 'H';
            operatingMap[currentXLoc, currentYLoc] = 'D';
            currentXLoc = targetX;
            currentYLoc = targetY;
            northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
            eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
            southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
            westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
            myPlayer.gameObject.transform.position = targetSpawn.position;
            mySpawner.SpawnEnemies(currentXLoc, currentYLoc);
            //myMap.ShowMapOnScreen();
            print(CompletedRooms[currentXLoc, currentYLoc]);
        }
        else
        {
            print("Invalid Room!");
        }
    }

    public void LoadArea()
    {
        switch (currentArea)
        {
            case Area.Start:
                northDoor.LoadPortal(Area.Woods);
                eastDoor.LoadPortal(Area.Church);
                southDoor.LoadPortal(Area.Market);
                westDoor.LoadPortal(Area.Academy);
                break;
            case Area.Woods:
                currentXLoc = woodsStartX;
                currentYLoc = woodsStartY;
                northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
                eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
                southDoor.LoadPortal(Area.Start);
                westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
                break;
            case Area.Church:
                currentXLoc = churchStartX;
                currentYLoc = churchStartY;
                northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
                eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
                southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
                westDoor.LoadPortal(Area.Start);
                break;
            case Area.Market:
                currentXLoc = marketStartX;
                currentYLoc = marketStartY;
                northDoor.LoadPortal(Area.Start);
                eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
                southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
                westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
                break;
            case Area.Academy:
                currentXLoc = academyStartX;
                currentYLoc = academyStartY;
                northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
                eastDoor.LoadPortal(Area.Start);
                southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
                westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
                break;
        }
    }
}
