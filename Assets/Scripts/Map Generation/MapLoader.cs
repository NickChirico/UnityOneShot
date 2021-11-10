using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public char[,] operatingMap;
    public MapGenerator myMap;
    public bool[,] CompletedRooms = new bool[15,15];
    public int currentXLoc, currentYLoc;
    public DoorManager northDoor, eastDoor, southDoor, westDoor;
    public Transform northSpawn, eastSpawn, southSpawn, westSpawn;
    public Player myPlayer;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (i == 7 && j == 7)
                {
                    CompletedRooms[i, j] = true;
                }
                else
                {
                    CompletedRooms[i, j] = false;
                }
            }
        }
        currentXLoc = 7;
        currentYLoc = 7;
        myMap.GenerateMap();
        myMap.roomArray[currentXLoc, currentYLoc] = 'H';
        myMap.ShowMap();
        northDoor.LoadNewDoor(currentXLoc - 1, currentYLoc);
        eastDoor.LoadNewDoor(currentXLoc, currentYLoc + 1);
        southDoor.LoadNewDoor(currentXLoc + 1, currentYLoc);
        westDoor.LoadNewDoor(currentXLoc, currentYLoc - 1);
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
            //myMap.ShowMapOnScreen();
        }
        else
        {
            print("Invalid Room!");
        }
    }
}
