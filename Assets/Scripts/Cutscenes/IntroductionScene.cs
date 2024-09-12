using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class IntroductionScene : MonoBehaviour
{
    public Transform npc;
    bool playedFlag = false;
    public float moveDistance = 10f;  
    public float duration = 1f;       
    
    private void Awake()
    {
        npc = GameObject.FindGameObjectWithTag("NPC").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("starting cutscene");
        if (playedFlag) { return; }
        StartCoroutine(StartCutscene());
        playedFlag = true;
    }
    
    IEnumerator StartCutscene()
    {
        Vector3 startPos = npc.position;
        Vector3 endPos = startPos + Vector3.down * moveDistance;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            npc.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        npc.position = endPos;
        
        
        yield return new WaitForSeconds(4f);
        Debug.Log("Welcome to the Infinite Dungeons");
        yield return new WaitForSeconds(5f);
        Debug.Log("1");
    }
}
