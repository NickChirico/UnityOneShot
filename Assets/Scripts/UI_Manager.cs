using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager _uiControl;
    public static UI_Manager GetUIManager { get { return _uiControl; } }

    private PlayerInputActions playerInputActions;

    MovementController move;
    ShotController shot;
    AltShotController alt;
    MeleeController melee;
    PlayerStateManager SM;

    EquipmentManager Equipment;
    PauseController pause;

    [Header("Active UI")]
    //public Image manaBar;

    [Header("Equipment Panel")]
    public GameObject EquipmentPanel;
    [Space(10)]
    public Button SelectWeap_Melee;
    public Button SelectWeap_Ranged;
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
    [Space(10)]
    public TextMeshProUGUI controlDisplay;

    [Header("Pause Window")]
    public GameObject PausePanel;
    public Button ResumeButton;
    public Button RestartButton;
    public GameObject SettingsPanel;
    public Button SettingsButton1;
    public Button SettingsButton2;
    public Button SettingsButton_Back;

    [Header("In-Game HUD")]
    public GameObject weaponPanel;
    public TextMeshProUGUI currentWeaponLabel;
    public TextMeshProUGUI ammoLabel;
    public Image healthbar;

    [Header("UI Elements")]
    public Color EButton_NormalColor;
    public Color EButton_SelectedColor;
    public Color ammoLabelColor_Normal;
    public Color ammoLabelColor_Low;

    private Button currentWeapButton;
    private Button currentGunButton;
    private Button currentBulletButton;
    private Button currentAltButton;
    private Button[] weapButtons;
    private Button[] gunButtons;
    private Button[] bulletButtons;
    private Button[] altButtons;

    private void Awake()
    {
        _uiControl = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    private void Start()
    {
        move = MovementController.GetMoveController;
        shot = ShotController.GetShotControl;
        alt = AltShotController.GetAltControl;
        melee = MeleeController.GetMeleeControl;
        SM = FindObjectOfType<PlayerStateManager>();

        Equipment = EquipmentManager.GetEquipManager;
        pause = PauseController.GetPauseController;

        weapButtons = new Button[] { SelectWeap_Melee, SelectWeap_Ranged };
        gunButtons = new Button[] { SelectGun_Basic, SelectGun_Charge, SelectGun_Inverse };
        bulletButtons = new Button[] { SelectBullet_Basic, SelectBullet_Pierce, SelectBullet_Impact };
        altButtons = new Button[] { SelectAlt_Shotgun, SelectAlt_Burst, SelectAlt_Flamethrower };

        EquipmentPanel.SetActive(false);
        TogglePlayerControl(true);
        SetInitialEquipment();
    }

    bool pressedM = false;
    bool pressedP = false;
    private void Update()
    {
        //manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, alt.currentMana / alt.maxMana, Time.deltaTime * 8);

        // ~~~ Menu
        float menuPressed = playerInputActions.Player.Menu.ReadValue<float>();

        if (menuPressed > 0 && !PausePanel.activeSelf && !SettingsPanel.activeSelf) // open EQUIP if PAUSE && SETTINGS are NOT open
        {
            if (pressedM)
            {
                ToggleEquipmentPanel();
                pressedM = !pressedM;
            }
        }
        else
        {
            if (!pressedM)
                pressedM = !pressedM;
        }
        // ~~~ Pause
        float pausePressed = playerInputActions.Player.Pause.ReadValue<float>();
        if (pausePressed > 0)
        {
            if (pressedP)
            {
                TogglePausePanel();
                if (SettingsPanel.activeSelf)
                    SettingsPanel.SetActive(false);
                if (EquipmentPanel.activeSelf)
                    EquipmentPanel.SetActive(false);
                pressedP = !pressedP;
            }
        }
        else
        {
            if (!pressedP)
                pressedP = !pressedP;
        }
        // ~~~

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
        SetWeapon(Equipment.currentWeapon.ToString());
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
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(SelectWeap_Melee.gameObject);
            TogglePlayerControl(false);
            ButtonEffect_equipment();
        }
    }

    public void TogglePausePanel()
    {
        if (PausePanel.activeSelf)
        {
            pause.TogglePause(false);
            PausePanel.SetActive(false);
            TogglePlayerControl(true);
        }
        else
        {
            pause.TogglePause(true);
            PausePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(ResumeButton.gameObject);
            TogglePlayerControl(false);
        }
    }

    public void ToggleSettingsPanel()
    {
        if (SettingsPanel.activeSelf)
        {
            SettingsPanel.SetActive(false);
            PausePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(ResumeButton.gameObject);
        }
        else
        {
            PausePanel.SetActive(false);
            SettingsPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(SettingsButton1.gameObject);
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
        if (weapButtons != null)
        {
            foreach (Button b in weapButtons)
            {
                if (currentWeapButton == b)
                    b.targetGraphic.color = EButton_SelectedColor;
                else
                    b.targetGraphic.color = EButton_NormalColor;
            }
        }

        if (gunButtons != null)
        {
            foreach (Button b in gunButtons)
            {
                if (currentGunButton == b)
                    b.targetGraphic.color = EButton_SelectedColor;
                else
                    b.targetGraphic.color = EButton_NormalColor;
            }
        }

        if (bulletButtons != null)
        {
            foreach (Button b in bulletButtons)
            {
                if (currentBulletButton == b)
                    b.targetGraphic.color = EButton_SelectedColor;
                else
                    b.targetGraphic.color = EButton_NormalColor;
            }
        }

        if (altButtons != null)
        {
            foreach (Button b in altButtons)
            {
                if (currentAltButton == b)
                    b.targetGraphic.color = EButton_SelectedColor;
                else
                    b.targetGraphic.color = EButton_NormalColor;
            }
        }
    }

    private void ButtonEffect_equipment()
    {
        HighlightActiveEquipment();
        Equipment.UpdateEquipment();
    }


    public void SetWeapon(string name)
    {
        switch (name)
        {
            case "Stave":
                Equipment.SetWeapon(EquipmentManager.Weapon.Stave);
                currentWeapButton = SelectWeap_Melee;
                break;

            case "Rifle":
                Equipment.SetWeapon(EquipmentManager.Weapon.Rifle);
                currentWeapButton = SelectWeap_Ranged;
                break;

            default:
                break;
        }
        ButtonEffect_equipment();
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

    public void ToggleControl()
    {
        melee.usingMouse = !melee.usingMouse;
        shot.usingMouse = !shot.usingMouse;
        alt.usingMouse = !alt.usingMouse;
        ToggleControlDisplay(melee.usingMouse);
    }
    private void ToggleControlDisplay(bool on)
    {
        if (on)
            controlDisplay.text = "Selected: Mouse";
        else
            controlDisplay.text = "Selected: Controller";
    }
    public void RestartScene()
    {
        TogglePausePanel();
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    // ~~~~~~ In-Game HUD UI ~~~~~~~~~

    public void UpdateCurrentWeaponLabel(string name)
    {
        currentWeaponLabel.text = name;
        if (name == "Rifle")
        {
            ammoLabel.enabled = true;
        }
        else
            ammoLabel.enabled = false;
    }
    public void UpdateAmmo(int cur, int max)
    {
        ammoLabel.text = "" + cur + "/" + max;
        if (cur <= (float)max*0.2f)
            ammoLabel.color = ammoLabelColor_Low;
        else if(cur == max)
            ammoLabel.color = ammoLabelColor_Normal;
    }

    public void UpdateHealth(int curr, int max)
    {
        healthbar.fillAmount = ((float)curr / (float)max);
    }

    public void test()
    { }
}
