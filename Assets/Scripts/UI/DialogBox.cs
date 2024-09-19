using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public GameObject dialogPanel; // Drag your dialog panel here in the Inspector
    public Text dialogMessage; // Drag your Text component here

    // Show the dialog with a custom message
    public void ShowDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogMessage.text = message;
    }

    public void Start()
    {
        
    }
}

