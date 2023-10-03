using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static DataManager dataManager;
    private static PoolManager poolManager;
    private static ResourceManager resourceManager;
    private static UIManager uiManager;
    private static SceneManager sceneManager;
    private static Inventory inventory;
    private static DialogManager dialogManager;
    private static SoundManager soundManager;

    public static GameManager Instance { get { return instance; } }
    public static DataManager Data { get { return dataManager; } }
    public static PoolManager Pool { get { return poolManager; } }
    public static ResourceManager Resource { get { return resourceManager; } }
    public static UIManager UI { get { return uiManager; } }
    public static Inventory Inventory { get { return inventory; } set { inventory = value; } }
    public static DialogManager Dialog { get { return dialogManager; } set { dialogManager = value; } }
    public static SoundManager Sound { get { return soundManager; } set { soundManager = value; } }

    public static bool isStop = false;


    public static void TimeStop(float num)
    {
        isStop = num == 0f ? true : false;
        Time.timeScale = num;
    }

    public static SceneManager Scene { get { return sceneManager; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject resourceObj = new GameObject();
        resourceObj.name = "ResourceManager";
        resourceObj.transform.parent = transform;
        resourceManager = resourceObj.AddComponent<ResourceManager>();

        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject uiObj = new GameObject();
        uiObj.name = "UIManager";
        uiObj.transform.parent = transform;
        uiManager = uiObj.AddComponent<UIManager>();

        GameObject sceneObj = new GameObject();
        sceneObj.name = "SceneManager";
        sceneObj.transform.parent = transform;
        sceneManager = sceneObj.AddComponent<SceneManager>();

        GameObject dialogObj = new GameObject();
        dialogObj.name = "DialogManager";
        dialogObj.transform.parent = transform;
        dialogManager = dialogObj.AddComponent<DialogManager>();

        GameObject dataObj = new GameObject();
        dataObj.name = "DataManager";
        dataObj.transform.parent = transform;
        dataManager = dataObj.AddComponent<DataManager>();

        GameObject soundObj = new GameObject();
        soundObj.name = "SoundManager";
        soundObj.transform.parent = transform;
        soundManager = soundObj.AddComponent<SoundManager>();
    }

}