using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : MonoBehaviour, IDropHandler, IBeginDragHandler
{
    WeaponManager weaponControl;
    RectTransform rect;

    public Weapon_DragUI myWeapon;

    private void Awake()
    {
        weaponControl = WeaponManager.GetWeaponManager;
        rect = this.GetComponent<RectTransform>();

        if (myWeapon != null)
        {
            myWeapon.Slot = this;
            SetWeaponLocation();
        }
    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<Weapon_DragUI>() != null)
            {
                Weapon_DragUI newWeapon = eventData.pointerDrag.GetComponent<Weapon_DragUI>();
                if (myWeapon == null)
                {
                    myWeapon = newWeapon;
                    myWeapon.Slot.ClearWeapon();
                    myWeapon.Slot = this;
                    SetWeaponLocation();
                }
            }
            else if (eventData.pointerDrag.GetComponent<WeaponSlot>() != null)
            {
                WeaponSlot newSlot = eventData.pointerDrag.GetComponent<WeaponSlot>();

                Debug.Log("NEW WEAPON");
                //myWeapon.Slot = newWeapon.Slot;
                //myWeapon.Slot.SetWeaponLocation();
                //myWeapon = newWeapon;
                //myWeapon.Slot.SetWeaponLocation();
            }
        }
    }

    private void SetWeaponLocation()
    {
        myWeapon.transform.position = rect.transform.position;
    }

    public void ClearWeapon()
    {
        myWeapon = null;
    }

    public Weapon_DragUI GetWeapon()
    {
        return myWeapon;
    }

    public Vector2 GetPosition()
    { return rect.position; }

}
