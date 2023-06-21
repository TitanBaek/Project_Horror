using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("���콺����");
        text.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("���콺����Ʈ");
        text.color = Color.white;
    }



}
