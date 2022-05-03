using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endAnim : MonoBehaviour
{
    public MeleeWeapon weap;
    float test;

    public void HideWeapon()
    {
        if(weap != null)
            weap.SetIndicator(false);
    }
}
