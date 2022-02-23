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
    void Start()
    {
        myController = GameObject.Find("SeraphController").GetComponent<SeraphController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ChoosePickupImage(int pickupCode)
    {
        switch (pickupCode)
        {
            case 0:
                GetComponent<SpriteRenderer>().sprite = ruptureSprite;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = siphonSprite;
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = contaminateSprite;
                break;
        }
    }

    public override void OnPickup()
    {
        used = true;
        myController.SpawnSeraph(seraphType);
        Destroy(gameObject);
    }
}
