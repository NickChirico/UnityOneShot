using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MasterDictionary : MonoBehaviour
{
    public readonly Dictionary<int, Weapon> SimpleWeaponDictionary;
    
    public readonly Dictionary<int, Armor> SimpleArmorDictionary;
    
    public readonly Dictionary<int, Flask> SimpleFlaskDictionary;
    
    public readonly Dictionary<int, Boots> SimpleBootsDictionary;

    public Dictionary<string, Weapon> ComplexWeaponDictionary;

    public Dictionary<string, StatusEffect> ComplexStatusDictionary;

    public List<int> simpleWeaponKeys, simpleArmorKeys, simpleFlaskKeys, simpleBootsKeys;

    //public List<string> complexWeaponKeys;

    public List<Weapon> weapons;
    
    public List<Armor> armors;
    
    public List<Flask> flasks;
    
    public List<Boots> boots;

    public MasterDictionary(Dictionary<int, Weapon> simpleWeaponDictionary, Dictionary<int, Armor> simpleArmorDictionary, Dictionary<int, Flask> simpleFlaskDictionary, Dictionary<int, Boots> simpleBootsDictionary)
    {
        SimpleArmorDictionary = simpleArmorDictionary;
        SimpleFlaskDictionary = simpleFlaskDictionary;
        SimpleBootsDictionary = simpleBootsDictionary;
        SimpleWeaponDictionary = simpleWeaponDictionary;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (simpleWeaponKeys.Count == weapons.Count)
        {
            for (int i = 0; i < simpleWeaponKeys.Count; i++)
            {
                SimpleWeaponDictionary.Add(simpleWeaponKeys[i], weapons[i]);
            }
        }
        if (simpleArmorKeys.Count == armors.Count)
        {
            for (int i = 0; i < simpleArmorKeys.Count; i++)
            {
                SimpleArmorDictionary.Add(simpleArmorKeys[i], armors[i]);
            }
        }
        if (simpleFlaskKeys.Count == flasks.Count)
        {
            for (int i = 0; i < simpleFlaskKeys.Count; i++)
            {
                SimpleFlaskDictionary.Add(simpleFlaskKeys[i], flasks[i]);
            }
        }
        if (simpleBootsKeys.Count == boots.Count)
        {
            for (int i = 0; i < simpleBootsKeys.Count; i++)
            {
                SimpleBootsDictionary.Add(simpleBootsKeys[i], boots[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
