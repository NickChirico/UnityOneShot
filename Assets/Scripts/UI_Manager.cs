using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    MovementController move;
    ShotController shot;
    AltShotController alt;
    PlayerStateManager SM;

    EquipmentManager Equipment;

    [Header("Active UI")]
    public Image manaBar;

    [Header("Equipment Panel")]
    public GameObject EquipmentPanel;
    [Space(10)]
    public Button SelectGun_Basic;
    public Button SelectGun_Charge;
    public Button SelectGun_Inverse;
    [Space(10)]
    public Button SelectBullet_Basic;
    public Button SelectBullet_Pierce;
    public Button SelectBullet_Impact;
    [Space(10)]
    public Button SelectAlt_Shotgun;
    public Button SelectAlt_Burst;
    public Button SelectAlt_Flamethrower;

    [Header("UI Elements")]
    public Color NormalColor;
    public Color SelectedColor;

    private Button currentGunButton;
    private Button currentBulletButton;
    private Button currentAltButton;
    private Button[] gunButtons;
    private Button[] bulletButtons;
    private Button[] altButtons;

    private void Start()
    {
        move = MovementController.GetMoveController;
        shot = ShotController.GetShotControl;
        alt = AltShotController.GetAltControl;
        SM = FindObjectOfType<PlayerStateManager>();

        Equipment = EquipmentManager.GetEquipManager;

        gunButtons = new Button[] { SelectGun_Basic, SelectGun_Charge, SelectGun_Inverse };
        bulletButtons = new Button[] { SelectBullet_Basic, SelectBullet_Pierce, SelectBullet_Impact };
        altButtons = new Button[] { SelectAlt_Shotgun, SelectAlt_Burst, SelectAlt_Flamethrower };

        EquipmentPanel.SetActive(false);
        TogglePlayerControl(true);
        SetInitialEquipment();
    }

    private void Update()
    {
        manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, alt.currentMana / alt.maxMana, Time.deltaTime * 8);
        //
        if (Input.GetKeyDown(KeyCode.I))
            ToggleEquipmentPanel();

        if (Input.GetKeyDown(KeyCode.Escape))
            Escape();

        //
        if (EquipmentPanel.activeSelf)
        {
            HighlightActiveEquipment();
        }
    }

    private void Escape()
    {
        if (EquipmentPanel.activeSelf)
        {
            ToggleEquipmentPanel();
        }
    }

    private void SetInitialEquipment()
    {
        SetGun(Equipment.currentGun.ToString());
        SetBullet(Equipment.currentBullet.ToString());
        SetAltFire(Equipment.currentAltFire.ToString());
    }

    private void ToggleEquipmentPanel()
    {
        if (EquipmentPanel.activeSelf)
        {
            EquipmentPanel.SetActive(false);
            TogglePlayerControl(true);
        }
        else
        {
            EquipmentPanel.SetActive(true);
            TogglePlayerControl(false);
            ButtonEffect_equipment();
        }
    }

    private void TogglePlayerControl(bool check)
    {
        //move.enabled = check;
        //shot.enabled = check;
        //alt.enabled = check;
        SM.ActivePlayer(check);
    }

    private void HighlightActiveEquipment()
    {
        if (gunButtons != null)
        {
            foreach (Button b in gunButtons)
            {
                if (currentGunButton == b)
                    b.targetGraphic.color = SelectedColor;
                else
                    b.targetGraphic.color = NormalColor;
            }
        }

        if (bulletButtons != null)
        {
            foreach (Button b in bulletButtons)
            {
                if (currentBulletButton == b)
                    b.targetGraphic.color = SelectedColor;
                else
                    b.targetGraphic.color = NormalColor;
            }
        }

        if (altButtons != null)
        {
            foreach (Button b in altButtons)
            {
                if (currentAltButton == b)
                    b.targetGraphic.color = SelectedColor;
                else
                    b.targetGraphic.color = NormalColor;
            }
        }
    }

    private void ButtonEffect_equipment()
    {
        HighlightActiveEquipment();
        Equipment.UpdateEquipment();
    }

    public void SetGun(string gun)
    {
        switch (gun)
        {
            case "Basic":
                Equipment.SetGun(EquipmentManager.GunType.Basic);
                currentGunButton = SelectGun_Basic;
                break;

            case "Charge":
                Equipment.SetGun(EquipmentManager.GunType.Charge);
                currentGunButton = SelectGun_Charge;
                break;

            case "Inverse":
                Equipment.SetGun(EquipmentManager.GunType.Inverse);
                currentGunButton = SelectGun_Inverse;
                break;
        }
        ButtonEffect_equipment();
    }

    public void SetBullet(string bullet)
    {
        switch (bullet)
        {
            case "Basic":
                Equipment.SetBullet(EquipmentManager.BulletType.Basic);
                currentBulletButton = SelectBullet_Basic;
                break;

            case "Pierce":
                Equipment.SetBullet(EquipmentManager.BulletType.Pierce);
                currentBulletButton = SelectBullet_Pierce;
                break;

            case "Impact":
                Equipment.SetBullet(EquipmentManager.BulletType.Impact);
                currentBulletButton = SelectBullet_Impact;
                break;
        }
        ButtonEffect_equipment();
    }

    public void SetAltFire(string alt)
    {
        switch (alt)
        {
            case "None":
                Equipment.SetAlternate(EquipmentManager.AlternateFire.None);
                currentAltButton = null;
                break;

            case "Shotgun":
                Equipment.SetAlternate(EquipmentManager.AlternateFire.Shotgun);
                currentAltButton = SelectAlt_Shotgun;
                break;

            case "Burst":
                Equipment.SetAlternate(EquipmentManager.AlternateFire.Burst);
                currentAltButton = SelectAlt_Burst;
                break;

            case "Flamethrower":
                Equipment.SetAlternate(EquipmentManager.AlternateFire.Flamethrower);
                currentAltButton = SelectAlt_Flamethrower;
                break;
        }
        ButtonEffect_equipment();
    }
}
