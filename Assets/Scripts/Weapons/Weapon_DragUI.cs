using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon_DragUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    WeaponManager weaponControl;
    public string weaponName;


    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public WeaponSlot Slot;
    private RectTransform myRect;

    private void Awake()
    {
        weaponControl = WeaponManager.GetWeaponManager;
        myRect = this.GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
        canvasGroup = this.GetComponent<CanvasGroup>();
    }
    void Start()
    {
       // GoToSpot();
    }

    void Update()
    {

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
        myRect.position = Slot.GetPosition();
        //this.transform.position = Slot.transform.position;
    }

    public void GoToSpot()
    {
        this.transform.position = Slot.transform.position;

        //if (Slot != null)
          //  myRect.position = Slot.GetPosition();
            //this.transform.position = Slot.transform.position;
    }
}
