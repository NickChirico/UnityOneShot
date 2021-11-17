using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    public float Delay = 1f;

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(Delay));
    }

    IEnumerator DestroyAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
