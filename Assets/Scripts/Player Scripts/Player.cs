using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;

public class Player : Entity
{
    public PlayerController myController;
    public TextMeshPro interactLabel;
    public SpriteRenderer sp;
    private bool _canInteract = false;
    private InteractableObject _interactTarget = null;
    public MasterDictionary myMasterDictionary;
    private PlayerStateManager SM;
    private UI_Manager ui;
    private SeraphController seraphControl;
    private AnimationController animControl;

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
        try { myMasterDictionary = GameObject.Find("Master Dictionary").GetComponent<MasterDictionary>(); }
        catch { myMasterDictionary = null; }
    }
    public override void Start()
    {
        seraphControl = SeraphController.GetSeraphController;
        animControl = AnimationController.GetAnimController;

        currentHealth = MaxHealth;
        ui.UpdateHealth(currentHealth, MaxHealth);
        base.Start();
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
        /*if (currentHealth <= 0)
        {
            SceneManager.LoadScene("DeathScreen");
        }*/
    }

    public override bool TakeDamage(int damageAmount, Vector2 damageSpot, float knockForce, float postureDamage)
    {
        bool b = base.TakeDamage(damageAmount, damageSpot, knockForce, postureDamage);
        seraphControl.ActivateArmorSeraphs(null, this.transform.position);
        SM.ChangeState(SM.Damaged);
        StartCoroutine(FlashRed());
        ui.UpdateHealth(currentHealth, MaxHealth);
        return b;
    }

    public override void Die()
    {
        //sp.color = Color.red;
        animControl.DieAnim();
        canBeDamaged = false;
        ui.TogglePlayerControl(true);
        MovementController.GetMoveController.SetMoveType(MovementController.Movement.Hold);
        StartCoroutine(DeathScreen());
    }

    IEnumerator DeathScreen()
    {
        yield return new WaitForSeconds(1f);
        ui.EnableDeathScreen();
    }

    public override bool IsPlayer()
    {
        return true;
    }

    public bool CanBeDamaged()
    { return canBeDamaged; }

    public bool IsNimble()
    { return nimble; }

    IEnumerator FlashRed()
    {
        animControl.TakeDamageAnim();
        //sp.color = Color.red;
        canBeDamaged = false;
        yield return new WaitForSeconds(invulnDuration + invulnBuffer);
        canBeDamaged = true;
        //sp.color = Color.white;
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
    
    public void ChangeChitinNum(int setAmount)
    {
        chitinNum = setAmount;
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

    public void ChangeBloodNum(int setAmount)
    {
        bloodNum = setAmount;
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
    
    public void ChangeBrainNum(int setAmount)
    {
        brainNum = setAmount;
        ui.brainAmount.text = brainNum.ToString();
    }

    public void RestoreFullHealth()
    {
        //currentHealth = maxHealth;
        //ui.UpdateHealth(currentHealth, maxHealth);
    }

    public Player LoadIntoLevel(PlayerLoader loader)
    {
        ui = UI_Manager.GetUIManager;
        seraphControl = SeraphController.GetSeraphController;

        SetAllSeraphs(loader.baggedSeraphim, loader.mainSeraphim, loader.altSeraphim, loader.armorSeraphim);
        loader.baggedSeraphim.Clear();
        loader.mainSeraphim.Clear();
        loader.altSeraphim.Clear();
        loader.armorSeraphim.Clear();

        SetAllEquipment(loader.mainWeaponCode, loader.altWeaponCode, loader.armorCode, loader.bootsCode, loader.flaskCode);
        UpdateStatsToMatchEquipment();
        currentHealth = loader.currentHealth <= MaxHealth ? loader.currentHealth : MaxHealth;
        ChangeChitinNum(loader.currentChitin);
        ChangeBrainNum(loader.currentBrains);
        ChangeBloodNum(loader.currentBlood);
        return this;
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
                interactLabel.gameObject.SetActive(true);
                _interactTarget = targetObject;
                break;
            case false:
                _canInteract = false;
                interactLabel.gameObject.SetActive(false);
                _interactTarget = null;
                break;
        }
    }

    public void InteractWith()
    {
        if (_interactTarget != null)
        {
            _interactTarget.GetComponent<InteractableObject>().Interact();
        }
    }

    public bool GetInteractStatus()
    {
        return _canInteract;
    }

    public UI_Manager GetUIManager()
    {
        return ui;
    }

    public void SetAllEquipment(string mainWeap, string altWeap, string armor, string boots, string flask)
    {
        myController.SelectWeapon(mainWeap, true);
        myController.SelectWeapon(altWeap, false);
        if (myMasterDictionary != null)
        {
            myMasterDictionary.ArmorDictionary.TryGetValue(armor, out _myArmor);
            myMasterDictionary.BootsDictionary.TryGetValue(armor, out _myBoots);
            myMasterDictionary.FlaskDictionary.TryGetValue(armor, out _myFlask);
        }
        myController.UpdateSeraphs();
    }

    public void UpdateStatsToMatchEquipment()
    {
        
    }

    public void SetAllSeraphs(List<int> inBag, List<int> onMain, List<int> onAlt, List<int> onArmor)
    {
        foreach (var temp in inBag)
        {
            seraphControl.SpawnSeraphUIToList(seraphControl.BagSeraphs, temp);
        }
        foreach (var temp in onMain)
        {
            seraphControl.SpawnSeraphUIToList(seraphControl.MainWeapSeraphs, temp);
        }
        foreach (var temp in onAlt)
        {
            seraphControl.SpawnSeraphUIToList(seraphControl.AltWeapSeraphs, temp);
        }
        foreach (var temp in onArmor)
        {
            seraphControl.SpawnSeraphUIToList(seraphControl.ArmorSeraphs, temp);

        }
        seraphControl.DownloadSeraphs();
    }

}
