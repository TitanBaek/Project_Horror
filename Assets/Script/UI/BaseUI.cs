using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    protected Dictionary<string, RectTransform> transforms;
    protected Dictionary<string, Button> buttons;
    protected Dictionary<string, TMP_Text> texts;

    protected virtual void Awake()
    {
        BindChild();
    }
    private void BindChild()
    {
        transforms = new Dictionary<string, RectTransform>();
        buttons = new Dictionary<string, Button>();
        texts = new Dictionary<string, TMP_Text>();

        RectTransform[] children = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform child in children)
        {
            string key = child.gameObject.name;
            if (transforms.ContainsKey(key))
                continue;   // 이미 딕셔너리에 해당 키 값(오브젝트 이름)이 있으면 그냥 넘어감.

            transforms.Add(key, child);

            Button button = child.GetComponent<Button>();
            if(button != null)      // 해당 게임오브젝트에 버튼이 있으면 버튼즈에 Add
            {
                buttons.Add(key, button);
            }

            TMP_Text text = child.GetComponent<TMP_Text>();
            if(text != null)        // 해당 게임오브젝트에 텍스트가 있으면 텍스트즈에 Add
            {
                texts.Add(key, text);
            }
        }
    }
}
