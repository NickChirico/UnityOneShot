using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Pickup : InteractableObject
{
    public PlayerInputActions PickUpAction;

    public bool used;

    public int numInRoom;


    // Start is called before the first frame update
    void Start()
    {
        PickUpAction = new PlayerInputActions();
        PickUpAction.Player.Enable();
        used = false;
        interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual Pickup CreatePickup(string pickupCode, int inputNumInRoom)
    {
        ChoosePickupImage(pickupCode);
        numInRoom = inputNumInRoom;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        return this;
    }
    
    
    /*
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !used && interactable)
        {
            OnPickup();
        }
        if (other.gameObject.CompareTag("Activator") && !interactable)
        {
            interactable = true;
        }
    }
    */

    public virtual void ChoosePickupImage(string pickupCode)
    {
        Sprite sprite;
        GameObject.Find("Master Dictionary").GetComponent<MasterDictionary>().SpriteDictionary.TryGetValue(pickupCode, out sprite);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public virtual void OnPickup()
    {
        used = true;
        GameObject.Find("Map Loader").GetComponent<MapLoader>().loadedRoom.RemovePickup(numInRoom);
        Destroy(gameObject);
    }
    
    public override void Interact()
    {
        base.Interact();
        print("interacting as a pickup");
    }
}
