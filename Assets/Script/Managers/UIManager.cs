using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private EventSystem eventSystem;

    private Canvas popUpCanvas;
    //private Stack<BaseUI> popUpStack;

    private Canvas windowCanvas;

    private Canvas inGameCanvas;

    public HurtScreenUI hurtScreenUI;

    public GetItemUI getItemUI;

    public InventoryUI inventoryUI;

    public bool uiActive;
    private void Awake()
    {
        Init();
        uiActive = false;
        eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
        eventSystem.transform.SetParent(transform, false);
    }

    public void Init()
    {
        /*
        popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        popUpCanvas.gameObject.name = "PopUpCanvas";
        popUpCanvas.sortingOrder = 100;
        popUpStack = new Stack<PopUpUI>();
        */
        /*
        windowCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        windowCanvas.gameObject.name = "WindowCanvas";
        windowCanvas.sortingOrder = 10;
        */
        //gameSceneCanvas.sortingOrder = 1;

        inGameCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        inGameCanvas.gameObject.name = "InGameCanvas";
        //inGameCanvas.transform.parent = transform;
        inGameCanvas.sortingOrder = 100;
    }
    /*   
    public void ShowHurtScreen<T>(T hurtScreenUI) where T : HurtScreenUI
    {
        T ui = GameManager.Pool.GetUI<T>(hurtScreenUI);
        ui.transform.SetParent(inGameCanvas.transform, false);
    }
    */

    public void ShowGetItemScreen<T>(Item item) where T : GetItemUI
    {
        uiActive = true;
        // 생성하고 매개변수로 받아온 아이템을 전달해서 초기세팅 하게
        //GetItemUI ui = GameManager.Pool.GetUI("UI/GetItemUI");
        T ui = GameManager.Resource.Load<T>("UI/GetItemUI");
        getItemUI = GameManager.Resource.Instantiate(ui);
        getItemUI.transform.SetParent(inGameCanvas.transform, false);

        getItemUI.SetGetItemUI(item);
    }

    public void DisableGetItemScreen()
    {
        uiActive = false;
        Debug.Log("UI매니저 아이템스크린 디스트로이");
        GameManager.Resource.Destroy(getItemUI.gameObject);
    }

    public void ShowHurtScreen<T>(string path) where T : HurtScreenUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        hurtScreenUI = GameManager.Resource.Instantiate(ui);
        hurtScreenUI.transform.SetParent(inGameCanvas.transform, false);
    }

    public void CreateInventoryUI()
    {
        inventoryUI = GameManager.Resource.Instantiate<InventoryUI>("UI/InventoryUI",new Vector3(1000,1000,1000),Quaternion.identity);
        inventoryUI.transform.SetParent(transform, false);
    }

    /*
    public T ShowPopUpUI<T>(T popUpUI) where T : PopUpUI
    {
        if (popUpStack.Count > 0)
        {
            PopUpUI prevUI = popUpStack.Peek();
            prevUI.gameObject.SetActive(false);
        }

        T ui = GameManager.Pool.GetUI<T>(popUpUI);
        ui.transform.SetParent(popUpCanvas.transform, false);
        popUpStack.Push(ui);

        Time.timeScale = 0f;
        return ui;
    }

    public T ShowPopUpUI<T>(string path) where T : PopUpUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowPopUpUI(ui);
    }

    public void ClosePopUpUI()
    {
        PopUpUI ui = popUpStack.Pop();
        GameManager.Pool.ReleaseUI(ui.gameObject);

        if (popUpStack.Count > 0)
        {
            PopUpUI curUI = popUpStack.Peek();
            curUI.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void ClearPopUpUI()
    {
        while (popUpStack.Count > 0)
        {
            ClosePopUpUI();
        }
    }

    public T ShowWindowUI<T>(T windowUI) where T : WindowUI
    {
        T ui = GameManager.Pool.GetUI(windowUI);
        ui.transform.SetParent(windowCanvas.transform, false);
        return ui;
    }

    public T ShowWindowUI<T>(string path) where T : WindowUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowWindowUI(ui);
    }

    public void SelectWindowUI<T>(T windowUI) where T : WindowUI
    {
        windowUI.transform.SetAsLastSibling();
    }

    public void CloseWindowUI<T>(T windowUI) where T : WindowUI
    {
        GameManager.Pool.ReleaseUI(windowUI.gameObject);
    }
    public void ClearWindowUI()
    {
        WindowUI[] windows = windowCanvas.GetComponentsInChildren<WindowUI>();

        foreach (WindowUI windowUI in windows)
        {
            GameManager.Pool.ReleaseUI(windowUI.gameObject);
        }
    }

    public T ShowInGameUI<T>(T gameUi) where T : InGameUI
    {
        T ui = GameManager.Pool.GetUI(gameUi);
        ui.transform.SetParent(inGameCanvas.transform, false);

        return ui;
    }

    public T ShowInGameUI<T>(string path) where T : InGameUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowInGameUI(ui);
    }

    public void CloseInGameUI<T>(T inGameUI) where T : InGameUI
    {
        GameManager.Pool.ReleaseUI(inGameUI.gameObject);
    }

    public void ClearInGameUI()
    {
        InGameUI[] inGames = inGameCanvas.GetComponentsInChildren<InGameUI>();

        foreach (InGameUI inGameUI in inGames)
        {
            GameManager.Pool.ReleaseUI(inGameUI.gameObject);
        }
    }
    */
}