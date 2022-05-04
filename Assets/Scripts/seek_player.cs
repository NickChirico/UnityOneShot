using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seek_player : MonoBehaviour
{
    public float speed;
    public int damage;
    Vector2 targetDir;
    Rigidbody2D rb;

    Entity myEnemy;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        Vector3 playerPos = FindObjectOfType<Player>().transform.position;

        targetDir = (playerPos - this.transform.position).normalized;

        Debug.Log(playerPos + " : " + targetDir);

        rb.velocity = (targetDir * speed);
        Destroy(this, 2f);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            Player player = coll.gameObject.GetComponent<Player>();
            if (player.CanBeDamaged())
            {
                if (myEnemy != null)
                {
                    player.TakeDamage(myEnemy, damage, 0);
                }
            }
        }
    }

    public void SetMyEnemy(Entity e)
    {
        myEnemy = e;
    }
}
