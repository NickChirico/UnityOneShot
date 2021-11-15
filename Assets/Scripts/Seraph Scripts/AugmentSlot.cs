using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AugmentSlot : MonoBehaviour, IDropHandler, IBeginDragHandler
{
    SeraphController seraphControl;

    public enum EquipmentType { bag, mainWeapon, altWeapon, armor, boots, stimulate }
    public EquipmentType myEquipment;

    public Seraph_UI mySeraph_ui;
    //public Seraph mySeraph;

    RectTransform rect;
    private void Awake()
    {
        seraphControl = SeraphController.GetSeraphController;
        rect = this.GetComponent<RectTransform>();

        if (mySeraph_ui != null)
        {
            //mySeraph = mySeraph_ui.mySeraph;
            mySeraph_ui.Slot = this;
            SetSeraphLocation();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<Seraph_UI>() != null)
            {
                mySeraph_ui = null;
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<Seraph_UI>() != null)
            {
                if (mySeraph_ui == null)
                {
                    mySeraph_ui = eventData.pointerDrag.GetComponent<Seraph_UI>();//.ReturnSeraph();
                    mySeraph_ui.Slot.ClearSeraph();
                    mySeraph_ui.Slot=this;
                    SetSeraphLocation();
                    Debug.Log(mySeraph_ui.name);
                    //seraphControl.AddToSeraphList(mySeraph_ui, myEquipment);
                }
                else
                {
                    
                }
            }
        }
    }

    private void Start()
    {

    }

    private void SetSeraphLocation()
    {
        mySeraph_ui.transform.position = rect.transform.position;
    }

    public void ClearSeraph()
    {
        mySeraph_ui = null;
    }

    public bool HasSeraph()
    {
        if (mySeraph_ui != null)
            return true;
        else
            return false;
    }

}
