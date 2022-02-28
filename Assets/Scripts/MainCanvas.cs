using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    Canvas c;

    private void Awake()
    {
        c = this.GetComponent<Canvas>();
    }
    public Canvas GetCanvas()
    {
        return c;
    }
}
