using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour,ICheckable
{ 
    [SerializeField] string[] scripts;
    bool nowOpen = false;

    public virtual void Check()
    {
        if (!nowOpen)
        {
            GameManager.Dialog.ShowDialog(scripts[0]);
            nowOpen = true;
        } else
        {
            GameManager.Dialog.HideDialog();
            nowOpen = false;
        }
    }
}
