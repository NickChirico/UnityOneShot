using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;

    int healthMax, healthCur;

    public void SetVals(int hp)
    {
        healthMax = hp;
        healthCur = hp;
        healthBar.fillAmount = 1;
    }

    private void Update()
    {
        
    }

    public void SetHealth(float percent)
    {
        healthBar.fillAmount = percent;
    }

}
