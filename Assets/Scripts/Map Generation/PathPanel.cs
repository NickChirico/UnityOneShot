using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathPanel : MonoBehaviour
{
    public PathOption leftPath, middlePath, rightPath;
    public Text myLayerText;
    public PathManager myManager;
    
    // Start is called before the first frame update
    void Start()
    {
        myManager = GameObject.Find("Path Manager").GetComponent<PathManager>();
        myManager.allOptions[0] = leftPath;
        myManager.allOptions[1] = middlePath;
        myManager.allOptions[2] = rightPath;
        myLayerText.text = "Layer: " + myManager.layerNum;
        myManager.Initiate();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
