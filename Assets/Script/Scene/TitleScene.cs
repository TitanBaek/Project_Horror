using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    private void Awake()
    {
        GameManager.Scene.LoadingUI.FadeIn();
    }
    public void StartButton()
    {
        GameManager.Scene.LoadScene("TownScene");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    protected override IEnumerator LoadingRoutine()
    {
        yield return new WaitForSeconds(1f);
    }

}
