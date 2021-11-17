using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public int[,] maxArray = new[,]
    {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
        {1,2,2,3,3,3,3,3,3,3,3,3,2,2,1},
        {1,2,2,3,3,3,3,3,3,3,3,3,2,2,1},
        {1,2,2,3,3,4,4,4,4,4,3,3,2,2,1},
        {1,2,2,3,3,4,4,4,4,4,3,3,2,2,1},
        {1,2,2,3,3,4,4,4,4,4,3,3,2,2,1},
        {1,2,2,3,3,4,4,4,4,4,3,3,2,2,1},
        {1,2,2,3,3,4,4,4,4,4,3,3,2,2,1},
        {1,2,2,3,3,3,3,3,3,3,3,3,2,2,1},
        {1,2,2,3,3,3,3,3,3,3,3,3,2,2,1},
        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

    public int[,] minArray = new[,]
    {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,2,2,2,2,2,1,1,1,1,1},
        {1,1,1,1,1,2,2,2,2,2,1,1,1,1,1},
        {1,1,1,1,1,2,2,3,2,2,1,1,1,1,1},
        {1,1,1,1,1,2,2,2,2,2,1,1,1,1,1},
        {1,1,1,1,1,2,2,2,2,2,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

    public char[,] roomArray = new char[15,15];
    public int numDoneRooms, numDeadEnds;
    public List<int> deadEndXPos, deadEndYPos;
    public MapLoader myLoader;
    public Text screenText;
    // Start is called before the first frame update
    void Start()
    {
        ResetMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public char[,] GenerateMap(string direction)
    {
        //direction determines if we start on the top, bottom, left, or right of the map, and where the final boss is
        bool stillGenerating = true; //allows us to remake a map if it isn't satisfactory
        //ShowMap();
        while (stillGenerating)
        {
            ResetMap();
            IterateMap();
            if (numDoneRooms > 16 && numDeadEnds > 3)
            {
                stillGenerating = false;
            }
        }
        OrientMap(direction);
        return roomArray;
    }

    public void OrientMap(string direction)
    {
        int idealValue;
        int finalValue;
        int startLoc = 0;
        int endLoc = 0;
        switch (direction)
        {
            case "North": //smallest x pos
                idealValue = 14;
                finalValue = 0;
                for (int i = 0; i < deadEndXPos.Count; i++)
                {
                    if (deadEndXPos[i] < idealValue)
                    {
                        startLoc = i;
                        idealValue = deadEndXPos[i];
                    }
                    if (deadEndXPos[i] > idealValue)
                    {
                        endLoc = i;
                        finalValue = deadEndXPos[i];
                    }
                }
                break;
            case "South": //largest x pos
                idealValue = 0;
                finalValue = 14;
                for (int i = 0; i < deadEndXPos.Count; i++)
                {
                    if (deadEndXPos[i] > idealValue)
                    {
                        startLoc = i;
                        idealValue = deadEndXPos[i];
                    }
                    if (deadEndXPos[i] < idealValue)
                    {
                        endLoc = i;
                        finalValue = deadEndXPos[i];
                    }
                }
                break;
            case "West": //smallest y pos
                idealValue = 14;
                finalValue = 0;
                for (int i = 0; i < deadEndYPos.Count; i++)
                {
                    if (deadEndYPos[i] < idealValue)
                    {
                        startLoc = i;
                        idealValue = deadEndYPos[i];
                    }
                    if (deadEndYPos[i] > idealValue)
                    {
                        endLoc = i;
                        finalValue = deadEndYPos[i];
                    }
                }
                break;
            case "East": //largest y pos
                idealValue = 0;
                finalValue = 14;
                for (int i = 0; i < deadEndYPos.Count; i++)
                {
                    if (deadEndYPos[i] > idealValue)
                    {
                        startLoc = i;
                        idealValue = deadEndYPos[i];
                    }
                    if (deadEndYPos[i] < finalValue)
                    {
                        endLoc = i;
                        finalValue = deadEndYPos[i];
                    }
                }
                break;
        }
        roomArray[deadEndXPos[startLoc], deadEndYPos[startLoc]] = 'H';
        roomArray[deadEndXPos[startLoc], deadEndYPos[startLoc]] = 'B';
        for (int i = 0; i < deadEndXPos.Count; i++)
        {
            if (roomArray[deadEndXPos[startLoc], deadEndYPos[startLoc]] == 'D')
            {
                roomArray[deadEndXPos[startLoc], deadEndYPos[startLoc]] = 'S';
            }
        }
    }

    public void IterateMap()
    {
        bool changed = false;
        int targetX = -1;
        int targetY = -1;
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                //print("checking at (" + i + "," + j + ")");
                if (!changed && roomArray[i, j] == 'W')
                {
                    //print("Found a W at (" + i + "," + j + ")");
                    changed = true;
                    targetX = i;
                    targetY = j;
                }
            }
        }
        if (changed && targetX != -1 && targetY != -1)
        {
            ExpandRoom(targetX, targetY);
        }
        FinalizeMap(changed);
    }

    public void ExpandRoom(int targetX, int targetY)
    {
        //ShowMapOnScreen();
        List<string> possibleConnections = new List<string>();
        //print("Here we go");
        int minConnections;
        int maxConnections;
        int numConnections = 0;
        int numWaiting = 0;
        int numDone = 0;
        int numInvalid = 0;
        int numEmpty = 0;
        if (targetX > 0)
        {
            switch (roomArray[targetX - 1, targetY])
            {    
                case 'W':
                    numWaiting++;
                    break;
                case 'X':
                    numInvalid++;
                    break;
                case 'O':
                    numEmpty++;
                    possibleConnections.Add("a");
                    break;
                case 'D':
                    numDone++;
                    break;
            }
        }
        if (targetX < 14)
        {
            switch (roomArray[targetX + 1, targetY])
            {
                case 'W':
                    numWaiting++;
                    break;
                case 'X':
                    numInvalid++;
                    break;
                case 'O':
                    numEmpty++;;
                    possibleConnections.Add("b");
                    break;
                case 'D':
                    numDone++;
                    break;
            }
        }
        if (targetY > 0)
        {
            switch (roomArray[targetX, targetY - 1])
            {
                case 'W':
                    numWaiting++;
                    break;
                case 'X':
                    numInvalid++;
                    break;
                case 'O':
                    numEmpty++;;
                    possibleConnections.Add("c");
                    break;
                case 'D':
                    numDone++;
                    break;
            }
        }
        if (targetY < 14)
        {
            switch (roomArray[targetX, targetY + 1])
            {
                case 'W':
                    numWaiting++;
                    break;
                case 'X':
                    numInvalid++;
                    break;
                case 'O':
                    numEmpty++;;
                    possibleConnections.Add("d");
                    break;
                case 'D':
                    numDone++;
                    break;
            }
        }
        
        
        //("There are " + numDone + " Done, " + numEmpty + " Empty, " + numInvalid + " Invalid, and " +
                  //numWaiting + " Waiting");
        

        if (minArray[targetX, targetY] < (numWaiting + numDone))
        {
            minConnections = (numWaiting + numDone);
        }
        else
        {
            minConnections = minArray[targetX, targetY];
        }
        //print("The minimum number of connections is " + minConnections);

        if (maxArray[targetX, targetY] > (4 - numInvalid))
        {
            maxConnections = (4 - numInvalid);
        }
        else
        {
            maxConnections = maxArray[targetX, targetY];
        }
        //print("The maximum number of connections is " + maxConnections);
        
        if (minConnections >= maxConnections)
        {
            numConnections = minConnections;
        }
        else if (maxConnections - minConnections == 1)
        {
            int selector = Random.Range(0, 2);
            switch (selector)
            {
                case 0:
                    numConnections = minConnections;
                    break;
                case 1:
                    numConnections = maxConnections;
                    break;
            }
        }
        else if (maxConnections - minConnections >= 2)
        {
            int selector = Random.Range(0, 3);
            switch (selector)
            {
                case 0:
                    numConnections = minConnections;
                    break;
                case 1:
                    numConnections = minConnections + 1;
                    break;
                case 2:
                    numConnections = maxConnections;
                    break;
            }
        }
        if (numConnections == 1)
        {
            deadEndXPos.Add(targetX);
            deadEndYPos.Add(targetY);
        }
        var numToConnect = numConnections - (numWaiting + numDone);
        if (numToConnect > numEmpty)
        {
            numToConnect = numEmpty;
        }
        
        if (numToConnect > 0)
        {
            for (int i = 0; i < numToConnect; i++)
            {
                int selector = Random.Range(0, possibleConnections.Count);
                switch (possibleConnections[selector])
                {
                    case "a":
                        roomArray[targetX - 1, targetY] = 'W';
                        possibleConnections.Remove("a");
                        break;
                    case "b":
                        roomArray[targetX + 1, targetY] = 'W';
                        possibleConnections.Remove("b");
                        break;
                    case "c":
                        roomArray[targetX, targetY - 1] = 'W';
                        possibleConnections.Remove("c");
                        break;
                    case "d":
                        roomArray[targetX, targetY + 1] = 'W';
                        possibleConnections.Remove("d");
                        break;
                    }
            }
        }

        if (targetX > 0 && roomArray[targetX - 1, targetY] == 'O')
        {
            roomArray[targetX - 1, targetY] = 'X';
        }
        if (targetX < 14 && roomArray[targetX + 1, targetY] == 'O')
        {
            roomArray[targetX + 1, targetY] = 'X';
        }
        if (targetY > 0 && roomArray[targetX, targetY - 1] == 'O')
        {
            roomArray[targetX, targetY - 1] = 'X';
        }
        if (targetY < 14 && roomArray[targetX, targetY + 1] == 'O')
        {
            roomArray[targetX, targetY + 1] = 'X';
        }
        roomArray[targetX, targetY] = 'D';
    }
    
    public void FinalizeMap(bool tempChanged)
    {
        if (tempChanged)
        {
            //numDoneRooms = 20;
            IterateMap();
        }
        else
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (roomArray[i, j] == 'O')
                    {
                        roomArray[i, j] = 'X';
                    }
                }
            }
            numDoneRooms = CountRooms();
            numDeadEnds = deadEndXPos.Count;
        }
    }

    public int CountRooms()
    {
        int counted = 0;
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (roomArray[i,j] == 'D')
                {
                    counted++;
                }
            }
        }
        return counted;
    }

    public void ResetMap() //prepares the room array for a new generation
    {
        roomArray = new [,]
        {
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','W','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
            {'O','O','O','O','O','O','O','O','O','O','O','O','O','O','O'},
        };
        numDoneRooms = 0;
        numDeadEnds = 0;
    }
    
}
