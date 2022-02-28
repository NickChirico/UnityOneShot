using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathOption : MonoBehaviour
{
    public bool selected, valid;
    public Image highlightImage, pathSymbol, grayOverlay;
    public string pathCode;
    public Color desertColor, grasslandsColor, volcanoColor;
    public Sprite churchSymbol, guildSymbol, academySymbol;
    
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("Path Manager").GetComponent<PathManager>().allOptions[pathNum] = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSelection(bool nowSelected)
    {
        selected = nowSelected;
        highlightImage.enabled = nowSelected;
    }

    public void SetPath(string tempCode)
    {
        
        pathCode = tempCode;
        
    }

}
