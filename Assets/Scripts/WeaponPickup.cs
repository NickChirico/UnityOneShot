using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    public int weaponNum;

    public Sprite meleeSprite, rangedSprite; //ranged weapons are negative, melee weapons are positive
    //randomly generate from -4 (inclusive) to 4 (exclusive)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ChoosePickupImage(int pickupCode)
    {
        GetComponent<SpriteRenderer>().sprite = pickupCode >= 0 ? meleeSprite : rangedSprite;
    }
}
