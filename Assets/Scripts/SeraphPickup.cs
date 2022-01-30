using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeraphPickup : Pickup
{
    public string seraphCode;
    public int seraphType;
    public SeraphController myController;

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
        
        throw new NotImplementedException();
    }

    public override void OnPickup()
    {
        
    }
}
