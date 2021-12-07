using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrb : MonoBehaviour
{
    public int healAmount;
    public float speed;
    Player player;
    TrailRenderer trail;
    SpriteRenderer sp;
    public Color myColor;

    private void Awake()
    {
        trail = this.GetComponent<TrailRenderer>();
        sp = this.GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        player = FindObjectOfType<Player>();

        sp.color = myColor;
        trail.startColor = myColor;
        trail.endColor = myColor;
    }

    void Update()
    {
        // Fly to player
        float step = speed * Time.deltaTime;
        this.transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, step);
    }

    public void SetHealAmount(int heal)
    {
        healAmount = heal;
    }    



    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            coll.gameObject.GetComponent<Player>().currentHealth += healAmount;
            if (coll.gameObject.GetComponent<Player>().currentHealth >=
                coll.gameObject.GetComponent<Player>().MaxHealth)
            {
                coll.gameObject.GetComponent<Player>().currentHealth = coll.gameObject.GetComponent<Player>().MaxHealth;
            }
            coll.gameObject.GetComponent<Player>().GetUIManager().UpdateHealth(coll.gameObject.GetComponent<Player>().currentHealth, coll.gameObject.GetComponent<Player>().MaxHealth);
            //Debug.Log("Player HEAL for " + healAmount);
            //player.Augment(thisOrb);
            Destroy(this.gameObject);
        }

    }


}
