using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapEnding : MonoBehaviour
{
    public GameObject EndScreen;
    public GameObject EndButton;
    void Start()
    {
        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        yield return new WaitForSeconds(0.25f);
        bool b = FindObjectOfType<PathManager>().DoEnding();

        
        if(b)
        { 
            EndScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(EndButton);
        }
    }


}
