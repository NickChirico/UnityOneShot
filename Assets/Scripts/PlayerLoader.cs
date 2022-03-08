using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public List<Seraph> baggedSeraphim, mainSeraphim, altSeraphim, armorSeraphim, bootsSeraphim, flaskSeraphim;
    public string mainWeaponCode, altWeaponCode, armorCode, bootsCode, flaskCode;
    public Weapon mainWeapon, altWeapon;
    public Armor armor;
    public Boots boots;
    public Flask flask;
    public int currentHealth, currentChitin, currentBlood, currentBrains, currentFlaskCharges, currentEssence, currentPathLevel; //currentPathLevel gets added after finishing level
    public GameObject playerPrefab;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Player LoadIntoRoom(Vector3 playerSpawnPos)
    {
        return Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity).GetComponent<Player>().LoadIntoLevel(this);
        //set everything in the player
    }
}
