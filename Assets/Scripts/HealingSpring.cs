using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSpring : MonoBehaviour
{
    public SpriteRenderer mySR;
    public Sprite unusedSpring, usedSpring;
    public MapLoader myMap;
    public bool used;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSpring()
    {
        if (myMap.GetAreaMap()[myMap.currentXLoc, myMap.currentYLoc] == 'S')
        {
            gameObject.SetActive(true);
            mySR.sprite = unusedSpring;
            used = false;
        }
        else if (myMap.GetAreaMap()[myMap.currentXLoc, myMap.currentYLoc] == 'U')
        {
            gameObject.SetActive(true);
            mySR.sprite = usedSpring;
            used = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!used && other.CompareTag("Player"))
        {
            used = true;
            other.gameObject.GetComponent<Player>().RestoreFullHealth();
            mySR.sprite = usedSpring;
            myMap.GetAreaMap()[myMap.currentXLoc, myMap.currentYLoc] = 'U';
        }
    }
}
