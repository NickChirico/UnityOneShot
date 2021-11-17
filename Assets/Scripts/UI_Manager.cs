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

    [Header("Weapons Panel")]
    public TextMeshProUGUI currentWeaponTMP;

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
    public GameObject ammoSubPanel;
    public Image healthbar;

    [Header("UI Elements")]
    public Button firstSelected;
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

        EquipmentPanel.SetActive(true);
        TogglePlayerControl(false);
        ToggleControlDisplay(shot.usingMouse);
        SwitchCurrentMenu(1); // 1:Weap , 2:Serap , 3:Options
        SetInitialEquipment();

        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }

    bool pressedM = false;
    bool pressedP = false;
    private void Update()
    {
        //manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, alt.currentMana / alt.maxMana, Time.deltaTime * 8);

        // ~~~ Menu (*select)
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
        // ~~~ Pause (*start)
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

        // UPDATE:: Tool Tips
        if(weaponPanel.activeSelf)
            ManageTooltips();
    }

    private void SetInitialEquipment()
    {
        SetWeapon(Equipment.currentWeapon.ToString());
        SetGun(Equipment.currentGun.ToString());
        SetBullet(Equipment.currentBullet.ToString());
        SetAltFire(Equipment.currentAltFire.ToString());
    }

    public void ToggleEquipmentPanel()
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
            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
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

    public void ToggleControlType()
    {
        melee.usingMouse = !melee.usingMouse;
        shot.usingMouse = !shot.usingMouse;
        alt.usingMouse = !alt.usingMouse;
        ToggleControlDisplay(melee.usingMouse);
    }
    private void ToggleControlDisplay(bool on)
    {
        if (on)
            controlDisplay.text = "Selected: MOUSE";
        else
            controlDisplay.text = "Selected: CONTROLLER";
    }
    public void RestartScene()
    {
        TogglePausePanel();
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    [Header("Sub-Panels")]
    public GameObject OptionsPanel;
    public GameObject WeaponsPanel;
    public GameObject SeraphimPanel;
    public void SwitchCurrentMenu(int num)
    {
        switch(num)
        {
            case 1: // go to Weapons panel
                WeaponsPanel.SetActive(true);
                SeraphimPanel.SetActive(false);
                OptionsPanel.SetActive(false);
                break;
            case 2: // go to Seraphim panel
                WeaponsPanel.SetActive(false);
                SeraphimPanel.SetActive(true);
                OptionsPanel.SetActive(false);
                break;
            case 3: // go to Options panel
                WeaponsPanel.SetActive(false);
                SeraphimPanel.SetActive(false);
                OptionsPanel.SetActive(true);
                break;
        }
    }

    public void UpdateCurrentWeaponPanelTMP(string weap)
    {
        currentWeaponTMP.text = weap;
    }

    // ~~~~~~ In-Game HUD UI ~~~~~~~~~

    public void UpdateCurrentWeaponLabel(string name, bool melee)
    {
        currentWeaponLabel.text = name;
        ammoSubPanel.SetActive(!melee);
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

    // ~~~~~~ Menu Tooltips ~~~~~~~~~
    [Header("Menu: Tool Tips")]
    public Button[] weaponButtons;
    public RectTransform background;
    public TextMeshProUGUI weaponTypeTMP;
    public TextMeshProUGUI descriptionTMP;
    public TextMeshProUGUI specialTMP;
    public float tooltip_Width;
    public float textPadding;
    public Vector2 offset;
    bool showingTooltip = false;

    GameObject currentSelected;
    public void ManageTooltips()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;
        foreach (Button b in weaponButtons)
        {
            if (b.gameObject == currentSelected)
            {
                UI_Tooltip i = b.GetComponent<UI_Tooltip>();

                background.gameObject.SetActive(true);
                int onRight = i.onRight;
                float newX = b.transform.position.x + (offset.x * onRight);
                float newY = b.transform.position.y - offset.y;
                Vector2 pos = new Vector2(newX, newY);
                background.position = pos;
                background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tooltip_Width);
                //textBox.text = i.description;

                weaponTypeTMP.text = i.weaponType;
                descriptionTMP.text = i.description;
                specialTMP.text = "Special:\n" + i.specialName;
                showingTooltip = true;
            }
        }


        if(showingTooltip)
        {
            bool doShow = false;
            foreach (Button b in weaponButtons)
            {
                if (b.gameObject == currentSelected)
                {
                    doShow = true;
                }
            }
            if (!doShow)
            {
                background.gameObject.SetActive(false);
                showingTooltip = false;
            }
        }
    }

}
