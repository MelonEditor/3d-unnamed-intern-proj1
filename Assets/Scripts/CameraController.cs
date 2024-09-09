using UnityEngine;
using UnityEngine.Serialization;

public class CameraController1 : MonoBehaviour
{
    [FormerlySerializedAs("rotationSpeed")] 
    public float mouseSensitivity = 200f;
    
    public float maxPitch = 90f;
    public float minPitch = -90f;
    public KeyCode exitKey = KeyCode.Escape;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(exitKey))
        {
            Cursor.lockState = CursorLockMode.None;  
            Cursor.visible = true;
            return;
        }

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}


