using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "dialogScript")]


public class DialogScript : ScriptableObject
{
    public string[] myCar = new string[]
    {   "내가 타고 온 차, 심하게 파손되어있다.",
        "신기하게도 난 다치지 않았다."};

    public string[] abandonRadio = new string[]
    {
        "라디오에서 이상한 잡음이 계속해서 들린다."
    };

}
