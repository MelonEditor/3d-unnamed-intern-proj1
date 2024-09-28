
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScript : MonoBehaviour
{
    public SpawnPoint spawnPoint;
    TextMeshProUGUI dialogMessage, centerMessage;
    GameObject dialogPanel;
    bool activated;
    private void Start()
    {
        dialogPanel = GameObject.FindGameObjectWithTag("DialogBoxPanel");
        dialogMessage = GameObject.FindGameObjectWithTag("DialogBoxText").GetComponent<TextMeshProUGUI>();
        centerMessage = GameObject.Find("CenterLabelText").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player")){
            return;
        }
        if (activated) { return; }
        activated = true;
        dialogPanel.SetActive(true);
        dialogPanel.GetComponent<Image>().enabled = true;
        if(dialogMessage != null)
        {
            dialogMessage.text = "Thank you for playing";
        }
        StartCoroutine(TextTimeout(7f));
    }

    private IEnumerator TextTimeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialogMessage.text = "";
        dialogPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        centerMessage.text = "THE END";
        StartCoroutine(LevelTimeout(5f));
    }
    private IEnumerator LevelTimeout(float delay)
    {
        yield return new WaitForSeconds(delay);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Level-1");
    }
}
