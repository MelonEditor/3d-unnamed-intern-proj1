using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelTrigger : MonoBehaviour
{
    Text message;
    public string textToDisplay;
    bool activated = false;

    private void Awake()
    {
        message = GameObject.Find("CenterLabelText").GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated) { return; }
        message.text = textToDisplay;
        StartCoroutine(TextTimeout(8.0f));
        activated = true;
    }

    private IEnumerator TextTimeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        message.text = "";
    }
}
