using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUpgrade : MonoBehaviour
{
    OneShot PlayerShot;
    PlayerMovement PlayerMove;

    [Header("Variables")]
    public int damageIncrement;
    public float atkspeedIncrement;

    [Header("Var Caps")]
    public int MinDamage;
    public int MaxDamage;
    public float MinAtkSpeed, MaxAtkSpeed;


    [Header("UI")]
    public GameObject UpgradePanel;
    public TextMeshProUGUI DamageText, FireRateText, ProximityText;


    void Start()
    {
        PlayerShot = this.GetComponent<OneShot>();
        PlayerMove = this.GetComponent<PlayerMovement>();
        UpdateStatDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }

    void ToggleUpgradeMenu()
    {
        if (UpgradePanel.activeSelf)
        {
            UpgradePanel.SetActive(false);
        }
        else
        {
            UpgradePanel.SetActive(true);
        }

    }

    public void UpdateStatDisplay()
    {
        DamageText.text = "" + PlayerShot.damage;
        FireRateText.text = "" + (PlayerShot.attackSpeed) + "s";
        ProximityText.text = PlayerShot.ProximityDamageActive.ToString();
    }

    public void ChangeDamage(int affector)
    {

        PlayerShot.damage += damageIncrement * affector;


        if (PlayerShot.damage > MaxDamage)
            PlayerShot.damage = MaxDamage;
        else if (PlayerShot.damage < MinDamage)
            PlayerShot.damage = MinDamage;

        UpdateStatDisplay();
    }

    public void ChangeAttackSpeed(int affector)
    {

        PlayerShot.attackSpeed -= atkspeedIncrement * affector;


        if (PlayerShot.attackSpeed > MaxAtkSpeed)
            PlayerShot.attackSpeed = MaxAtkSpeed;
        else if (PlayerShot.attackSpeed < MinAtkSpeed)
            PlayerShot.attackSpeed = MinAtkSpeed;

        PlayerShot.attackSpeed = Mathf.Round(PlayerShot.attackSpeed * 100.0f) * 0.01f;

        UpdateStatDisplay();
    }

    public void ToggleProximityDamage()
    {
        if (PlayerShot.ProximityDamageActive)
        {
            PlayerShot.ProximityDamageActive = false;
        }
        else
        {
            PlayerShot.ProximityDamageActive = true;
        }

        UpdateStatDisplay();
    }
    public void ToggleChargeShot()
    {

    }
}
