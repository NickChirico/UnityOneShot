using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDictionary : MonoBehaviour
{
    public Dictionary<int, Weapon> SimpleWeaponDictionary;

    public Dictionary<string, Weapon> ComplexWeaponDictionary;

    public Dictionary<string, StatusEffect> ComplexStatusDictionary;

    public List<int> simpleWeaponKeys;

    //public List<string> complexWeaponKeys;

    public List<Weapon> Weapons;
    // Start is called before the first frame update
    void Start()
    {
        if (simpleWeaponKeys.Count == Weapons.Count)
        {
            for (int i = 0; i < simpleWeaponKeys.Count; i++)
            {
                SimpleWeaponDictionary.Add(simpleWeaponKeys[i], Weapons[i]);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
