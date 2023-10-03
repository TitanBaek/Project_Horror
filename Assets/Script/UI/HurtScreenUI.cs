using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtScreenUI : BaseUI
{
    private Image img; // �� �̹���

    private void Awake()
    {
        base.Awake();
        img = GetComponent<Image>();
    }
    
    public void SetAlpha(float alpha)
    {
        Color color = img.color;
        color.a = 1f - alpha;
        img.color = color;
    }
}
