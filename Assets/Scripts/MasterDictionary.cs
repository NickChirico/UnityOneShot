using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MasterDictionary : MonoBehaviour
{
    public Dictionary<string, Weapon> WeaponDictionary;
    
    public Dictionary<string, Armor> ArmorDictionary;
    
    public Dictionary<string, Flask> FlaskDictionary;
    
    public Dictionary<string, Boots> BootsDictionary;

    public Dictionary<string, Sprite> SpriteDictionary;

    //public Dictionary<string, Weapon> ComplexWeaponDictionary;

    public Dictionary<string, StatusEffect> ComplexStatusDictionary;

    public List<string> simpleWeaponKeys, simpleArmorKeys, simpleFlaskKeys, simpleBootsKeys, spriteKeys;

    //public List<string> complexWeaponKeys;

    public List<Weapon> weapons;
    
    public List<Armor> armors;
    
    public List<Flask> flasks;
    
    public List<Boots> boots;

    public List<Sprite> sprites;

    public MasterDictionary(Dictionary<string, Weapon> tempWeaponDictionary, Dictionary<string, Armor> tempArmorDictionary, Dictionary<string, Flask> tempFlaskDictionary, Dictionary<string, Boots> tempBootsDictionary)
    {
        FlaskDictionary = tempFlaskDictionary;
        BootsDictionary = tempBootsDictionary;
        ArmorDictionary = tempArmorDictionary;
        WeaponDictionary = tempWeaponDictionary;
    }

    // Start is called before the first frame update
    void Awake()
    {
        //print("Master dictionary is awake");
        WeaponDictionary = new Dictionary<string, Weapon>();
        ArmorDictionary = new Dictionary<string, Armor>();
        BootsDictionary = new Dictionary<string, Boots>();
        FlaskDictionary = new Dictionary<string, Flask>();
        SpriteDictionary = new Dictionary<string, Sprite>();
        if (simpleWeaponKeys.Count == weapons.Count)
        {
            for (int i = 0; i < simpleWeaponKeys.Count; i++)
            {
                WeaponDictionary.Add(simpleWeaponKeys[i], weapons[i]);
            }
        }
        if (simpleArmorKeys.Count == armors.Count)
        {
            for (int i = 0; i < simpleArmorKeys.Count; i++)
            {
                ArmorDictionary.Add(simpleArmorKeys[i], armors[i]);
            }
        }
        if (simpleFlaskKeys.Count == flasks.Count)
        {
            for (int i = 0; i < simpleFlaskKeys.Count; i++)
            {
                FlaskDictionary.Add(simpleFlaskKeys[i], flasks[i]);
            }
        }
        if (simpleBootsKeys.Count == boots.Count)
        {
            for (int i = 0; i < simpleBootsKeys.Count; i++)
            {
                BootsDictionary.Add(simpleBootsKeys[i], boots[i]);
            }
        }

        if (spriteKeys.Count == sprites.Count)
        {
            for (int i = 0; i < simpleBootsKeys.Count; i++)
            {
                SpriteDictionary.Add(spriteKeys[i], sprites[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
