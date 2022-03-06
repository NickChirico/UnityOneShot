using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    // Start is called before the first frame update
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
            col.GetComponent<Player>().SetInteract(true, this);
        }
        throw new NotImplementedException();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<Player>().SetInteract(false, this);
        throw new NotImplementedException();
    }
}
