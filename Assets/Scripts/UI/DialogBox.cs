using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogBox : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogMessage; // Drag your Text component here

    // Show the dialog with a custom message
    public void ShowDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogMessage.text = message;
    }

    public void Start()
    {
        dialogPanel.GetComponent<Image>().enabled = false;
    }
}

