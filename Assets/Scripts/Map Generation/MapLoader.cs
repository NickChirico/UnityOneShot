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
    public GameObject entranceRoom, woodsRoom, churchRoom, marketRoom, academyRoom;

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
        churchMap = myMap.GenerateMap("North"); //west
        academyMap = myMap.GenerateMap("North"); //east
        marketMap = myMap.GenerateMap("North"); //south
        //ShowMap(woodsMap);
        //ShowMap(churchMap);
        //ShowMap(marketMap);
        //ShowMap(academyMap);
        AssignStartPositions(woodsMap, Area.Woods);
        AssignStartPositions(churchMap, Area.Church);
        AssignStartPositions(marketMap, Area.Market);
        AssignStartPositions(academyMap, Area.Academy);
        northDoor.LoadPortal(Area.Woods);
        eastDoor.LoadPortal(Area.Church);
        southDoor.LoadPortal(Area.Market);
        westDoor.LoadPortal(Area.Academy);
        //myMap.roomArray[currentXLoc, currentYLoc] = 'H';
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

    public void LoadArea(string direction)
    {
        northDoor.currentArea = currentArea;
        eastDoor.currentArea = currentArea;
        southDoor.currentArea = currentArea;
        westDoor.currentArea = currentArea;
        Transform targetSpawn = myPlayer.transform;
        switch (direction)
        {
            case "north":
                targetSpawn = southSpawn;
                break;
            case "east":
                targetSpawn = westSpawn;
                break;
            case "south":
                targetSpawn = northSpawn;
                break;
            case "west":
                targetSpawn = eastSpawn;
                break;
        }
        switch (currentArea)
        {
            case Area.Start:
                entranceRoom.SetActive(true);
                woodsRoom.SetActive(false);
                churchRoom.SetActive(false);
                marketRoom.SetActive(false);
                academyRoom.SetActive(false);
                northDoor.LoadPortal(Area.Woods);
                eastDoor.LoadPortal(Area.Church);
                southDoor.LoadPortal(Area.Market);
                westDoor.LoadPortal(Area.Academy);
                break;
            case Area.Woods:
                entranceRoom.SetActive(false);
                woodsRoom.SetActive(true);
                churchRoom.SetActive(false);
                marketRoom.SetActive(false);
                academyRoom.SetActive(false);
                currentXLoc = woodsStartX;
                currentYLoc = woodsStartY;
                northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
                eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
                southDoor.LoadPortal(Area.Start);
                westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
                break;
            case Area.Church:
                entranceRoom.SetActive(false);
                woodsRoom.SetActive(false);
                churchRoom.SetActive(true);
                marketRoom.SetActive(false);
                academyRoom.SetActive(false);
                currentXLoc = churchStartX;
                currentYLoc = churchStartY;
                northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
                eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
                southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
                westDoor.LoadPortal(Area.Start);
                break;
            case Area.Market:
                entranceRoom.SetActive(false);
                woodsRoom.SetActive(false);
                churchRoom.SetActive(false);
                marketRoom.SetActive(true);
                academyRoom.SetActive(false);
                currentXLoc = marketStartX;
                currentYLoc = marketStartY;
                northDoor.LoadPortal(Area.Start);
                eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
                southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
                westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
                break;
            case Area.Academy:
                entranceRoom.SetActive(false);
                woodsRoom.SetActive(false);
                churchRoom.SetActive(false);
                marketRoom.SetActive(false);
                academyRoom.SetActive(true);
                currentXLoc = academyStartX;
                currentYLoc = academyStartY;
                northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
                eastDoor.LoadPortal(Area.Start);
                southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
                westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
                break;
        }

        myPlayer.gameObject.transform.position = targetSpawn.position;
    }

    public void AssignStartPositions(char[,] tempMap, Area whichArea)
    {
        int tempX = 0;
        int tempY = 0;
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (tempMap[i, j] == 'H')
                {
                    tempX = i;
                    tempY = j;
                }
            }
        }
        switch (whichArea)
        {
            case Area.Woods:
                woodsStartX = tempX;
                woodsStartY = tempY;
                break;
            case Area.Church:
                churchStartX = tempX;
                churchStartY = tempY;
                break;
            case Area.Market:
                marketStartX = tempX;
                marketStartY = tempY;
                break;
            case Area.Academy:
                academyStartX = tempX;
                academyStartY = tempY;
                break;
        }
    }

    public void ShowMap(char[,] thisMap)
    {
        string willPrint = "";
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                willPrint += thisMap[i, j] + " ";
            }

            willPrint += "\n";
        }
        print(willPrint);
    }
}
