using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipmentButton : MonoBehaviour
{
    public Image sp;

    bool testBool;

    void Start()
    {
        
    }

    void Update()
    {
        if (testBool)
            sp.color = Color.black;
        else
            sp.color = Color.white;
    }

    public void ToggleColor()
    {
        if (testBool)
            testBool = false;
        else
            testBool = true;
    }
}
