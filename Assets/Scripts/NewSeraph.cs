using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSeraph : MonoBehaviour
{
    public bool chitinBoostA, chitinBoostB, chitinBoostC;
    public bool bloodBoostA, bloodBoostB, bloodBoostC;
    public SeraphBrainBoost brain1, brain2, brain3;
    public bool canEquipWeapon, canEquipArmor, canEquipFlask, canEquipBoots;
    public int level, chitinLv, bloodLv, brainLv;
    // Start is called before the first frame update
    void Start()
    {
        brain1 = null;
        brain2 = null;
        brain3 = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
