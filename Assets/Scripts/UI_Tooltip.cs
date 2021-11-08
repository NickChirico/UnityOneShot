using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    [Space(10)]
    public string weaponType;
    public string description;
    public string specialName;
    public int onRight;
    private bool isSelected = false;

    public bool IsSelected { get => isSelected; set => isSelected = value; }

    private void Awake()
    {
    }

    private void Update()
    {
        
    }

    private void ShowTooltip(Vector3 pos)
    {
        /*textBox.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        background.position = pos;
        textBox.rectTransform.position = pos;

        textBox.text = description;
        Vector2 backgroundSize = new Vector2(tooltip_Width + textPadding * 2, textBox.preferredHeight + textPadding * 2);
        background.sizeDelta = backgroundSize;*/
    }

    private void HideTooltip()
    {
        /*textBox.gameObject.SetActive(false);
        background.gameObject.SetActive(false);*/
    }


    public void OnPointerEnter(PointerEventData e)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnPointerExit(PointerEventData e)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnSelect(BaseEventData e)
    {
        IsSelected = true;
    }
}
