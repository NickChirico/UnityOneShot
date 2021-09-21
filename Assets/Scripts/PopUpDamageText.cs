﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpDamageText : MonoBehaviour
{
    public Color colorLow;
    public Color colorHigh;
    public int midDamage;

    public Vector3 smallScale, largeScale;

    public void SetTextRun(int damage)
    {
        TextMeshPro textMesh = this.GetComponentInChildren<TextMeshPro>();
        textMesh.text = "" + damage;

        if (damage >= midDamage)
        {
            textMesh.color = colorHigh;
            this.transform.localScale = largeScale;
        }
        else
        {
            textMesh.color = colorLow;
            this.transform.localScale = smallScale;
        }
    }

    // Called in child - DestroyParent - NOT THIS CODE
    public void DestoryThis()
    {
        Destroy(this.gameObject);
    }
}
