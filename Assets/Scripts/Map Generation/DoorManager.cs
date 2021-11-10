using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public MapGenerator myMap;
    public GameObject deadEnd, locked, unlocked;
    public MapLoader myMapLoader;
    public DoorLoader myLoader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNewDoor(int targetX, int targetY)
    {
        //unlocked.SetActive(false);
        if (myMapLoader.operatingMap[targetX, targetY] == 'X')
        {
            unlocked.SetActive(false);
            myLoader.enabled = false;
            deadEnd.SetActive(true);
        }
        else if (myMapLoader.operatingMap[targetX, targetY] == 'D')
        {
            unlocked.SetActive(true);
            myLoader.enabled = true;
            myLoader.traveled = false;
            deadEnd.SetActive(false);
        }
    }

    public void Unlock()
    {
        if (locked.activeInHierarchy)
        {
            locked.SetActive(false);
            unlocked.SetActive(true);
        }
    }
}