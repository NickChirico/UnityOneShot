using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeraphDROP : MonoBehaviour
{
    SeraphController seraphControl;
    public enum SeraphType { Rupture, Siphon, Contam, Storm, Surge }
    public SeraphType upgradeType;
    //public int value; // 0 == rup, 1 == siph, 2 == contam, 3 == storm, 4 == surge;

    bool canPickUp = true;
    bool moving = false;
    float speed = 4;
    GameObject target;
    Vector2 targetPos;
    void Start()
    {
        seraphControl = SeraphController.GetSeraphController;

        target = FindObjectOfType<bag_place>().gameObject;
    }

    void Update()
    {
        if (moving)
        {
            targetPos = target.transform.position;

            float step = speed * Time.deltaTime;
            speed += 0.05f;

            // move sprite towards the target location
            transform.position = Vector2.MoveTowards(transform.position, targetPos, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().sfxSource.PlayOneShot(GameObject.Find("AudioManager").GetComponent<AudioManager>().seraphPickupSound);
            if (canPickUp)
                seraphControl.SpawnSeraph(((int)upgradeType));
            canPickUp = false;
            moving = true;
        }

        if (collision.gameObject.CompareTag("bag"))
        {
            Destroy(this.gameObject);
        }
    }
}
