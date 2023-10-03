using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private Canvas dialogCanvas;
    [SerializeField] private Dialog dialog;
    private string[] dialogScripts;
    private Queue<string> dialogQueue;
    private TMP_Text dialog_TextField;
    private Coroutine textCoroutine;
    private Objects haveDialogObject;

    private void Start()
    {
        Init();
    }
     
    public void Init()
    {
        dialogCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        dialogCanvas.gameObject.name = "dialogCanvas";
        dialogCanvas.sortingOrder = 101;
        dialogScripts = new string[] { };
        dialogQueue = new Queue<string>();
        dialog = GameManager.Resource.Instantiate(GameManager.Resource.Load<Dialog>("Dialog/DialogText"));
        dialog.transform.SetParent(dialogCanvas.transform, false);
        dialog_TextField = dialog.GetComponent<TMP_Text>();
        dialog_TextField.text = "";
    }

    public void SetDialogScripts(Objects objects,string[] scripts)
    {
        haveDialogObject = objects;
        dialogScripts = scripts;                        // �Ű������� ���ڿ� �迭 dialogScripts�� �־���
        for(int i = 0; i < dialogScripts.Length; i++)   // ��� ť�� Enqueue
        {
            dialogQueue.Enqueue(dialogScripts[i]);
        }
    }

    public void TabDialogKey()
    {
        if (dialog.gameObject.activeSelf)   // ���̾�α� ����� ����
        {
            if (dialogQueue.Count > 0)      // ��簡 ���� ���Ҵ�.
            {
                StopCoroutine(textCoroutine);
                ShowDialog();
            }
            else                            // ��簡 ���� ����
            {
                HideDialog();
            }
        } else                             // �� ����� ����
        {
            ShowDialog();
        }
    }

    public void ShowDialog()
    {
        if(dialogCanvas == null)
            dialogCanvas = GameObject.Find("InGameCanvas").GetComponent<Canvas>();

        if (!dialog.gameObject.activeSelf)
            dialog.gameObject.SetActive(true);

        if (dialog_TextField.text != "")        
            dialog_TextField.text = "";
        
        Time.timeScale = 0;
        textCoroutine = StartCoroutine(playDialog(dialogQueue.Dequeue().ToString()));
    }

    public void HideDialog()
    {
        Time.timeScale = 1.0f;
        StopCoroutine(textCoroutine);
        dialog_TextField.text = "";
        ClearObejctState();
        dialog.gameObject.SetActive(false);
    }

    public void ClearObejctState()
    {
        haveDialogObject.NowOpen = false;
        dialogQueue.Clear();                            // ��� ť �ʱ�ȭ
    }

    public IEnumerator playDialog(string msg)
    {
        for (int i = 0; i < msg.Length ; i++)
        {
                dialog_TextField.text += msg[i];
                yield return new WaitForSecondsRealtime(0.02f);            
        }

        yield return null;
    }

    private void OnDisable()
    {
        if(textCoroutine != null)
            StopCoroutine(textCoroutine);
    }

}
