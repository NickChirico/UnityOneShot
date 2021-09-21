using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    public void DestroyMyParent()
    {
        Destroy(transform.parent.gameObject);
    }
}
