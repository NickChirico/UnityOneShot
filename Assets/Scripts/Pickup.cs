using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Pickup : MonoBehaviour
{
    public PlayerInputActions pickUpAction;

    public int seraphType;

    public SeraphController myController;

    public bool used, pickupAble;

    public Image pickupImage;
    
    
    // Start is called before the first frame update
    void Start()
    {
        myController = GameObject.Find("SeraphController").GetComponent<SeraphController>();
        pickUpAction = new PlayerInputActions();
        pickUpAction.Player.Enable();
        used = false;
        pickupAble = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !used && pickupAble)
        {
            OnPickup();
        }
        if(other.gameObject.CompareTag("Activator") && !pickupAble)
        {
            pickupAble = true;
        }
    }

    public virtual void OnPickup()
    {
        
    }
}
