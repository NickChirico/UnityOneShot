using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingPremade : MonoBehaviour
{
    public PathManager myPathManager;
    public PlayerLoader myPlayerLoader;
    public MapGenerator myMapGenerator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseIfPremade(bool premade)
    {
        myPathManager.usingPremade = premade;
        myMapGenerator.usingPremade = premade;
        myMapGenerator.LoadMapScene();
    }
}
