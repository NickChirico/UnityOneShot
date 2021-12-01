using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;
    public float postureDamage;
    public float radius;
    public float knockForce;
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
                    Entity entity = hit.GetComponent<Entity>();
                    if (entity != null)
                    {
                        //ApplyAttack(entity, hit.transform.position, damageToDeal);
                        Vector2 hitPoint = new Vector2(entity.transform.position.x, entity.transform.position.y + 0.35f);
                        entity.TakeDamage(damageToDeal, hitPoint, knockForce, postureDamage);
                    }
                }
            }
        }
    }

    public void SetValues(int dmg, float radi, float knock, float duration)
    {
        damage = dmg;
        radius = radi;
        knockForce = knock;
        explosionTime = duration;
    }

    IEnumerator DestoryAfterDelay()
    {
        yield return new WaitForSeconds(explosionTime);
        Destroy(this.gameObject);
    }

}
