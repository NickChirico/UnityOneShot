using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerInputActions pickUpAction;

    public int seraphType;

    public SeraphController myController;

    public bool used;
    // Start is called before the first frame update
    void Start()
    {
        myController = GameObject.Find("SeraphController").GetComponent<SeraphController>();
        pickUpAction = new PlayerInputActions();
        pickUpAction.Player.Enable();
        used = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !used)
        {
            used = true;
            myController.SpawnSeraph(seraphType);
            Destroy(gameObject);
        }
    }
}
