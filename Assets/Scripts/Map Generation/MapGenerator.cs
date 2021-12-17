using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public string[,] path = new string[5, 5];
    //public string[,] demoMap = new string[1, 1];
    private int _currentTier;
    private string _currentPathCode;
    public int numDoneRooms, numDeadEnds, xMin, xMax, yMin, yMax, numDifferentRooms, numBossRooms;
    public List<int> deadEndXPos, deadEndYPos, allRoomsXPos, allRoomsYPos;
    public MapLoader myLoader;
    public Text screenText;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        allTierLimits = new[] {tier1Limits, tier2Limits, tier3Limits, tier4Limits, tier5Limits};
        ResetMap();
        //myLoader.complexMap = GenerateMapFromPath(1, "test");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GeneratePath();
            GenerateMapFromPath(1, "rupture");
            //ShowPath();
            //ShowMap();
        }
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
        bool pathReady = false;
        while (!pathReady)
        {
            pathReady = true;
            IteratePath();
            pathReady = CheckPath();
        }
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
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                willPrint += path[i, j].ToString() + " ";
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
