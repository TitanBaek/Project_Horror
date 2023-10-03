using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    Player player;
    public Player _player { get {  return player; } set { player = value; } }
    Coroutine findPlayerCoroutine;

    private void Start()
    {
        findPlayerCoroutine = StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null );
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnDisable()
    {
        if(findPlayerCoroutine != null)
            StopCoroutine(findPlayerCoroutine);
    }

}
