using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOrb : MonoBehaviour
{
    public float speed;
    ProtoPlayer player;
    TrailRenderer trail;
    SpriteRenderer sp;

    public enum OrbType { red, blue, green, yellow }
    OrbType thisOrb;

    private void Awake()
    {
        trail = this.GetComponent<TrailRenderer>();
        sp = this.GetComponent<SpriteRenderer>();

    }
    void Start()
    {
        player = FindObjectOfType<ProtoPlayer>();
    }

    void Update()
    {

        // Initial Delay

        // Fly to player
        float step = speed * Time.deltaTime;
        this.transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, step);
    }

    public void SetType(ProtoEnemy1.EnemyType type)
    {
        switch(type)
        {
            case ProtoEnemy1.EnemyType.Damage:
                sp.color = Color.red;
                trail.startColor = Color.red;
                trail.endColor = Color.red;
                thisOrb = OrbType.red;
                break;
            case ProtoEnemy1.EnemyType.Fire:
                sp.color = Color.yellow;
                trail.startColor = Color.yellow;
                trail.endColor = Color.yellow;
                thisOrb = OrbType.yellow;
                break;
            case ProtoEnemy1.EnemyType.Grow:
                sp.color = Color.green;
                trail.startColor = Color.green;
                trail.endColor = Color.green;
                thisOrb = OrbType.green;
                break;
            case ProtoEnemy1.EnemyType.Speed:
                sp.color = Color.blue;
                trail.startColor = Color.blue;
                trail.endColor = Color.blue;
                thisOrb = OrbType.blue;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.Augment(thisOrb);
            Destroy(this.gameObject);
        }
        
    }
}
