using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : MonoBehaviour, IDropHandler, IBeginDragHandler
{
    WeaponManager weaponControl;
    RectTransform rect;

    Weapon_DragUI myWeapon;

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
        throw new System.NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<Weapon_DragUI>() != null)
            {
                if (myWeapon == null)
                {
                    myWeapon = eventData.pointerDrag.GetComponent<Weapon_DragUI>();
                    myWeapon.Slot.ClearWeapon();
                    myWeapon.Slot = this;
                    SetWeaponLocation();
                }
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

}
