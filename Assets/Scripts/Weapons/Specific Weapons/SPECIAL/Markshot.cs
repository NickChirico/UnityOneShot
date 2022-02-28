using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Markshot : Projectile
{
    public GameObject impactEffect;
    public int impactDamage;
    public float markDuration;


    private void OnCollisionEnter2D(Collision2D coll)
    {
        Instantiate(impactEffect, this.transform.position, Quaternion.identity);


        if (coll != null)
        {
            if (coll.gameObject.CompareTag("Terrain"))
            { }
            else if (coll.gameObject.CompareTag("Enemy"))
            {
                Entity entity = coll.gameObject.GetComponent<Entity>();
                if (entity != null)
                {
                    Vector2 hitPoint = new Vector2(entity.transform.position.x, entity.transform.position.y + 0.15f);
                    entity.TakeDamage(impactDamage, hitPoint, 0, 0);
                    entity.Mark(markDuration);
                }
            }
        }



        DestroyThis();
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
