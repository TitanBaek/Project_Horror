using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class GetItemUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private GameObject itemPos;
    private AudioSource itemAudioSource;

    private void Awake()
    {
        itemAudioSource = GetComponent<AudioSource>();
        itemAudioSource.clip = GameManager.Resource.Load<AudioClip>("Sound/itemBeep");

    }
    private void Start()
    {
        itemAudioSource.Play();
    }
    public void SetGetItemUI(Item item)
    {
        GameManager.TimeStop(0f);
        // �𵨸� �ֱ�
        GameObject itemModel = GameManager.Resource.Instantiate<GameObject>(item.UIRender);
        itemModel.transform.SetParent(itemPos.transform, false);
        itemModel.AddComponent<ItemSpin>();
        // �׸��� �ؽ�Ʈ �����ϱ�
        itemText.text = $"<color=green>{item.ItemName}</color>��(��) �����.";
    }
    
    public void OnCheck(InputValue value)
    {
        GetItemUiDisable();
    }

    public void GetItemUiDisable()
    {
        GameManager.TimeStop(1f);
        GameManager.UI.DisableGetItemScreen();
    }
}
