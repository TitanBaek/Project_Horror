using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtScreenUI : BaseUI
{
    private Image img; // 피 이미지

    private void Awake()
    {
        base.Awake();
        img = GetComponent<Image>();
    }
    
    public void SetAlpha(float alpha)
    {
        Debug.Log($"{alpha}가 셋알파 들어옴");
        Color color = img.color;
        color.a += 0.2f;
        img.color = color;
    }
}
