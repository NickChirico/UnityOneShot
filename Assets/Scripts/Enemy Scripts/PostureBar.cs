using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostureBar : MonoBehaviour
{
    public Image bar_right, bar_left;

    public Color normalColor, brokenColor;

    float postureMax;
    float curPosture;

    void Start()
    {
        bar_right.color = normalColor;
        bar_left.color = normalColor;
    }

    public void SetVals(float max)
    { 
        postureMax = max;
        curPosture = 0;
    }

    void Update()
    {
        bar_right.fillAmount = curPosture / postureMax;
        bar_left.fillAmount = curPosture / postureMax;
    }

    public void SetPosture(float pos)
    {
        curPosture = pos;
    }


    public void GuardBreak(bool doBreak)
    {
        if (doBreak)
        {
            bar_right.color = brokenColor;
            bar_left.color = brokenColor;
        }
        else
        {
            bar_right.color = normalColor;
            bar_left.color = normalColor;
        }
    }
}
