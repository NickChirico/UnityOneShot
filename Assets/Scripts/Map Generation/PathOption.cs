using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathOption : MonoBehaviour
{
    public bool selected, valid;
    public Image highlightImage, pathSymbol, grayOverlay, areaBackground;
    private string _pathCode, _pathInformation, _areaInformation;
    public Color desertColor, grasslandsColor, volcanoColor;
    public Sprite churchSymbol, guildSymbol, academySymbol;
    
    // Start is called before the first frame update
    void Start()
    {
        _pathInformation = "";
        _areaInformation = "";
        
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
        
        _pathCode = tempCode;
        
    }

    public void SetPathCode(string inputCode)
    {
        _pathCode = inputCode;
    }

    public string GetPathCode()
    {
        return _pathCode;
    }
    
    public void SetPathInfo(string inputInfo)
    {
        _pathInformation = inputInfo;
    }

    public string GetPathInfo()
    {
        return _pathInformation;
    }
    
    public void SetAreaInfo(string inputInfo)
    {
        _areaInformation = inputInfo;
    }

    public string GetAreaInfo()
    {
        return _areaInformation;
    }

}
