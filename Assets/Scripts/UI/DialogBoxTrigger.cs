using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DialogBoxTrigger : MonoBehaviour
{
    TextMeshProUGUI dialogMessage;
    GameObject dialogPanel;
    public float timeout = -1;
    public string textToDisplay;
    bool activated;
    
    private void Start()
    {
        dialogPanel = GameObject.FindGameObjectWithTag("DialogBoxPanel");
        dialogMessage = GameObject.FindGameObjectWithTag("DialogBoxText").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated) { return; }
        if(!other.gameObject.CompareTag("Player")){
            return;
        }
        dialogPanel.SetActive(true);
        dialogPanel.GetComponent<Image>().enabled = true;
        if(dialogMessage != null)
        {
            dialogMessage.text = textToDisplay + "\n";
        }
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
