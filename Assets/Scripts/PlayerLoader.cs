using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLoader : MonoBehaviour
{
    public bool usingMouse;

    public List<int> baggedSeraphim, mainSeraphim, altSeraphim, armorSeraphim, bootsSeraphim, flaskSeraphim;
    public string mainWeaponCode, altWeaponCode, armorCode, bootsCode, flaskCode;
    public Weapon mainWeapon, altWeapon;
    public Armor armor;
    public Boots boots;
    public Flask flask;
    public int currentHealth, currentChitin, currentBlood, currentBrains, currentFlaskCharges, currentEssence, currentPathLevel; //currentPathLevel gets added after finishing level
    public GameObject playerPrefab;
    public bool playerLoaded;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        usingMouse = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Player LoadIntoRoom(Vector3 playerSpawnPos)
    {
        if (playerLoaded)
        {
            throw new Exception();
            return null;
        }
        else
        {
            playerLoaded = true;
            return Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity).GetComponent<Player>().LoadIntoLevel(this);
        }
        //set everything in the player
    }

    public bool PreLoadingSeraphs()
    {
        bool toReturn = false;
        if (baggedSeraphim.Count > 0)
        {
            print("Pre loaded in bag");
            toReturn = true;
        }
        else if (mainSeraphim.Count > 0)
        {
            print("Pre loaded on main");
            toReturn = true;
        }
        else if (altSeraphim.Count > 0)
        {
            print("Pre loaded on alt");
            toReturn = true;
        }
        else if (armorSeraphim.Count > 0)
        {
            print("Pre loaded on armor");
            toReturn = true;
        }
        
        return toReturn;
    }

    public TextMeshProUGUI displayText;
    public void ToggleControl()
    {
        usingMouse = !usingMouse;

        if (displayText != null)
        {
            if (usingMouse)
                displayText.text = "Keyboard + Mouse";
            else
                displayText.text = "XBOX Controller";
        }
    }

    public bool GetUsingMouse()
    { return usingMouse; }
}
