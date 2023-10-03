using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AAA = UnityEngine.Transform;  //�ٸ� Ŭ������ �̸��� AAA�� ���ڴٴ� ��
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
public class SceneManager : MonoBehaviour
{
    private LoadingUI loadingUI;

    private BaseScene curScene;

    public LoadingUI LoadingUI {  get { return loadingUI; } }
    public BaseScene CurScene
    {
        get
        {
            if (curScene == null)
                curScene = GameObject.FindObjectOfType<BaseScene>();

            return curScene;
        }
    }

    private void Awake()
    {
        LoadingUI ui = Resources.Load<LoadingUI>("UI/LoadingUI");
        loadingUI = Instantiate(ui);
        loadingUI.transform.SetParent(transform);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingRoutine(sceneName));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        loadingUI.FadeOut();
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName); // ��׶��� �ε�(�񵿱��)
        while (!oper.isDone)
        {
            // �ε����϶� ���ư��� �ݺ���
            loadingUI.SetProgress(Mathf.Lerp(0f, 0.5f, oper.progress));
            yield return null;
        }
        
        CurScene.LoadAsync();
        while (CurScene.progress < 1f)
        {
            loadingUI.SetProgress(Mathf.Lerp(0.5f, 1.0f, CurScene.progress));
            yield return null;
        }

        Time.timeScale = 1f;
        loadingUI.FadeIn();
        yield return new WaitForSeconds(0.7f);
    }
}