using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollections : MonoBehaviour
{
    public Room[] myRooms;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAllInactive()
    {
        foreach (var temp in myRooms)
        {
            temp.gameObject.SetActive(false);
        }
    }

    public void ActivateRoom(int whichRoom)
    {
        if (whichRoom < myRooms.Length)
        {
            myRooms[whichRoom].gameObject.SetActive(true);
        }
    }
}
