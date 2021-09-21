using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempManager : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject GruntOneShot;
    public GameObject Grunt50;
    public GameObject Grunt200;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1));
            Instantiate(GruntOneShot, targetPos, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1));
            Instantiate(Grunt50, targetPos, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1));
            Instantiate(Grunt200, targetPos, Quaternion.identity);
        }
    }
}
