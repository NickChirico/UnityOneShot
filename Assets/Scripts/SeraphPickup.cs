using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeraphPickup : Pickup
{
    public string seraphCode;
    public int seraphType;
    public SeraphController myController;
    public Sprite ruptureSprite, siphonSprite, contaminateSprite;

    // Start is called before the first frame update
    void Awake()
    {
        myController = GameObject.Find("SeraphController").GetComponent<SeraphController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ChoosePickupImage(string pickupCode)
    {
        base.ChoosePickupImage(pickupCode);
    }

    public override void OnPickup()
    {
        myController.SpawnSeraph(seraphCode);
        base.OnPickup();
    }

    public override void Interact()
    {
        base.Interact();
        print("interacting as a seraph pickup");
    }
}
