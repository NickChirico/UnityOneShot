using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CHEST : MonoBehaviour
{
    public bool isSeraphChest;
    public WeaponDrop contents;
    public SeraphDROP serContents;
    public TextMeshPro displayText;
    public Sprite openSprite;

    bool isOpen;

    PlayerInputActions inputActions;
    SpriteRenderer sp;

    void Start()
    {
        inputActions = MovementController.GetInputActions;
        sp = this.GetComponent<SpriteRenderer>();
        isOpen = false;
        displayText.enabled = false;
    }

    public void OpenChest()
    {
        sp.sprite = openSprite;
        Vector3 offset = new Vector2(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f));
        // drop contents
        if (isSeraphChest)
        {
            if (serContents != null)
            {
                isOpen = true;
                Instantiate(serContents, this.transform.position + offset, Quaternion.identity);
            }
        }
        else
        {
            if (contents != null)
            {
                isOpen = true;
                Instantiate(contents, this.transform.position + offset, Quaternion.identity);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!isOpen)
                displayText.enabled = true;

            if (inputActions.Player.Interact.ReadValue<float>() > 0)
            {
                if(!isOpen)
                    OpenChest();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            displayText.enabled = false;
        }
    }
}



