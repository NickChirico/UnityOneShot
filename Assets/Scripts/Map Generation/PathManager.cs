using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PathManager : MonoBehaviour
{
    public PathCollection myPathCollection;

    public PlayerLoader myPlayer;

    public PathOption[] allOptions, tier1Options, tier2Options, tier3Options, tier4Options, tier5Options, tier6Options;

    public PathSummary mySummary;

    public int currentOption, layerNum;

    public bool choosingPath, usingPremade;

    public PlayerInputActions navActions;

    public InputAction navigateLeft, navigateRight, selectOption;

    public Text layerText;

    public MapGenerator myMapGen;

    public PathOption[][] allPathOptions;

    public string[] newPaths;
    public string previousPath;
    public Color myBackgroundColor;

    //public InputAction menuNav;

    public InputActionMap uiMap;
    // Start is called before the first frame update
    private void Awake()
    {
        navActions = new PlayerInputActions();
        navActions.UI.Enable();
    }

    void Start()
    {

        DontDestroyOnLoad(gameObject);
        //5 - layerNum = myPlayer.currentPathLevel;
        //previousPaths = new[] {"healing", "shop", "mystery"};
        //LoadPathScene();
    }

    // Update is called once per frame
    void Update()
    {
        //selectOption.started += SelectOptionOnstarted;
        if (choosingPath && SceneManager.GetActiveScene().name == "MapScene")
        {
            if (navActions.UI.NavigateLeft.triggered)
            {
                //print("left, " + currentOption);
                if (currentOption == 4 && allPathOptions[5 - layerNum][3].valid)
                {
                    currentOption = 3;
                    UpdateOptions();
                }
                else if (currentOption > 2 && allPathOptions[5 - layerNum][2].valid)
                {
                    currentOption = 2;
                    UpdateOptions();
                }
                else if (currentOption > 1 && allPathOptions[5 - layerNum][1].valid)
                {
                    currentOption = 1;
                    UpdateOptions();
                }
                else if (currentOption > 0 && allPathOptions[5 - layerNum][0].valid)
                {
                    currentOption = 0;
                    UpdateOptions();
                }
            }

            if (navActions.UI.NavigateRight.triggered)
            {
                //print("right, " + currentOption);
                if (currentOption == 0 && allPathOptions[5 - layerNum][1].valid)
                {
                    //Debug.Log("1 is  valid");
                    currentOption = 1;
                    UpdateOptions();
                }
                else if (currentOption < 2 && allPathOptions[5 - layerNum][2].valid)
                {
                    //Debug.Log("2 is valid");
                    currentOption = 2;
                    UpdateOptions();
                }
                else if (currentOption < 3 && allPathOptions[5 - layerNum][3].valid)
                {
                    //Debug.Log("3 is valid");
                    currentOption = 3;
                    UpdateOptions();
                }
                else if (currentOption < 4 && allPathOptions[5 - layerNum][4].valid)
                {
                    //Debug.Log("4 is valid");
                    currentOption = 4;
                    UpdateOptions();
                }
            }

            if (navActions.UI.Select.triggered)
            {
                myBackgroundColor = allPathOptions[5 - layerNum][currentOption].areaBackground.color;
                if (usingPremade)
                {
                    myMapGen.UpdatePregenMap(myMapGen.allPregens[layerNum], allPathOptions[5 - layerNum][currentOption].GetPathCode());
                    myMapGen.roomArray = myMapGen.allPregens[layerNum];
                }
                else
                {
                    //previousPath = allOptions[currentOption].GetPathCode();
                    //myMapGen.GenerateMap("North");
                    myMapGen.GenerateMapFromPath(layerNum, allPathOptions[5 - layerNum][currentOption].GetPathCode());
                    //myMapGen.ShowMap();

                }
                myMapGen.UpdatePath(5 - layerNum, currentOption);
                myMapGen.ShowMap();
                SceneManager.LoadScene("SingleRoomIso");
                choosingPath = false; //HERE set to true again when we load back in!
            }
        }
    }

    public void UpdateNextPath(int layer, int whichOption)
    {

    }

    private void SelectOptionOnStarted(InputAction.CallbackContext obj)
    {
        print("funny");
        //throw new System.NotImplementedException();
    }

    int endCount = 0;
    public bool DoEnding()
    {
        if (endCount >= 3)
            return true;
        else
            return false;
    }

    public bool IsFirstLevel()
    {
        if (endCount <= 1)
            return true;
        else return false;
    }

    int roomsCleared = -1;
    public void DoNewSection()
    {
        roomsCleared++;

        if (IsFirstLevel())
        {
            if (roomsCleared == 0)
            {
                //UI_Manager.GetUIManager.InitialControlPanel();
                UI_Manager.GetUIManager.ShowInitInstructions(true);
            }
            else if (roomsCleared == 1)
            {
                // ENEMY TUTORIAL;
                UI_Manager.GetUIManager.DoShowEnemyPanel();
            }
        }
    }

    public void EnterMapScene()
    {
        endCount++;
        myPlayer.playerLoaded = false;
        layerNum = myPlayer.currentPathLevel;
        SceneManager.LoadScene("MapScene");
        print("Loaded into scene now");
        choosingPath = true;
    }

    public void LoadLevel()
    {
        
    }

    public void InitialOptions()
    {
        print("choosing initial options");
        //Debug.Log(allPathOptions[5 - layerNum][0].valid.ToString());
        if (allPathOptions[5 - layerNum][0].valid)
        {
            //Debug.Log("initial option is 0");
            currentOption = 0;
            UpdateOptions();
        }
        else if (allPathOptions[5 - layerNum][1].valid)
        {
            //Debug.Log("initial option is 1");
            currentOption = 1;
            UpdateOptions();
        }
        else if (allPathOptions[5 - layerNum][2].valid)
        {
            //Debug.Log("initial option is 2");
            currentOption = 2;
            UpdateOptions();
        }
        else if (allPathOptions[5 - layerNum][3].valid)
        {
            //Debug.Log("initial option is 3");
            currentOption = 3;
            UpdateOptions();
        }
        else if (allPathOptions[5 - layerNum][4].valid)
        {
            //Debug.Log("initial option is 4");
            currentOption = 4;
            UpdateOptions();
        }
    }

    public void UpdateOptions()
    {
        //print("updating options");
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                allPathOptions[i][j].ChangeSelection(false);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (i == currentOption)
            {
                allPathOptions[5 - layerNum][i].ChangeSelection(true);
                string myPathCode = allPathOptions[5 - layerNum][i].GetPathCode();
                if (myPathCode.Contains("1"))
                {
                    if (mySummary == null)
                    {
                        print("it's null");
                    }
                    mySummary.biomeDescription.text = mySummary.grasslandsDescription;
                }
                else if (myPathCode.Contains("2"))
                {
                    mySummary.biomeDescription.text = mySummary.desertDescription;
                }
                else if (myPathCode.Contains("3"))
                {
                    mySummary.biomeDescription.text = mySummary.volcanoDescription;
                }
                else
                {
                    mySummary.biomeDescription.text = "You shouldn't be seeing this.";
                }

                if (myPathCode.Contains("A"))
                {
                    mySummary.positiveFaction.text = mySummary.proAcademy;
                    mySummary.negativeFaction.text = mySummary.antiChurch;
                }
                else if(myPathCode.Contains("a"))
                {
                    mySummary.positiveFaction.text = mySummary.proAcademy;
                    mySummary.negativeFaction.text = mySummary.antiGuild;
                }
                else if(myPathCode.Contains("C"))
                {
                    mySummary.positiveFaction.text = mySummary.proChurch;
                    mySummary.negativeFaction.text = mySummary.antiGuild;
                }
                else if(myPathCode.Contains("c"))
                {
                    mySummary.positiveFaction.text = mySummary.proChurch;
                    mySummary.negativeFaction.text = mySummary.antiAcademy;
                }
                else if(myPathCode.Contains("G"))
                {
                    mySummary.positiveFaction.text = mySummary.proGuild;
                    mySummary.negativeFaction.text = mySummary.antiAcademy;
                }
                else if(myPathCode.Contains("g"))
                {
                    mySummary.positiveFaction.text = mySummary.proGuild;
                    mySummary.negativeFaction.text = mySummary.antiChurch;
                }
                else
                {
                    mySummary.positiveFaction.text = "You shouldn't be seeing this.";
                    mySummary.negativeFaction.text = "You shouldn't be seeing this.";
                }
            }
        }
        
    }

    public void LoadPathScene()
    {
        choosingPath = true;
        if (SceneManager.GetActiveScene().name != "MapScene")
        {
            SceneManager.LoadScene("MapScene");
        }
        //allOptions[0] = GameObject.Find("Left Option").GetComponent<PathOption>();
        //allOptions[1] = GameObject.Find("Middle Option").GetComponent<PathOption>();
        //allOptions[2] = GameObject.Find("Right Option").GetComponent<PathOption>();
        //layerText = GameObject.Find("Layer Text").GetComponent<Text>();
        //layerText.text = "Layer: " + 5 - layerNum;
    }

    public void Initiate()
    {
        newPaths = GenerateOptions();
        for (int i = 0; i < allOptions.Length; i++)
        {
            allOptions[i].SetPath(newPaths[i]);
        }
        currentOption = 0;
        UpdateOptions();
    }

    public string[] GenerateOptions()
    {
        string[] options = new string[3];
        List<string> pathOptions = new List<string>()
            { "rupture", "siphon", "contaminate", "gold", "healing", "shop", "weapon", "mystery"};
        if (5 - layerNum == 1)
        {
            pathOptions.Remove("healing");
            pathOptions.Remove("shop");
            pathOptions.Remove("mystery");
        }
        else
        {
            //pathOptions.Remove(previousPath);
        }
        int currentSelection;
        for (int i = 0; i < options.Length; i++)
        {
            currentSelection = Random.Range(0, pathOptions.Count);
            //print(currentSelection);
            //print(pathOptions.Count);
            options[i] = pathOptions[currentSelection];
            pathOptions.RemoveAt(currentSelection);
            
        }

        string[] demoOptions = {"rupture", "contaminate", "siphon"};
        return demoOptions;
    }

    public void GetPathOptions()
    {
        tier1Options = myPathCollection.tier1Options;
        tier2Options = myPathCollection.tier2Options;
        tier3Options = myPathCollection.tier3Options;
        tier4Options = myPathCollection.tier4Options;
        tier5Options = myPathCollection.tier5Options;
        tier6Options = myPathCollection.tier6Options;
        allPathOptions =  new []{tier6Options, tier5Options, tier4Options, tier3Options, tier2Options, tier1Options};
    }
    
    public void GeneratePathMapOnScreen(string[,] inputArray)
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                MakePathOptionVisible(allPathOptions[i][j], inputArray[i,j]);
            }
        }
    }

    public void MakePathOptionVisible(PathOption inputOption, string inputCode)
    {
        inputOption.SetPathCode(inputCode);
        if (inputOption.GetPathCode() == "X")
        {
            inputOption.valid = false;
            inputOption.gameObject.SetActive(false);
        }
        else
        {
            char biomeSelection = inputOption.GetPathCode().ToCharArray()[1];
            char factionSelection = inputOption.GetPathCode().ToCharArray()[2];
            char initialChar = inputOption.GetPathCode().ToCharArray()[0];
            if (initialChar == '*')
            {
                inputOption.grayOverlay.enabled = true;
                inputOption.valid = false;
            }
            else
            {
                inputOption.grayOverlay.enabled = false;
                if (initialChar == '?')
                {
                    //Debug.Log("question mark here");
                    inputOption.valid = true;
                }
            }

            switch (biomeSelection)
            {
                case '1':
                    inputOption.areaBackground.color = inputOption.grasslandsColor;
                    break;
                case '2':
                    inputOption.areaBackground.color = inputOption.desertColor;
                    break;
                case '3':
                    inputOption.areaBackground.color = inputOption.volcanoColor;
                    break;
                default:
                    inputOption.areaBackground.color = inputOption.grasslandsColor;
                    break;
            }

            switch (factionSelection)
            {
                case 'a':
                    inputOption.pathSymbol.sprite = inputOption.academySymbol;
                    break;
                case 'A':
                    inputOption.pathSymbol.sprite = inputOption.academySymbol;
                    break;
                case 'c':
                    inputOption.pathSymbol.sprite = inputOption.churchSymbol;
                    break;
                case 'C':
                    inputOption.pathSymbol.sprite = inputOption.churchSymbol;
                    break;
                case 'g':
                    inputOption.pathSymbol.sprite = inputOption.guildSymbol;
                    break;
                case 'G':
                    inputOption.pathSymbol.sprite = inputOption.guildSymbol;
                    break;
            }
        }
    }
}