using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour,ICheckable
{ 
    [SerializeField] string[] scripts;
    bool nowOpen = false;
    public bool NowOpen { get { return nowOpen; }  set { nowOpen = value; } }

    public virtual void Check()
    {
        if (!nowOpen)
        {
            GameManager.Dialog.SetDialogScripts(this, scripts);
            nowOpen = true;
        }

        GameManager.Dialog.TabDialogKey();
    }
}
