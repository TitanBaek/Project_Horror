using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogScript", menuName = "Data/Dialogue")]

public class DialogueData : ScriptableObject
{
    [SerializeField] DialogueInfo[] dialogue;
    public DialogueInfo[] Dialogue { get { return dialogue; } }


    [Serializable]
    public class DialogueInfo
    {
        public string name;

        public string[] script;
    }
}