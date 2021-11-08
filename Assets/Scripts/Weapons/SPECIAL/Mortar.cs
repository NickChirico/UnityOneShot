using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : Projectile
{
    public Vector2 offset;
    public GameObject preIndicator;
    public GameObject explosion;

    void Start()
    {
        //Instantiate(preIndicator, dest, Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            float lerpRatio = timer / duration;
            Vector2 positionOffset = (arcCurve.Evaluate(lerpRatio) / 10) * (offset * timeRatio);

            this.transform.position = Vector2.Lerp(origin, dest, lerpRatio) + positionOffset;

        }
        else
        {
            Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
