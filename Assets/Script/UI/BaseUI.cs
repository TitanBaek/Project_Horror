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
                continue;   // �̹� ��ųʸ��� �ش� Ű ��(������Ʈ �̸�)�� ������ �׳� �Ѿ.

            transforms.Add(key, child);

            Button button = child.GetComponent<Button>();
            if(button != null)      // �ش� ���ӿ�����Ʈ�� ��ư�� ������ ��ư� Add
            {
                buttons.Add(key, button);
            }

            TMP_Text text = child.GetComponent<TMP_Text>();
            if(text != null)        // �ش� ���ӿ�����Ʈ�� �ؽ�Ʈ�� ������ �ؽ�Ʈ� Add
            {
                texts.Add(key, text);
            }
        }
    }
}
