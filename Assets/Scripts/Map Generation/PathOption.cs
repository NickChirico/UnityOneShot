using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathOption : MonoBehaviour
{
    public bool selected;
    public Image selectedImage;
    public int pathNum;
    public string pathCode;

    public Text pathNameText, pathInfoText;

    public Image pathSymbol;

    public Sprite ruptureSymbol,
        siphonSymbol,
        contaminateSymbol,
        goldSymbol,
        shopSymbol,
        healingSymbol,
        weaponSymbol,
        mysterySymbol;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("Path Manager").GetComponent<PathManager>().allOptions[pathNum] = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSelection(bool nowSelected)
    {
        selected = nowSelected;
        selectedImage.enabled = nowSelected;
    }

    public void SetPath(string tempCode)
    {
        pathCode = tempCode;
        switch (tempCode)
        {
            case "rupture":
                pathNameText.text = "Explosive Path";
                pathInfoText.text = "Guaranteed Rupture Seraphs";
                pathSymbol.sprite = ruptureSymbol;
                break;
            case "siphon":
                pathNameText.text = "Parasitic Path";
                pathInfoText.text = "Guaranteed Siphon Seraphs";
                pathSymbol.sprite = siphonSymbol;
                break;
            case "contaminate":
                pathNameText.text = "Infectious Path";
                pathInfoText.text = "Guaranteed Contaminate Seraphs";
                pathSymbol.sprite = contaminateSymbol;
                break;
            case "gold":
                pathNameText.text = "Wealthy Path";
                pathInfoText.text = "Guaranteed Gold";
                pathSymbol.sprite = goldSymbol;
                break;
            case "shop":
                pathNameText.text = "Merchant Camp";
                pathInfoText.text = "Spend Gold to Grow Stronger";
                pathSymbol.sprite = shopSymbol;
                break;
            case "healing":
                pathNameText.text = "Healing Spring";
                pathInfoText.text = "Fully Heal";
                pathSymbol.sprite = healingSymbol;
                break;
            case "weapon":
                pathNameText.text = "Violent Path";
                pathInfoText.text = "Guaranteed Weapon";
                pathSymbol.sprite = weaponSymbol;
                break;
            case "mystery":
                pathNameText.text = "Mysterious Path";
                pathInfoText.text = "It could be Anything!";
                pathSymbol.sprite = mysterySymbol;
                break;
        }
    }

}
