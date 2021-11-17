using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLoader : MonoBehaviour
{
    public string direction;
    public MapLoader myMapLoader;
    public bool traveled;
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
        if (other.CompareTag("Player") && !traveled)
        {
            traveled = true;
            myMapLoader.LoadRoom(direction);
        }
    }
}
