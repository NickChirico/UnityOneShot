using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Seraph_UI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    SeraphController seraphControl;

    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public AugmentSlot mySlot;
    private RectTransform myRect;
    private Image image;

    //public GameObject mySeraphPrefab;
    public Seraph mySeraph;

    public AugmentSlot Slot { get => mySlot; set => mySlot = value; }


    public enum Genome { None, Rupture, Siphon, Contaminate, Surge, Hex, Calcify }
    [Space(10)]
    public Genome seraphType;

    public GameObject[] SeraphPrefabs;
    public Color[] seraphColors;

    private void Awake()
    {

        seraphControl = SeraphController.GetSeraphController;
        myRect = this.GetComponent<RectTransform>();
        image = this.GetComponent<Image>();
        canvas = FindObjectOfType<MainCanvas>().GetCanvas();
        canvasGroup = this.GetComponent<CanvasGroup>();

        //mySeraph = Instantiate(mySeraphPrefab, seraphControl.seraphParent).GetComponent<Seraph>();
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
                image.color = seraphColors[0];
                mySeraph = Instantiate(SeraphPrefabs[0], seraphControl.seraphParent).GetComponent<Seraph>();
                break;
            case Genome.Siphon:
                image.color = seraphColors[1];
                mySeraph = Instantiate(SeraphPrefabs[1], seraphControl.seraphParent).GetComponent<Seraph>();
                break;
            case Genome.Contaminate:
                image.color = seraphColors[2];
                mySeraph = Instantiate(SeraphPrefabs[2], seraphControl.seraphParent).GetComponent<Seraph>();
                break;
            case Genome.Surge:
                image.color = seraphColors[3];
                mySeraph = Instantiate(SeraphPrefabs[3], seraphControl.seraphParent).GetComponent<Seraph>();
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
