using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwapPanel : MonoBehaviour
{
    public Weapon unequippedWeapon;

    public int unequippedCode, mainCode, altCode;

    public Image unequippedImage, mainImage, altImage;

    public TextMeshProUGUI unequippedName, unequippedDescription, mainName, mainDescription, altName, altDescription;

    public Sprite meleeSprite, rangedSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapWithMain()
    {
        //if any seraphim are equipped on main, unequip them and put them in bag, one by one. If at any point the bag is full, drop them on ground, or destroy them.
        (mainCode, unequippedCode) = (unequippedCode, mainCode);
        UpdateSwapPanel();
    }

    public void SwapWithAlt()
    {
        //if any seraphim are equipped on alt, unequip them and put them in bag, one by one. If at any point the bag is full, drop them on ground, or destroy them.
        (altCode, unequippedCode) = (unequippedCode, altCode);
        UpdateSwapPanel();
    }
    
    public void UpdateSwapPanel()
    {
        EditWeapon(unequippedImage, unequippedName, unequippedDescription, unequippedCode);
        EditWeapon(mainImage, mainName, mainDescription, mainCode);
        EditWeapon(altImage, altName, altDescription, altCode);
    }

    public void EditWeapon(Image imageToChange, TextMeshProUGUI nameToChange, TextMeshProUGUI descToChange, int weaponCode)
    {
        imageToChange.sprite = weaponCode >= 0 ? meleeSprite : rangedSprite;
        switch (weaponCode)
        {
            case -5: //pistol
                nameToChange.text = "Pistol";
                descToChange.text = "Standard ranged sidearm.";
                break;
            case -4: //rifle
                nameToChange.text = "Rifle";
                descToChange.text = "Long-range and powerful, but slow to fire.";
                break;
            case -3: //repeater
                nameToChange.text = "Repeater";
                descToChange.text = "Fast, low-power shots.";
                break;
            case -2: //blunderbuss
                nameToChange.text = "Blunderbuss";
                descToChange.text = "Lots of close-range power!";
                break;
            case -1: //mortar
                nameToChange.text = "Mortar";
                descToChange.text = "Hold the fire button to lob an explosive!";
                break;
            case 0: //knife
                nameToChange.text = "Knife";
                descToChange.text = "Short-ranged, fast melee attacks";
                break;
            case 1: //saber
                nameToChange.text = "Saber";
                descToChange.text = "3-hit melee combo!";
                break;
            case 2: //hammer
                nameToChange.text = "Hammer";
                descToChange.text = "Slow, heavy melee attacks.";
                break;
            case 3: //bat
                nameToChange.text = "Bat";
                descToChange.text = "Send enemies flying!";
                break;
        }
    }
}
