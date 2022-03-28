using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public bool interactable;

    void Awake()
    {
        interactable = false;
    }
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //col.GetComponent<Player>().SetInteract(true, this);
        }
        //throw new NotImplementedException();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.GetComponent<Player>().SetInteract(false, this);
        }
        //throw new NotImplementedException();
    }

    public virtual void Interact()
    {
        print("interacting now");
    }
}
