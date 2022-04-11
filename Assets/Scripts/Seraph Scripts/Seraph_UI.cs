using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Seraph_UI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    SeraphController seraphControl;
    public Image seraphIcon;
    public Image backdrop;
    public Sprite ruptureSprite, contaminateSprite, siphonSprite;

    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public AugmentSlot mySlot;
    private RectTransform myRect;

    //public GameObject mySeraphPrefab;
    public Seraph mySeraph;

    public AugmentSlot Slot { get => mySlot; set => mySlot = value; }


    public enum Genome { None, Rupture, Siphon, Contaminate, Surge, Hex, Calcify, Storm }
    [Space(10)]
    public Genome seraphType;

    public GameObject[] SeraphPrefabs;
    public Color[] seraphColors;

    [Header("Tooltip")]
    public Image Tooltip_box;
    public TextMeshProUGUI tooltip_title;
    public TextMeshProUGUI tooltip_description;

    private void Awake()
    {

        seraphControl = SeraphController.GetSeraphController;
        myRect = this.GetComponent<RectTransform>();
        canvas = FindObjectOfType<MainCanvas>().GetCanvas();
        canvasGroup = this.GetComponent<CanvasGroup>();

        //mySeraph = Instantiate(mySeraphPrefab, seraphControl.seraphParent).GetComponent<Seraph>();

        Tooltip_box.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        GoToSpot();
    }

    public void SetGenome(Genome g)
    {
        seraphType = g;
    }

    private void Start()
    {
        CreateSeraphObject();

        if (mySlot == null)
        {
            seraphControl.AddSeraphToBag(this);
        }
    }

    void CreateSeraphObject()
    {
        switch (seraphType)
        {
            case Genome.Rupture:
                backdrop.color = seraphColors[0];
                seraphIcon.sprite = ruptureSprite;
                mySeraph = Instantiate(SeraphPrefabs[0], seraphControl.seraphParent).GetComponent<Seraph>();
                tooltip_title.text = "Rupture Seraph";
                tooltip_description.text = "Creates an Explosion damaging and knocking back enemies.";
                break;
            case Genome.Siphon:
                backdrop.color = seraphColors[1];
                seraphIcon.sprite = siphonSprite;
                mySeraph = Instantiate(SeraphPrefabs[1], seraphControl.seraphParent).GetComponent<Seraph>();
                tooltip_title.text = "Siphon Seraph";
                tooltip_description.text = "Heal for a portion of the damage dealt or received.";
                break;
            case Genome.Contaminate:
                backdrop.color = seraphColors[2];
                seraphIcon.sprite = contaminateSprite;
                mySeraph = Instantiate(SeraphPrefabs[2], seraphControl.seraphParent).GetComponent<Seraph>();
                tooltip_title.text = "Contamination Seraph";
                tooltip_description.text = "Infect targets with a gradual poison effect.";
                break;
            case Genome.Storm:
                backdrop.color = seraphColors[3];
                mySeraph = Instantiate(SeraphPrefabs[3], seraphControl.seraphParent).GetComponent<Seraph>();
                tooltip_title.text = "Storm Seraph";
                tooltip_description.text = "Channel a powerful Lightning Strike.";
                break;
            case Genome.Surge:
                backdrop.color = seraphColors[4];
                mySeraph = Instantiate(SeraphPrefabs[4], seraphControl.seraphParent).GetComponent<Seraph>();
                break;


            default:
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.75f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        myRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        this.transform.position = Slot.transform.position;
    }

    private void OnMouseOver()
    {
        Tooltip_box.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        Tooltip_box.gameObject.SetActive(false);

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public Seraph ReturnSeraph()
    {
        return mySeraph;
    }

    public void SetSlot(AugmentSlot slot)
    {
        mySlot = slot;
    }

    public void GoToSpot() // called from "Seraph"
    {
        if(Slot != null)
            this.transform.position = Slot.transform.position;
    }
}
