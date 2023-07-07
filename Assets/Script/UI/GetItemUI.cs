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
        // 모델링 넣기
        GameObject itemModel = GameManager.Resource.Instantiate<GameObject>(item.UIRender);
        itemModel.transform.SetParent(itemPos.transform, false);
        itemModel.AddComponent<ItemSpin>();
        // 그리고 텍스트 변경하기
        itemText.text = $"<color=green>{item.ItemName}</color>을(를) 얻었다.";
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
