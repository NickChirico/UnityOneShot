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

    public Player myPlayer;

    public PathOption[] allOptions;

    public int currentOption, layerNum;

    public bool choosingPath;

    public PlayerInputActions navActions;

    public InputAction navigateLeft, navigateRight, selectOption;

    public Text layerText;

    //public MapGenerator myMapGen;

    public string[] previousPaths, newPaths;

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
        layerNum = 0;
        previousPaths = new[] {"healing", "shop", "mystery"};
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
                for (int i = 0; i < allOptions.Length; i++)
                {
                    previousPaths[i] = allOptions[i].pathCode;
                }
                //myMapGen.GenerateMapFromPath(layerNum, allOptions[currentOption].pathCode);
                choosingPath = false;
            }
        }
    }

    private void SelectOptionOnstarted(InputAction.CallbackContext obj)
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
        for (int i = 0; i < previousPaths.Length; i++)
        {
            pathOptions.Remove(previousPaths[i]);
        }
        int currentSelection;
        for (int i = 0; i < options.Length; i++)
        {
            currentSelection = Random.Range(0, pathOptions.Count);
            print(currentSelection);
            print(pathOptions.Count);
            options[i] = pathOptions[currentSelection];
            pathOptions.RemoveAt(currentSelection);
            
        }
        return options;
    }
}
