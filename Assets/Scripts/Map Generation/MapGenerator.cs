using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//MAKE IT MAKE SURE THAT THE START AND END ARE DEAD ENDS, KEEP TRACK OF THE LOWEST AND HIGHEST X AND Y VALUES
public class MapGenerator : MonoBehaviour
{
    
    public int[,,] tier1Limits = new int[,,]
    {
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}},
        {{1,1}, {1,2}, {1,2}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {2,4}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {1,2}, {1,2}, {1,1}},
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}}
    };
    public int[,,] tier2Limits = new int[,,]
    {
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}},
        {{1,1}, {1,2}, {1,2}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {3,4}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {1,2}, {1,2}, {1,1}},
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}}
    };
    public int[,,] tier3Limits = new int[,,]
    {
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}},
        {{1,1}, {1,3}, {1,3}, {1,3}, {1,1}},
        {{1,1}, {1,3}, {3,4}, {1,3}, {1,1}},
        {{1,1}, {1,3}, {1,3}, {1,3}, {1,1}},
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}}
    };
    public int[,,] tier4Limits = new int[,,]
    {
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}, {1,1}, {1,1}},
        {{1,1}, {1,2}, {1,2}, {1,2}, {1,2}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {1,3}, {1,3}, {1,3}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {1,3}, {3,4}, {1,3}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {1,3}, {1,3}, {1,3}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {1,2}, {1,2}, {1,2}, {1,2}, {1,1}},
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}, {1,1}, {1,1}}
    };
    public int[,,] tier5Limits = new int[,,]
    {
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}, {1,1}, {1,1}},
        {{1,1}, {1,2}, {1,2}, {1,2}, {1,2}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {2,3}, {2,3}, {2,3}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {2,3}, {3,4}, {2,3}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {2,3}, {2,3}, {2,3}, {1,2}, {1,1}},
        {{1,1}, {1,2}, {1,2}, {1,2}, {1,2}, {1,2}, {1,1}},
        {{1,1}, {1,1}, {1,1}, {1,1}, {1,1}, {1,1}, {1,1}}
    };

    public int[][,,] allTierLimits;

    public int[][] tierRoomNumbers = new[]
    {
        new int[] {3,11},
        new int[] {4,13},
        new int[] {5,15},
        new int[] {6,17},
        new int[] {7,19}
    };

    public int[] roomSizes = new[] {5, 5, 7, 7, 7};
    
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

    public string[,] roomArray = new string[1,1];

    public string[,] path = new string[,]
    {
        {"X", "X", "X", "X", "X"},
        {"X", "X", "X", "X", "X"},
        {"X", "X", "X", "X", "X"},
        {"X", "X", "X", "X", "X"},
        {"X", "X", "X", "X", "X"},
        {"X", "X", "X", "X", "X"}
    };
    //public string[,] demoMap = new string[1, 1];
    private int _currentTier;
    private string _currentPathCode;
    public int numDoneRooms, numDeadEnds, xMin, xMax, yMin, yMax, numDifferentRooms, numBossRooms;
    public List<int> deadEndXPos, deadEndYPos, allRoomsXPos, allRoomsYPos;
    public MapLoader myLoader;
    public Text screenText;

    public bool pathGenerated;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        pathGenerated = false;
        allTierLimits = new[] {tier1Limits, tier2Limits, tier3Limits, tier4Limits, tier5Limits};
        ResetMap();
        GeneratePath();
        ShowPath();
        //myLoader.complexMap = GenerateMapFromPath(1, "test");
    }

    void Update()
    {
        
    }

    public void SetTier(int input)
    {
        _currentTier = input;
    }

    public void SetPathCode(string input)
    {
        _currentPathCode = input;
    }

    public void GenerateMap()
    {
        //direction determines if we start on the top, bottom, left, or right of the map, and where the final boss is
        bool stillGenerating = true; //allows us to remake a map if it isn't satisfactory
        //ShowMap();
        while (stillGenerating)
        {
            ResetMap();
            IterateMap();
            if (numDoneRooms >= tierRoomNumbers[_currentTier-1][0] && numDoneRooms <= tierRoomNumbers[_currentTier-1][1] && numDeadEnds > 2 && CheckOrientation("North"))
            {
                stillGenerating = false;
            }
        }
        OrientMap();
        //return roomArray;
        ShowMap();
    }

    public void OrientMap()
    {
        string startRoomChar = "H";
        int idealValue;
        int finalValue;
        int startLoc = 0;
        int endLoc = 0;
        idealValue = 0;
        finalValue = roomSizes[_currentTier-1]-1;
        for (int i = 0; i < deadEndXPos.Count; i++)
        {
            if (deadEndXPos[i] > idealValue)
            {
                startLoc = i;
                idealValue = deadEndXPos[i];
            }
            if (deadEndXPos[i] < finalValue)
            {
                endLoc = i;
                finalValue = deadEndXPos[i];
            }
        }
        switch (_currentPathCode)
        {
            case "rupture":
                startRoomChar = "*R/0";
                break;
            case "contaminate":
                startRoomChar = "*C/0";
                break;
            case "siphon":
                startRoomChar = "*S/0";
                break;
        }
        roomArray[deadEndXPos[startLoc], deadEndYPos[startLoc]] = startRoomChar;
        roomArray[deadEndXPos[endLoc], deadEndYPos[endLoc]] = "*B/" + Random.Range(0, numBossRooms);
        for (int i = 0; i < deadEndXPos.Count; i++)
        {
            /*
            if (roomArray[deadEndXPos[i], deadEndYPos[i]] == "D")
            {
                roomArray[deadEndXPos[i], deadEndYPos[i]] = 'S';
            }*/
        }
    }

    public void IterateMap()
    {
        bool changed = false;
        int targetX = -1;
        int targetY = -1;
        for (int i = 0; i < roomSizes[_currentTier-1]; i++)
        {
            for (int j = 0; j < roomSizes[_currentTier-1]; j++)
            {
                //print("checking at (" + i + "," + j + ")");
                if (!changed && roomArray[i, j] == "W")
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
            ExpandRoom(targetX, targetY, _currentTier);
        }
        FinalizeMap(changed, _currentTier);
    }

    public void ExpandRoom(int targetX, int targetY, int tempNum)
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
                case "W":
                    numWaiting++;
                    break;
                case "X":
                    numInvalid++;
                    break;
                case "O":
                    numEmpty++;
                    possibleConnections.Add("a");
                    break;
                case "*D":
                    numDone++;
                    break;
            }
        }
        if (targetX < roomSizes[tempNum-1]-1)
        {
            switch (roomArray[targetX + 1, targetY])
            {
                case "W":
                    numWaiting++;
                    break;
                case "X":
                    numInvalid++;
                    break;
                case "O":
                    numEmpty++;;
                    possibleConnections.Add("b");
                    break;
                case "*D":
                    numDone++;
                    break;
            }
        }
        if (targetY > 0)
        {
            switch (roomArray[targetX, targetY - 1])
            {
                case "W":
                    numWaiting++;
                    break;
                case "X":
                    numInvalid++;
                    break;
                case "O":
                    numEmpty++;;
                    possibleConnections.Add("c");
                    break;
                case "*D":
                    numDone++;
                    break;
            }
        }
        if (targetY < roomSizes[tempNum-1]-1)
        {
            switch (roomArray[targetX, targetY + 1])
            {
                case "W":
                    numWaiting++;
                    break;
                case "X":
                    numInvalid++;
                    break;
                case "O":
                    numEmpty++;;
                    possibleConnections.Add("d");
                    break;
                case "*D":
                    numDone++;
                    break;
            }
        }
        
        
        //("There are " + numDone + " Done, " + numEmpty + " Empty, " + numInvalid + " Invalid, and " +
                  //numWaiting + " Waiting");
        

        if (allTierLimits[tempNum][targetX, targetY, 0] < (numWaiting + numDone))
        {
            minConnections = (numWaiting + numDone);
        }
        else
        {
            minConnections = allTierLimits[tempNum][targetX, targetY, 0];
        }
        //print("The minimum number of connections is " + minConnections);

        if (allTierLimits[tempNum][targetX, targetY, 1] > (4 - numInvalid))
        {
            maxConnections = (4 - numInvalid);
        }
        else
        {
            maxConnections = allTierLimits[tempNum][targetX, targetY, 1];
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
                        roomArray[targetX - 1, targetY] = "W";
                        possibleConnections.Remove("a");
                        break;
                    case "b":
                        roomArray[targetX + 1, targetY] = "W";
                        possibleConnections.Remove("b");
                        break;
                    case "c":
                        roomArray[targetX, targetY - 1] = "W";
                        possibleConnections.Remove("c");
                        break;
                    case "d":
                        roomArray[targetX, targetY + 1] = "W";
                        possibleConnections.Remove("d");
                        break;
                    }
            }
        }

        if (targetX > 0 && roomArray[targetX - 1, targetY] == "O")
        {
            roomArray[targetX - 1, targetY] = "X";
        }
        if (targetX < roomSizes[tempNum-1]-1 && roomArray[targetX + 1, targetY] == "O")
        {
            roomArray[targetX + 1, targetY] = "X";
        }
        if (targetY > 0 && roomArray[targetX, targetY - 1] == "O")
        {
            roomArray[targetX, targetY - 1] = "X";
        }
        if (targetY < roomSizes[tempNum-1]-1 && roomArray[targetX, targetY + 1] == "O")
        {
            roomArray[targetX, targetY + 1] = "X";
        }
        roomArray[targetX, targetY] = "*D/" + Random.Range(0, numDifferentRooms);
    }
    
    public void FinalizeMap(bool tempChanged, int tempNum)
    {
        if (tempChanged)
        {
            //numDoneRooms = 20;
            IterateMap();
        }
        else
        {
            for (int i = 0; i < roomSizes[tempNum-1]; i++)
            {
                for (int j = 0; j < roomSizes[tempNum-1]; j++)
                {
                    if (roomArray[i, j] == "O")
                    {
                        roomArray[i, j] = "X";
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
        for (int i = 0; i < roomSizes[_currentTier-1]; i++)
        {
            for (int j = 0; j < roomSizes[_currentTier-1]; j++)
            {
                if (roomArray[i,j].Contains("*D/"))
                {
                    counted++;
                    allRoomsXPos.Add(i);
                    allRoomsYPos.Add(j);
                    if (i < xMin)
                    {
                        xMin = i;
                    }

                    if (i > xMax)
                    {
                        xMax = i;
                    }

                    if (j < yMin)
                    {
                        yMin = j;
                    }

                    if (j > yMax)
                    {
                        yMax = j;
                    }
                }
            }
        }
        return counted;
    }

    public void ResetMap() //prepares the room array for a new generation
    {
        switch (_currentTier)
        {
            case 1:
                roomArray = new[,]
                {
                    {"O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O"},
                    {"O", "O", "W", "O", "O"},
                    {"O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O"}
                };
                xMin = 4;
                yMax = 4;
                break;
            case 2:
                roomArray = new[,]
                {
                    {"O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O"},
                    {"O", "O", "W", "O", "O"},
                    {"O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O"}
                };
                xMin = 4;
                yMin = 4;
                break;
            case 3:
                roomArray = new[,]
                {
                    {"O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O"},
                    {"O", "O", "W", "O", "O"},
                    {"O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O"}
                };
                xMin = 4;
                yMin = 4;
                break;
            case 4:
                roomArray = new[,]
                {
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "W", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"}
                };
                xMin = 6;
                yMin = 6;
                break;
            case 5:
                roomArray = new[,]
                {
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "W", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"},
                    {"O", "O", "O", "O", "O", "O", "O"}
                };
                xMin = 6;
                yMin = 6;
                break;
        }
        xMax = 0;
        yMin = 0;
        allRoomsXPos.Clear();
        allRoomsYPos.Clear();
        deadEndXPos.Clear();
        deadEndYPos.Clear();
        numDoneRooms = 0;
        numDeadEnds = 0;
    }

    public void ShowMap()
    {
        string willPrint = "";
        for (int i = 0; i < roomSizes[_currentTier-1]; i++)
        {
            for (int j = 0; j < roomSizes[_currentTier-1]; j++)
            {
                willPrint += roomArray[i, j] + " ";
            }

            willPrint += "\n";
        }
        print(willPrint);
    }

    public bool CheckOrientation(string direction)
    {
        bool toReturn = false;
        bool maxEnd = false;
        bool minEnd = false;
        if (direction == "North" || direction == "South") //check vertical orientation
        {
            for (int i = 0; i < deadEndXPos.Count; i++)
            {
                if (deadEndXPos[i] == xMax) //a dead end exists at the max
                {
                    maxEnd = true;
                }
                if (deadEndXPos[i] == xMin) //a dead end exists at the min
                {
                    minEnd = true;
                }
            }
        }
        else if (direction == "East" || direction == "West") //check horizontal orientation
        {
            for (int i = 0; i < deadEndYPos.Count; i++)
            {
                if (deadEndYPos[i] == yMax) //a dead end exists at the max
                {
                    maxEnd = true;
                }
                if (deadEndYPos[i] == yMin) //a dead end exists at the min
                {
                    minEnd = true;
                }
            }
        }
        return minEnd && maxEnd;
    }

    public void GeneratePath()
    {
        string[] pathCodeArray = new string[6];
        int initialPath = Random.Range(0, 10);
        switch (initialPath)
        {
            case 0:
                pathCodeArray[5] = "abc";
                break;
            case 1:
                pathCodeArray[5] = "abd";
                break;
            case 2:
                pathCodeArray[5] = "abe";
                break;
            case 3:
                pathCodeArray[5] = "acd";
                break;
            case 4:
                pathCodeArray[5] = "ace";
                break;
            case 5:
                pathCodeArray[5] = "ade";
                break;
            case 6:
                pathCodeArray[5] = "bcd";
                break;
            case 7:
                pathCodeArray[5] = "bce";
                break;
            case 8:
                pathCodeArray[5] = "bde";
                break;
            case 9:
                pathCodeArray[5] = "cde";
                break;
        }
        for (int i = 4; i > -1; i--)
        {
            pathCodeArray[i] = GetValidNextPath(pathCodeArray[i + 1]);
        }
        path = GeneratePathFromPathCodes(pathCodeArray);
    }

    public string GetValidNextPath(string lastPath)
    {
        int selection;
        switch (lastPath)
        {
            case "ab":
                return "abc";
            case "ac":
                selection = Random.Range(0, 5);
                switch (selection)
                {
                    case 0:
                        return "abc";
                    case 1:
                        return "abd";
                    case 2:
                        return "acd";
                    case 3:
                        return "bcd";
                    case 4:
                        return "abcd";
                    default:
                        return "abc";
                }
            case "ad":
                selection = Random.Range(0, 14);
                switch (selection)
                {
                    case 0:
                        return "abc";
                    case 1:
                        return "abd";
                    case 2:
                        return "abe";
                    case 3:
                        return "acd";
                    case 4:
                        return "ace";
                    case 5:
                        return "ade";
                    case 6:
                        return "bcd";
                    case 7:
                        return "bce";
                    case 8:
                        return "bde";
                    case 9:
                        return "abcd";
                    case 10:
                        return "abce";
                    case 11:
                        return "abde";
                    case 12:
                        return "acde";
                    case 13:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "ae":
                selection = Random.Range(0, 5);
                switch (selection)
                {
                    case 0:
                        return "abd";
                    case 1:
                        return "abe";
                    case 2:
                        return "ade";
                    case 3:
                        return "bde";
                    case 4:
                        return "abde";
                    default:
                        return "abde";
                }
            case "bc":
                selection = Random.Range(0, 5);
                switch (selection)
                {
                    case 0:
                        return "abc";
                    case 1:
                        return "abd";
                    case 2:
                        return "acd";
                    case 3:
                        return "bcd";
                    case 4:
                        return "abcd";
                    default:
                        return "acd";
                }
            case "bd":
                selection = Random.Range(0, 15);
                switch (selection)
                {
                    case 0:
                        return "abc";
                    case 1:
                        return "abd";
                    case 2:
                        return "abe";
                    case 3:
                        return "acd";
                    case 4:
                        return "ace";
                    case 5:
                        return "ade";
                    case 6:
                        return "bcd";
                    case 7:
                        return "bce";
                    case 8:
                        return "bde";
                    case 9:
                        return "cde";
                    case 10:
                        return "abcd";
                    case 11:
                        return "abce";
                    case 12:
                        return "abde";
                    case 13:
                        return "acde";
                    case 14:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "be":
                selection = Random.Range(0, 14);
                switch (selection)
                {
                    case 0:
                        return "cde";
                    case 1:
                        return "abd";
                    case 2:
                        return "abe";
                    case 3:
                        return "acd";
                    case 4:
                        return "ace";
                    case 5:
                        return "ade";
                    case 6:
                        return "bcd";
                    case 7:
                        return "bce";
                    case 8:
                        return "bde";
                    case 9:
                        return "abcd";
                    case 10:
                        return "abce";
                    case 11:
                        return "abde";
                    case 12:
                        return "acde";
                    case 13:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "cd":
                selection = Random.Range(0, 5);
                switch (selection)
                {
                    case 0:
                        return "bcd";
                    case 1:
                        return "bce";
                    case 2:
                        return "bde";
                    case 3:
                        return "cde";
                    case 4:
                        return "bcde";
                    default:
                        return "bcde";
                }
            case "ce":
                selection = Random.Range(0, 5);
                switch (selection)
                {
                    case 0:
                        return "bcd";
                    case 1:
                        return "bce";
                    case 2:
                        return "bde";
                    case 3:
                        return "cde";
                    case 4:
                        return "bcde";
                    default:
                        return "bcde";
                }
            case "de":
                return "cde";
            case "abc":
                selection = Random.Range(0, 9);
                switch (selection)
                {
                    case 0:
                        return "ab";
                    case 1:
                        return "ac";
                    case 2:
                        return "ad";
                    case 3:
                        return "bc";
                    case 4:
                        return "cd";
                    case 5:
                        return "abd";
                    case 6:
                        return "acd";
                    case 7:
                        return "bcd";
                    case 8:
                        return "abcd";
                    default:
                        return "abcd";
                }
            case "abd":
                selection = Random.Range(0, 19);
                switch (selection)
                {
                    case 0:
                        return "ac";
                    case 1:
                        return "ad";
                    case 2:
                        return "ae";
                    case 3:
                        return "bc";
                    case 4:
                        return "bd";
                    case 5:
                        return "be";
                    case 6:
                        return "abc";
                    case 7:
                        return "abe";
                    case 8:
                        return "acd";
                    case 9:
                        return "ace";
                    case 10:
                        return "ade";
                    case 11:
                        return "bcd";
                    case 12:
                        return "bce";
                    case 13:
                        return "bde";
                    case 14:
                        return "abcd";
                    case 15:
                        return "abce";
                    case 16:
                        return "abde";
                    case 17:
                        return "acde";
                    case 18:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "abe":
                selection = Random.Range(0, 16);
                switch (selection)
                {
                    case 0:
                        return "ad";
                    case 1:
                        return "ae";
                    case 2:
                        return "bd";
                    case 3:
                        return "be";
                    case 4:
                        return "abd";
                    case 5:
                        return "acd";
                    case 6:
                        return "ace";
                    case 7:
                        return "ade";
                    case 8:
                        return "bcd";
                    case 9:
                        return "bce";
                    case 10:
                        return "bde";
                    case 11:
                        return "abcd";
                    case 12:
                        return "abce";
                    case 13:
                        return "abde";
                    case 14:
                        return "acde";
                    case 15:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "acd":
                selection = Random.Range(0, 18);
                switch (selection)
                {
                    case 0:
                        return "ac";
                    case 1:
                        return "ad";
                    case 2:
                        return "bc";
                    case 3:
                        return "bd";
                    case 4:
                        return "be";
                    case 5:
                        return "abc";
                    case 6:
                        return "abd";
                    case 7:
                        return "abe";
                    case 8:
                        return "ace";
                    case 9:
                        return "ade";
                    case 10:
                        return "bcd";
                    case 11:
                        return "bce";
                    case 12:
                        return "bde";
                    case 13:
                        return "abcd";
                    case 14:
                        return "abce";
                    case 15:
                        return "abde";
                    case 16:
                        return "acde";
                    case 17:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "ace":
                selection = Random.Range(0, 16);
                switch (selection)
                {
                    case 0:
                        return "ad";
                    case 1:
                        return "ae";
                    case 2:
                        return "bd";
                    case 3:
                        return "be";
                    case 4:
                        return "abd";
                    case 5:
                        return "abe";
                    case 6:
                        return "acd";
                    case 7:
                        return "ade";
                    case 8:
                        return "bcd";
                    case 9:
                        return "bce";
                    case 10:
                        return "bde";
                    case 11:
                        return "abcd";
                    case 12:
                        return "abce";
                    case 13:
                        return "abde";
                    case 14:
                        return "acde";
                    case 15:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "ade":
                selection = Random.Range(0, 16);
                switch (selection)
                {
                    case 0:
                        return "ad";
                    case 1:
                        return "ae";
                    case 2:
                        return "bd";
                    case 3:
                        return "be";
                    case 4:
                        return "abd";
                    case 5:
                        return "abe";
                    case 6:
                        return "acd";
                    case 7:
                        return "ace";
                    case 8:
                        return "bcd";
                    case 9:
                        return "bce";
                    case 10:
                        return "bde";
                    case 11:
                        return "abcd";
                    case 12:
                        return "abce";
                    case 13:
                        return "abde";
                    case 14:
                        return "acde";
                    case 15:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "bcd":
                selection = Random.Range(0, 21);
                switch (selection)
                {
                    case 0:
                        return "ac";
                    case 1:
                        return "ad";
                    case 2:
                        return "bc";
                    case 3:
                        return "bd";
                    case 4:
                        return "be";
                    case 5:
                        return "cd";
                    case 6:
                        return "ce";
                    case 7:
                        return "abc";
                    case 8:
                        return "abd";
                    case 9:
                        return "abe";
                    case 10:
                        return "acd";
                    case 11:
                        return "ace";
                    case 12:
                        return "ade";
                    case 13:
                        return "bce";
                    case 14:
                        return "bde";
                    case 15:
                        return "cde";
                    case 16:
                        return "abcd";
                    case 17:
                        return "abce";
                    case 18:
                        return "abde";
                    case 19:
                        return "acde";
                    case 20:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "bce":
                selection = Random.Range(0, 18);
                switch (selection)
                {
                    case 0:
                        return "ad";
                    case 1:
                        return "bd";
                    case 2:
                        return "be";
                    case 3:
                        return "cd";
                    case 4:
                        return "ce";
                    case 5:
                        return "abd";
                    case 6:
                        return "abe";
                    case 7:
                        return "acd";
                    case 8:
                        return "ace";
                    case 9:
                        return "ade";
                    case 10:
                        return "bcd";
                    case 11:
                        return "bde";
                    case 12:
                        return "cde";
                    case 13:
                        return "abcd";
                    case 14:
                        return "abce";
                    case 15:
                        return "abde";
                    case 16:
                        return "acde";
                    case 17:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "bde":
                selection = Random.Range(0, 19);
                switch (selection)
                {
                    case 0:
                        return "ad";
                    case 1:
                        return "ae";
                    case 2:
                        return "bd";
                    case 3:
                        return "be";
                    case 4:
                        return "cd";
                    case 5:
                        return "ce";
                    case 6:
                        return "abd";
                    case 7:
                        return "abe";
                    case 8:
                        return "acd";
                    case 9:
                        return "ace";
                    case 10:
                        return "ade";
                    case 11:
                        return "bcd";
                    case 12:
                        return "bce";
                    case 13:
                        return "cde";
                    case 14:
                        return "abcd";
                    case 15:
                        return "abce";
                    case 16:
                        return "abde";
                    case 17:
                        return "acde";
                    case 18:
                        return "bcde";
                    default:
                        return "abde";
                }
            case "cde":
                selection = Random.Range(0, 25);
                switch (selection)
                {
                    case 0:
                        return "bd";
                    case 1:
                        return "be";
                    case 2:
                        return "cd";
                    case 3:
                        return "ce";
                    case 4:
                        return "de";
                    case 5:
                        return "bcd";
                    case 6:
                        return "bce";
                    case 7:
                        return "bde";
                    case 8:
                        return "bcde";
                    default:
                        return "bcde";
                }
            case "abcd":
                selection = Random.Range(0, 14);
                switch (selection)
                {
                    case 0:
                        return "ac";
                    case 1:
                        return "ad";
                    case 2:
                        return "bc";
                    case 3:
                        return "bd";
                    case 4:
                        return "be";
                    case 5:
                        return "abc";
                    case 6:
                        return "abd";
                    case 7:
                        return "abe";
                    case 8:
                        return "acd";
                    case 9:
                        return "ace";
                    case 10:
                        return "ade";
                    case 11:
                        return "bcd";
                    case 12:
                        return "bce";
                    case 13:
                        return "bde";
                    default:
                        return "ace";
                }
            case "abce":
                selection = Random.Range(0, 11);
                switch (selection)
                {
                    case 0:
                        return "ad";
                    case 1:
                        return "bd";
                    case 2:
                        return "be";
                    case 3:
                        return "abd";
                    case 4:
                        return "abe";
                    case 5:
                        return "acd";
                    case 6:
                        return "ace";
                    case 7:
                        return "ade";
                    case 8:
                        return "bcd";
                    case 9:
                        return "bce";
                    case 10:
                        return "bde";
                    default:
                        return "ace";
                }
            case "abde":
                selection = Random.Range(0, 12);
                switch (selection)
                {
                    case 0:
                        return "ad";
                    case 1:
                        return "ae";
                    case 2:
                        return "bd";
                    case 3:
                        return "be";
                    case 4:
                        return "abd";
                    case 5:
                        return "abe";
                    case 6:
                        return "acd";
                    case 7:
                        return "ace";
                    case 8:
                        return "ade";
                    case 9:
                        return "bcd";
                    case 10:
                        return "bce";
                    case 11:
                        return "bde";
                    default:
                        return "ace";
                }
            case "acde":
                selection = Random.Range(0, 11);
                switch (selection)
                {
                    case 0:
                        return "ad";
                    case 1:
                        return "bd";
                    case 2:
                        return "be";
                    case 3:
                        return "abd";
                    case 4:
                        return "abe";
                    case 5:
                        return "acd";
                    case 6:
                        return "ace";
                    case 7:
                        return "ade";
                    case 8:
                        return "bcd";
                    case 9:
                        return "bce";
                    case 10:
                        return "bde";
                    default:
                        return "ace";
                }
            case "bcde":
                selection = Random.Range(0, 14);
                switch (selection)
                {
                    case 0:
                        return "ad";
                    case 1:
                        return "bd";
                    case 2:
                        return "be";
                    case 3:
                        return "cd";
                    case 4:
                        return "ce";
                    case 5:
                        return "abd";
                    case 6:
                        return "abe";
                    case 7:
                        return "acd";
                    case 8:
                        return "ace";
                    case 9:
                        return "ade";
                    case 10:
                        return "bcd";
                    case 11:
                        return "bce";
                    case 12:
                        return "bde";
                    case 13:
                        return "cde";
                    default:
                        return "bcd";
                }
            default:
                return "abcde";
        }
    }

    public string[,] GeneratePathFromPathCodes(string[] myPathCodes)
    {
        string[,] pathToReturn = new string[,]
        {
            {"X", "X", "X", "X", "X"},
            {"X", "X", "X", "X", "X"},
            {"X", "X", "X", "X", "X"},
            {"X", "X", "X", "X", "X"},
            {"X", "X", "X", "X", "X"},
            {"X", "X", "X", "X", "X"}
        };
        for (int i = 5; i >= 0; i--)
        {
            if (myPathCodes[i].Contains("a"))
            {
                pathToReturn[i, 0] = "O";
            }
            if (myPathCodes[i].Contains("b"))
            {
                pathToReturn[i, 1] = "O";
            }
            if (myPathCodes[i].Contains("c"))
            {
                pathToReturn[i, 2] = "O";
            }
            if (myPathCodes[i].Contains("d"))
            {
                pathToReturn[i, 3] = "O";
            }
            if (myPathCodes[i].Contains("e"))
            {
                pathToReturn[i, 4] = "O";
            }
        }
        return pathToReturn;
    }
    
    public void IteratePath()
    {
        ClearPaths();
        List<int> validPaths = new List<int> {0, 1, 2, 3, 4};
        List<int> currentPaths = new List<int>();
        int tierNum = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                path[i, j] = "O";
            }
        }
        for (int i = 0; i < 3; i++)
        {
            int selection = Random.Range(0, validPaths.Count);
            currentPaths.Add(validPaths[selection]);
            validPaths.RemoveAt(selection);
        }
        BuildPaths(tierNum, currentPaths, path);

        for (int i = 3; i > 0; i--)
        {
            int maxPaths = Random.Range(3, 5);
            validPaths = FindValidPaths(currentPaths);
            if (validPaths.Count < maxPaths)
            {
                maxPaths = validPaths.Count;
            }
            currentPaths = FindRequiredPaths(currentPaths, validPaths);
            if (currentPaths.Count < maxPaths)
            {
                for (int j = 0; j < maxPaths - currentPaths.Count; j++)
                {
                    int selection = Random.Range(0, validPaths.Count);
                    currentPaths.Add(validPaths[selection]);
                    validPaths.RemoveAt(selection);
                }
            }
            tierNum++;
            BuildPaths(tierNum, validPaths, path);
        }
    }

    public void ClearPaths()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                path[i, j] = "O";
            }
        }
    }

    public List<int> FindValidPaths(List<int> currentTier)
    {
        List<int> toReturn = new List<int>();
        if (currentTier.Contains(0))
        {
            toReturn.Add(0);
            toReturn.Add(1);
        }
        if (currentTier.Contains(1))
        {
            toReturn.Add(0);
            toReturn.Add(1);
            toReturn.Add(2);
        }
        if (currentTier.Contains(2))
        {
            toReturn.Add(1);
            toReturn.Add(2);
            toReturn.Add(3);
        }
        if (currentTier.Contains(3))
        {
            toReturn.Add(2);
            toReturn.Add(3);
            toReturn.Add(4);
        }
        if (currentTier.Contains(4))
        {
            toReturn.Add(3);
            toReturn.Add(4);
        }
        return toReturn;
    }

    public List<int> FindRequiredPaths(List<int> targetCurrent, List<int> targetValid)
    {
        List<int> toReturn = new List<int>();
        
        if (targetCurrent.Contains(0) && targetCurrent.Contains(1) && targetCurrent.Contains(2))
        {
            toReturn.Add(1);
            targetValid.Remove(1);
        }
        else if (targetCurrent.Contains(0) && targetCurrent.Contains(1))
        {
            toReturn.Add(0);
            targetValid.Remove(0);
        }
        
        if (targetCurrent.Contains(1) && targetCurrent.Contains(2) && targetCurrent.Contains(3))
        {
            toReturn.Add(2);
            targetValid.Remove(2);
        }
        
        if (targetCurrent.Contains(2) && targetCurrent.Contains(3) && targetCurrent.Contains(4))
        {
            toReturn.Add(3);
            targetValid.Remove(3);
        }
        else if (targetCurrent.Contains(3) && targetCurrent.Contains(4))
        {
            toReturn.Add(4);
            targetValid.Remove(4);
        }
        return toReturn;
    }

    public void BuildPaths(int currentTierNum, List<int> currentTier, string[,] targetPath)
    {
        print("it happens here");
        foreach (var temp in currentTier)
        {
            targetPath[temp, currentTierNum] = "*D";
        }
        for (int i = 0; i < 5; i++)
        {
            if (targetPath[i, currentTierNum] == "O")
            {
                targetPath[i, currentTierNum] = "X";
            }
        }
    }

    public bool CheckPath()
    {
        bool valid = true;
        for (int i = 4; i > 0; i--)
        {
            if (path[i, 0] == "X" && path[i, 1] == "X" && path[i, 2] == "X" && path[i, 3] == "X" && path[i, 4] == "X")
            {
                valid = false;
            }
            if (path[i, 0].Contains("*D/"))
            {
                if (path[i - 1, 0] == "X" && path[i - 1, 1] == "X")
                {
                    valid = false;
                }
            }
            if (path[i, 1].Contains("*D/"))
            {
                if (path[i - 1, 0] == "X" && path[i - 1, 1] == "X" && path[i - 1, 2] == "X")
                {
                    valid = false;
                }
            }
            if (path[i, 2].Contains("*D/"))
            {
                if (path[i - 1, 1] == "X" && path[i - 1, 2] == "X" && path[i - 1, 3] == "X")
                {
                    valid = false;
                }
            }
            if (path[i, 3].Contains("*D/"))
            {
                if (path[i - 1, 2] == "X" && path[i - 1, 3] == "X" && path[i - 1, 4] == "X")
                {
                    valid = false;
                }
            }
            if (path[i, 4].Contains("*D/"))
            {
                if (path[i - 1, 3] == "X" && path[i - 1, 4] == "X")
                {
                    valid = false;
                }
            }
        }
        return valid;
    }

    public void ShowPath()
    {
        string willPrint = "";
        for (int i = 0; i < 6; i++)
        {
            willPrint += i + "= ";
            for (int j = 0; j < 5; j++)
            {
                willPrint += path[i, j] + " ";
                //print(path[i,j]);
            }

            willPrint += "\n";
        }
        print(willPrint);
    }

    public void GenerateMapFromPath(int tierLevel, string pathCode)
    {
        SetTier(tierLevel);
        SetPathCode(pathCode);
        GenerateMap();
    }

    public int GetCurrentTier()
    {
        return _currentTier;
    }

    public string GetCurrentPathCode()
    {
        return _currentPathCode;
    }

}
