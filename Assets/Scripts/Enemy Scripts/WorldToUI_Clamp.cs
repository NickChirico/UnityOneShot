using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldToUI_Clamp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 postureBarPos = Camera.main.WorldToScreenPoint(this.transform.position);
        //postureBar.transform.position = postureBarPos;
    }
}
