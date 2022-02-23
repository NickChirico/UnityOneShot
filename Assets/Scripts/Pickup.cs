using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Pickup : MonoBehaviour
{
    public PlayerInputActions PickUpAction;

    public bool used, pickupAble;


    // Start is called before the first frame update
    void Start()
    {
        PickUpAction = new PlayerInputActions();
        PickUpAction.Player.Enable();
        used = false;
        pickupAble = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePickup(int pickupCode)
    {
        ChoosePickupImage(pickupCode);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !used && pickupAble)
        {
            OnPickup();
        }
        if (other.gameObject.CompareTag("Activator") && !pickupAble)
        {
            pickupAble = true;
        }
    }

    public virtual void ChoosePickupImage(int pickupCode)
    {
        
    }

    public virtual void OnPickup()
    {
        
    }
}
