using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Entity
{
    public PlayerController myController;
    public TextMeshProUGUI interactLabel;
    public SpriteRenderer sp;
    private bool _canInteract = false;
    private InteractableObject _interactTarget = null;

    private PlayerStateManager SM;
    private UI_Manager ui;
    private SeraphController seraphControl;

    [Header("PLAYER")]
    public float invulnDuration;
    public float invulnBuffer;
    public float knockedForce;

    private bool canBeDamaged = true;
    private Vector2 hitPoint;

    public List<Seraph> bagSeraphim, mainWeaponSeraphim, altWeaponSeraphim, armorSeraphim, bootsSeraphim, flaskSeraphim;

    public int chitinNum, bloodNum, brainNum;

    private bool nimble = false;
    //private Weapon _myMainWeapon, _myAltWeapon;
    private Armor _myArmor;
    private Boots _myBoots;
    private Flask _myFlask;

    private void Awake()
    {
        SM = this.GetComponent<PlayerStateManager>();
    }
    public override void Start()
    {
        base.Start();
        ui = UI_Manager.GetUIManager;
        seraphControl = SeraphController.GetSeraphController;
    }

    //void Update()
    //{
    //}

    public override void TakeDamage(Entity attacker, int damageAmount, float posture)
    {
        base.TakeDamage(attacker, damageAmount, posture);
        seraphControl.ActivateArmorSeraphs(attacker, this.transform.position);
        ui.UpdateHealth(currentHealth, MaxHealth);
        SM.ChangeState(SM.Damaged);
        StartCoroutine(FlashRed());
    }

    public override bool TakeDamage(int damageAmount, Vector2 damageSpot, float knockForce, float postureDamage)
    {
        SM.ChangeState(SM.Damaged);
        StartCoroutine(FlashRed());
        return base.TakeDamage(damageAmount, damageSpot, knockForce, postureDamage);
    }

    public bool CanBeDamaged()
    { return canBeDamaged; }

    public bool IsNimble()
    { return nimble; }

    IEnumerator FlashRed()
    {
        sp.color = Color.red;
        canBeDamaged = false;
        yield return new WaitForSeconds(invulnDuration + invulnBuffer);
        canBeDamaged = true;
        sp.color = Color.white;
    }

    public void Nimble(bool b)
    {
        nimble = b;
        Physics2D.IgnoreLayerCollision(3, 6, b);// 3 = player, 6 = hittableEntity
    }

    public void ChangeChitinNum(bool isIncrease, int changeAmount)
    {
        if (isIncrease)
        {
            chitinNum += changeAmount;
        }
        else
        {
            chitinNum -= changeAmount;
        }
        if (ui.chitinAmount != null)
            ui.chitinAmount.text = chitinNum.ToString();
    }
    
    public void ChangeBloodNum(bool isIncrease, int changeAmount)
    {
        if (isIncrease)
        {
            bloodNum += changeAmount;
        }
        else
        {
            bloodNum -= changeAmount;
        }
        if (ui.bloodAmount != null)
            ui.bloodAmount.text = bloodNum.ToString();
    }
    
    public void ChangeBrainNum(bool isIncrease, int changeAmount)
    {
        if (isIncrease)
        {
            brainNum += changeAmount;
        }
        else
        {
            brainNum -= changeAmount;
        }
        if (ui.brainAmount != null)
            ui.brainAmount.text = brainNum.ToString();
    }

    public void RestoreFullHealth()
    {
        //currentHealth = maxHealth;
        //ui.UpdateHealth(currentHealth, maxHealth);
    }

    public void LoadIntoLevel(PlayerLoader loader)
    {
        SetAllEquipment(loader.mainWeapon, loader.altWeapon, loader.armor, loader.boots, loader.flask);
        UpdateStatsToMatchEquipment();
        currentHealth = loader.currentHealth <= MaxHealth ? loader.currentHealth : MaxHealth;
        ChangeChitinNum(true, loader.currentChitin);
        ChangeBrainNum(true, loader.currentBrains);
        ChangeBloodNum(true, loader.currentBlood);
    }

    public void DrinkFlask()
    {
        
    }

    public void SetInteract(bool canInteract, InteractableObject targetObject)
    {
        switch (canInteract)
        {
            case true:
                _canInteract = true;
                interactLabel.enabled = true;
                _interactTarget = targetObject;
                break;
            case false:
                _canInteract = false;
                interactLabel.enabled = false;
                _interactTarget = null;
                break;
        }
    }

    public UI_Manager GetUIManager()
    {
        return ui;
    }

    public void SetAllEquipment(Weapon mainWeap, Weapon altWeap, Armor armor, Boots boots, Flask flask)
    {
        myController.SelectWeapon(mainWeap, true);
        myController.SelectWeapon(altWeap, false);
        _myArmor = armor;
        _myBoots = boots;
        _myFlask = flask;
    }

    public void UpdateStatsToMatchEquipment()
    {
        
    }
}
