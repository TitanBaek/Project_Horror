using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScene : BaseScene
{
    private AudioSource[] a_source;

    private void Awake()
    {
        a_source = GetComponents<AudioSource>();
        GameManager.UI.Init();
        GameManager.UI.ShowHurtScreen<HurtScreenUI>("UI/HurtScreenUI");
    }

    protected override IEnumerator LoadingRoutine()
    {
        // ���ε��� ������ ���� Prefab ��ġ ���� ���� �߰������� �۾����� �ڿ� �ε��� �Ϸ� ��Ű��
        // ���� ���� ��ġ 
        progress = 0f;
        Debug.Log("���� ������ġ");
        yield return new WaitForSecondsRealtime(0.2f);
        progress = 0.2f;
        // ���ҽ� �ҷ�����
        Debug.Log("���ҽ� �ҷ�����");
        yield return new WaitForSecondsRealtime(0.5f);
        progress = 0.4f;
        // ���� ������ ��ġ
        Debug.Log("������ ��ġ");
        yield return new WaitForSecondsRealtime(0.5f);
        progress = 0.7f;
        // �÷��̾� ���� 
        Debug.Log("�÷��̾� ��ġ");
        Debug.Log("ī�޶� �ȷο�");
        yield return new WaitForSecondsRealtime(1f);
        progress = 1f;
        for (int i = 0; i < a_source.Length; i++)
            a_source[i].Play();
    }

}
