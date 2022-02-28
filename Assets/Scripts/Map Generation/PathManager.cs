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

    
    public PlayerLoader myPlayer;

    public PathOption[] allOptions, tier1Options, tier2Options, tier3Options, tier4Options, tier5Options, tier6Options;

    public int currentOption, layerNum;

    public bool choosingPath;

    public PlayerInputActions navActions;

    public InputAction navigateLeft, navigateRight, selectOption;

    public Text layerText;

    public MapGenerator myMapGen;

    public string[] newPaths;
    public string previousPath;

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
        layerNum = myPlayer.currentPathLevel;
        //previousPaths = new[] {"healing", "shop", "mystery"};
        //LoadPathScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadPathScene();
        }
        //selectOption.started += SelectOptionOnstarted;
        if (choosingPath)
        {
            if (navActions.UI.NavigateLeft.triggered)
            {
                print("left");
                if (currentOption > 0)
                {
                    currentOption--;
                    UpdateOptions();
                }
            }

            if (navActions.UI.NavigateRight.triggered)
            {
                print("right");
                if (currentOption < 2)
                {
                    currentOption++;
                    UpdateOptions();
                }
            }

            if (navActions.UI.Select.triggered)
            {
                print("here ya go");
                previousPath = allOptions[currentOption].pathCode;
                print("funny");
                //myMapGen.GenerateMap("North");
                myMapGen.GenerateMapFromPath(layerNum, allOptions[currentOption].pathCode);
                //myMapGen.ShowMap();
                SceneManager.LoadScene("SingleRoomIso");
                choosingPath = false;
            }
        }
    }

    private void SelectOptionOnStarted(InputAction.CallbackContext obj)
    {
        print("funny");
        //throw new System.NotImplementedException();
    }

    public void LoadLevel()
    {
        
    }

    public void UpdateOptions()
    {
        print("updating options");
        for (int i = 0; i < allOptions.Length; i++)
        {
            if (i == currentOption)
            {
                allOptions[i].ChangeSelection(true);
            }
            else
            {
                allOptions[i].ChangeSelection(false);
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
        //layerText.text = "Layer: " + layerNum;
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
        if (layerNum == 1)
        {
            pathOptions.Remove("healing");
            pathOptions.Remove("shop");
            pathOptions.Remove("mystery");
        }
        else
        {
            pathOptions.Remove(previousPath);
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

    public void GeneratePathMapOnScreen(string[,] inputArray)
    {
        PathOption[][] allPathOptions = 
            {tier6Options, tier5Options, tier4Options, tier3Options, tier2Options, tier1Options};
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                
            }
        }
    }
}
