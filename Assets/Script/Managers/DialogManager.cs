using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private Canvas dialogCanvas;
    [SerializeField] private Dialog dialog;
    private TMP_Text dialog_TextField;
    private Coroutine textCoroutine;

    private void Start()
    {
        Init();
    }
     
    public void Init()
    {
        dialogCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        dialogCanvas.gameObject.name = "dialogCanvas";
        dialogCanvas.sortingOrder = 101;

        dialog = GameManager.Resource.Instantiate(GameManager.Resource.Load<Dialog>("Dialog/DialogText"));
        dialog.transform.SetParent(dialogCanvas.transform, false);
        dialog_TextField = dialog.GetComponent<TMP_Text>();
        dialog_TextField.text = "";
    }

    public void tabDialogKey(string msg = null)
    {
        if (dialog.gameObject.activeSelf)
        {
            // ���̾�α� ����� ����
            HideDialog();
        } else
        {
            // ���̾�α� �ȶ���� ����
            ShowDialog(msg);
        }
    }

    public void ShowDialog(string msg)
    {
        if(dialogCanvas == null)
            GameObject.Find("InGameCanvas").GetComponent<Canvas>();
        dialog.gameObject.SetActive(true);
        Time.timeScale = 0;
        Debug.Log($"���� ���� �޼��� : {msg}");
        textCoroutine = StartCoroutine(playDialog(msg));
    }

    public void HideDialog()
    {
        Time.timeScale = 1.0f;
        StopCoroutine(textCoroutine);
        dialog_TextField.text = "";
        dialog.gameObject.SetActive(false);
    }

    public IEnumerator playDialog(string msg)
    {
        Debug.Log("�÷��� ���̾�α� �ڷ�ƾ�� ���Դ�");
        for (int i = 0; i < msg.Length ; i++)
        {
                dialog_TextField.text += msg[i];
                yield return new WaitForSecondsRealtime(0.05f);            
        }

        yield return null;
    }

    private void OnDisable()
    {
        StopCoroutine(textCoroutine);
    }

}
