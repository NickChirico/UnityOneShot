using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;
    public float radius;
    public float explosionTime;
    public LayerMask hittableEntity;

    void Start()
    {
        StartCoroutine(DestoryAfterDelay());

        Collider2D[] hitEnemies;
        this.transform.localScale = Vector3.one * (radius*2);
        int damageToDeal = damage; // configure damage calulation();
        hitEnemies = Physics2D.OverlapCircleAll(this.transform.position, radius, hittableEntity);
        if (hitEnemies != null)
        {
            foreach (Collider2D hit in hitEnemies)
            {
                if (hit.CompareTag("Terrain"))
                { }
                else if (hit.CompareTag("Enemy"))
                {
                    ShootableEntity entity = hit.GetComponent<ShootableEntity>();
                    if (entity != null)
                    {
                        //ApplyAttack(entity, hit.transform.position, damageToDeal);
                        Vector2 hitPoint = new Vector2(entity.transform.position.x, entity.transform.position.y + 0.35f);
                        entity.TakeDamage(damageToDeal, hitPoint);
                    }
                }
            }
        }
    }

    IEnumerator DestoryAfterDelay()
    {
        yield return new WaitForSeconds(explosionTime);
        Destroy(this.gameObject);
    }

}
