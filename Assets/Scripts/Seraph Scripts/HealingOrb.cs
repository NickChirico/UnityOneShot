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
            Debug.Log("Player HEAL for " + healAmount);
            //player.Augment(thisOrb);
            Destroy(this.gameObject);
        }

    }


}
