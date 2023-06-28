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
        // 씬로딩이 끝나고 이제 Prefab 위치 조정 등을 추가적으로 작업해준 뒤에 로딩을 완료 시키자
        // 몬스터 랜덤 배치 
        progress = 0f;
        Debug.Log("몬스터 랜덤배치");
        yield return new WaitForSecondsRealtime(0.2f);
        progress = 0.2f;
        // 리소스 불러오기
        Debug.Log("리소스 불러오기");
        yield return new WaitForSecondsRealtime(0.5f);
        progress = 0.4f;
        // 랜덤 아이템 배치
        Debug.Log("아이템 배치");
        yield return new WaitForSecondsRealtime(0.5f);
        progress = 0.7f;
        // 플레이어 생성 
        Debug.Log("플레이어 배치");
        Debug.Log("카메라 팔로우");
        yield return new WaitForSecondsRealtime(1f);
        progress = 1f;
        for (int i = 0; i < a_source.Length; i++)
            a_source[i].Play();
    }

}
