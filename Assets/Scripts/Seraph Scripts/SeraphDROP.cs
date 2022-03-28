using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeraphDROP : MonoBehaviour
{
    SeraphController seraphControl;
    public enum SeraphType { Rupture, Siphon, Contam, Storm }
    public SeraphType upgradeType;
    //public int value; // 0 == rup, 1 == siph, 2 == contam, 3 == storm;
    void Start()
    {
        seraphControl = SeraphController.GetSeraphController;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            seraphControl.SpawnSeraph(((int)upgradeType));
            Destroy(this.gameObject);
        }
    }
}
