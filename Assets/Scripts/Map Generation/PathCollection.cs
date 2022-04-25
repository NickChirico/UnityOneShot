using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCollection : MonoBehaviour
{
    public PathOption[] tier1Options, tier2Options, tier3Options, tier4Options, tier5Options, tier6Options;

    public PathManager myPathManager;
    // Start is called before the first frame update

    void Awake()
    {
        myPathManager = GameObject.Find("Path Manager").GetComponent<PathManager>();
        myPathManager.myPathCollection = this;
        myPathManager.GetPathOptions();
        if (myPathManager.usingPremade)
        {
            myPathManager.GeneratePathMapOnScreen(myPathManager.myMapGen.pregenPath);
        }
        else
        {
            myPathManager.GeneratePathMapOnScreen(myPathManager.myMapGen.path);
        }

        myPathManager.mySummary = GameObject.Find("Path Info Panel").GetComponent<PathSummary>();
        myPathManager.InitialOptions();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
