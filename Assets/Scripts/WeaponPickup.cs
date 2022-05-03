using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    public int weaponNum;
    public string weaponCode;

    public UI_Manager myUIManager;

    public PlayerController myPlayerController;
    //randomly generate from -4 (inclusive) to 4 (exclusive)
    // Start is called before the first frame update
    void Awake()
    {
        myUIManager = UI_Manager.GetUIManager;
        myPlayerController = GameObject.Find("PLAYER(Clone)").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPickup()
    {
        
    }

    public override void Interact()
    {
        myUIManager.ToggleWeaponPickupPanel();
    }
}
