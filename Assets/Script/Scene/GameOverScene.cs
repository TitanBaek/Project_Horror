using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScene : BaseScene
{
    private Animator anim;
    private TMP_Text tmp;
    private Color32 tmp_color;

    private void Awake()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        tmp = GameObject.FindGameObjectWithTag("UI_Text").GetComponent<TMP_Text>();
        tmp_color = new Color32(89,0,0,255);
    }

    private void Start()
    {
        anim.SetBool("Dead", true);
        tmp.fontSize = 200;
        tmp_color.a = 1;
    }
    protected override IEnumerator LoadingRoutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        progress = 1f;
       
    }
}
