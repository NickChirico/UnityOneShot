using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeraphController : MonoBehaviour
{
    public static SeraphController _seraphControl;
    public static SeraphController GetSeraphController { get { return _seraphControl; } }

    public RectTransform parentPanel;
    public Transform seraphParent;

    MovementController move;
    ShotController shot;
    SpecialController spec;
    MeleeController melee;


    public List<AugmentSlot> mainWeapSlots;
    public List<AugmentSlot> altWeapSlots;
    public List<AugmentSlot> armorSlots;
    public List<AugmentSlot> bootsSlots;
    public List<AugmentSlot> bagSlots;

    [Space(15)]

    public int bagCapacity;
    public List<Seraph_UI> BagSeraphs;
    public List<Seraph_UI> MainWeapSeraphs;
    public List<Seraph_UI> AltWeapSeraphs;
    public List<Seraph_UI> ArmorSeraphs;
    public List<Seraph_UI> BootsSeraphs;
    public List<Seraph_UI> FlaskSeraphs;

    public Seraph_UI UI_Seraph_Prefab;

    [Space(10)]
    [Header("TOOLTIP")]
    public Image tooltipPanel;
    public Image tooltipSprite;
    public TextMeshProUGUI tooltipTitle;
    public TextMeshProUGUI tooltipSubTitle;
    public TextMeshProUGUI tooltipDescription;
       

    private void Awake()
    {
        _seraphControl = this;
    }
    void Start()
    {
        move = MovementController.GetMoveController;
        shot = ShotController.GetShotControl;
        melee = MeleeController.GetMeleeControl;
        spec = SpecialController.GetSpecialController;

        ClearTooltip();
    }

    
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            ActivateMainWeaponSeraphs(new Vector2(12, 2)); 
        }

        if (Input.GetKeyDown(KeyCode.J)) // spawn rupture seraph
        {
            SpawnSeraph(0);
        }
        if (Input.GetKeyDown(KeyCode.K)) // spawn siphon seraph
        {
            SpawnSeraph(1);
        }*/

    }

    public void SpawnSeraph(int i) // ****** ADD NEW SERAPH ******
    {
        if (BagSeraphs.Count < bagCapacity)
        {
            Seraph_UI newSeraph = Instantiate(UI_Seraph_Prefab, parentPanel);

            switch (i)
            {
                case 0:
                    newSeraph.SetGenome(Seraph_UI.Genome.Rupture);
                    break;
                case 1:
                    newSeraph.SetGenome(Seraph_UI.Genome.Siphon);
                    break;
                case 2:
                    newSeraph.SetGenome(Seraph_UI.Genome.Contaminate);
                    break;
                case 3:
                    newSeraph.SetGenome(Seraph_UI.Genome.Storm);
                    break;
                case 4:
                    newSeraph.SetGenome(Seraph_UI.Genome.Surge);
                    break;
                default:
                    newSeraph.SetGenome(Seraph_UI.Genome.None);
                    break;
            }
        }
        else
        {
            Debug.Log("BAG FULL");
        }
    }

    public void SpawnSeraph(string inputCode)
    {
        if (BagSeraphs.Count < bagCapacity)
        {
            Seraph_UI newSeraph = Instantiate(UI_Seraph_Prefab, parentPanel);
            switch (inputCode)
            {
                case "rupture":
                    newSeraph.SetGenome(Seraph_UI.Genome.Rupture);
                    break;
                case "siphon":
                    newSeraph.SetGenome(Seraph_UI.Genome.Siphon);
                    break;
                case "contaminate":
                    newSeraph.SetGenome(Seraph_UI.Genome.Contaminate);
                    break;
                default:
                    newSeraph.SetGenome(Seraph_UI.Genome.None);
                    break;
            }
        }
    }

    public void AddSeraphToBag(Seraph_UI S)
    {
        if(BagSeraphs.Count < bagCapacity)
            BagSeraphs.Add(S);
        else
        {
            //HERE: Activate the popup
        }

        foreach (AugmentSlot slot in bagSlots)
        {
            if (!slot.HasSeraph())
            {
                slot.mySeraph_ui = S;
                S.SetSlot(slot);
                S.GoToSpot();
                break;
            }
        }
    }

    public void UpdateSeraphLists()
    {
        MainWeapSeraphs = new List<Seraph_UI>();
        AltWeapSeraphs = new List<Seraph_UI>();
        ArmorSeraphs = new List<Seraph_UI>();
        BootsSeraphs = new List<Seraph_UI>();

        BagSeraphs = new List<Seraph_UI>();

        foreach (AugmentSlot slot in mainWeapSlots)
        {
            if (slot.HasSeraph())
                MainWeapSeraphs.Add(slot.mySeraph_ui);
        }
        foreach (AugmentSlot slot in altWeapSlots)
        {
            if (slot.HasSeraph())
                AltWeapSeraphs.Add(slot.mySeraph_ui);
        }
        foreach (AugmentSlot slot in armorSlots)
        {
            if (slot.HasSeraph())
                ArmorSeraphs.Add(slot.mySeraph_ui);
        }
        foreach (AugmentSlot slot in bootsSlots)
        {
            if (slot.HasSeraph())
                BootsSeraphs.Add(slot.mySeraph_ui);
        }

        foreach (AugmentSlot slot in bagSlots)
        {
            if (slot.HasSeraph())
            {
                BagSeraphs.Add(slot.mySeraph_ui);
            }
        }
    }

    public void AddToSeraphList(Seraph_UI s, AugmentSlot.EquipmentType e)
    {
        switch (e)
        {
            case AugmentSlot.EquipmentType.mainWeapon:
                MainWeapSeraphs.Add(s);
                break;
            case AugmentSlot.EquipmentType.altWeapon:
                AltWeapSeraphs.Add(s);
                break;
            case AugmentSlot.EquipmentType.armor:
                ArmorSeraphs.Add(s);
                break;
            case AugmentSlot.EquipmentType.boots:
                BootsSeraphs.Add(s);
                break;
            case AugmentSlot.EquipmentType.bag:
                BagSeraphs.Add(s);
                break;
            default:
                break;
        }
    }
    public void RemoveFromSeraphList(Seraph_UI s, AugmentSlot.EquipmentType e)
    {
        switch (e)
        {
            case AugmentSlot.EquipmentType.mainWeapon:
                MainWeapSeraphs.Remove(s);
                break;
            case AugmentSlot.EquipmentType.altWeapon:
                AltWeapSeraphs.Remove(s);
                break;
            case AugmentSlot.EquipmentType.armor:
                ArmorSeraphs.Remove(s);
                break;
            case AugmentSlot.EquipmentType.boots:
                BootsSeraphs.Remove(s);
                break;
            case AugmentSlot.EquipmentType.bag:
                BagSeraphs.Remove(s);
                break;
            default:
                break;
        }
    }

    public void GoToSpots()
    {
        Seraph_UI[] AllSeraphs = FindObjectsOfType<Seraph_UI>();
        foreach (Seraph_UI s in AllSeraphs)
        {
            s.GoToSpot();
        }
    }

    // Called from Seraph_UI.cs prefabs when you hover over it. 
    public void LoadTooltip(Sprite sprite, Color color, string title, string subtitle, string description, string blood)
    {
        // Load all stuff from Seraph into the UI
        tooltipSprite.sprite = sprite;
        tooltipPanel.color = color;
        tooltipTitle.text = title;
        tooltipSubTitle.text = subtitle;
        tooltipDescription.text = description;

        switch (blood)
        {
            case "A": // A
                tooltipTitle.text += "<color=#330000>  'A'</color>";
                break;
            case "B": // B
                tooltipTitle.text += "<color=#000b33>  'B'</color>";
                break;
            case "AB": // AB
                tooltipTitle.text += "<color=#330027>  'AB'</color>";
                break;
            case "O": // O
                tooltipTitle.text += "<color=#053300>  'O'</color>";
                break;
        }

        if(!tooltipPanel.gameObject.activeSelf)
            tooltipPanel.gameObject.SetActive(true);
    }

    public void ClearTooltip()
    {
        if (tooltipPanel.gameObject.activeSelf)
            tooltipPanel.gameObject.SetActive(false);
    }



    #region Activation Functions 
    public void ActivateMainWeaponSeraphs(Entity entity, Vector2 pos)
    {
        foreach (Seraph_UI S in MainWeapSeraphs)
        {
            S.mySeraph.StartEffect(entity, pos);
        }
    }
    public void ActivateSpecWeaponSeraphs(Entity entity, Vector2 pos)
    {
        foreach (Seraph_UI S in AltWeapSeraphs)
        {
            S.mySeraph.StartEffect(entity, pos);
        }
    }
    public void ActivateArmorSeraphs(Entity entity, Vector2 pos)
    {
        foreach (Seraph_UI S in ArmorSeraphs)
        {
            S.mySeraph.StartEffect(entity, pos);
        }
    }
    public void ActivateBootsSeraphs(Entity entity, Vector2 pos)
    {
        foreach (Seraph_UI S in BootsSeraphs)
        {
            S.mySeraph.StartEffect(entity, pos);
        }
    }
    #endregion

}
