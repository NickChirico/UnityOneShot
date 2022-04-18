using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPortal : DoorLoader
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        throw new NotImplementedException();
    }
}
