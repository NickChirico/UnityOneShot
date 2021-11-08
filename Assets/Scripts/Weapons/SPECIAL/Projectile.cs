using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float force_noArc;
    [Space(10)]
    public AnimationCurve arcCurve;

    [HideInInspector] public float timer = 0;
    [HideInInspector] public float duration;
    [HideInInspector] public float timeRatio;

    [HideInInspector] public Vector2 origin;
    [HideInInspector] public Vector2 dest;


    //public GameObject explosionEffect;

    private void Awake()
    {
        //rb = this.GetComponent<Rigidbody2D>();
    }

    public void SetVals(Vector2 or, Vector2 dir, float range, AnimationCurve c, float dur, float timeRat)
    {
        origin = or;
        dest = origin + (dir*range);

        if (timeRat < 0.5f)
            timeRatio = 0.5f;
        else
            timeRatio = timeRat;

        duration = dur * timeRatio;

        timer = 0;
        arcCurve = c;
    }
}
