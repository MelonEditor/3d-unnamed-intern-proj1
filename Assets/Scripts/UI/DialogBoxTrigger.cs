using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DialogBoxTrigger : MonoBehaviour
{
    Text dialogMessage;
    GameObject dialogPanel;
    public float timeout = -1;
    public string textToDisplay;
    bool activated;
    
    private void Start()
    {
        dialogPanel = GameObject.FindGameObjectWithTag("DialogBoxPanel");
        dialogMessage = GameObject.FindGameObjectWithTag("DialogBoxText").GetComponent<Text>();
        dialogPanel.GetComponent<Image>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated) { return; }
        dialogPanel.SetActive(true);
        dialogPanel.GetComponent<Image>().enabled = true;
        dialogMessage.text += textToDisplay + "\n";
        activated = true;
        
        if (timeout < 0) return;
        StartCoroutine(TextTimeout(8.0f));
    }

    private IEnumerator TextTimeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogMessage.text = "";
        dialogPanel.SetActive(false);
    }
}
