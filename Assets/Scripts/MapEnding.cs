using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnding : MonoBehaviour
{
    public GameObject EndScreen;
    void Start()
    {
        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        yield return new WaitForSeconds(1f);
        bool b = FindObjectOfType<PathManager>().DoEnding();

        
        if(b)
        { EndScreen.SetActive(true);
        }
    }


}
