using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    private float timer;
    public float timeBtwnSpawns;

    public GameObject echo, echoFlipped;
    //public Sprite normalSprite, flipSprite;
    bool flipped;


    bool doActive;

    void Start()
    {

    }

    void Update()
    {
        if (doActive)
        {
            if (timer <= 0)
            {
                if (flipped)
                    Instantiate(echoFlipped, this.transform.position, Quaternion.identity);
                else
                    Instantiate(echo, this.transform.position, Quaternion.identity);

                timer = timeBtwnSpawns;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    public void Activate(bool b, bool isFlipX)
    {
        doActive = b;
        flipped = isFlipX;
    }
}
