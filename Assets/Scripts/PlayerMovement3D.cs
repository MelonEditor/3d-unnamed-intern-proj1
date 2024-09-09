using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement3D : MonoBehaviour
{
    public float speed = 20.0f;
    private Vector3 motion;
    private Rigidbody rb;
    
    [Header("Camera")]
    public Camera cam;
    public float mouseSensitivity = 200f;
    
    public float maxPitch = 90f;
    public float minPitch = -90f;
    public KeyCode exitKey = KeyCode.Escape;

    private float yaw = 0f;
    private float pitch = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        motion = Vector3.zero;
        
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            motion = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        }
        else if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            motion = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized * -1;
        }
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            motion = Vector3.Cross(new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z), Vector3.up).normalized * -1;
        }
        else if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            motion = Vector3.Cross(new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z), Vector3.up).normalized;
        }
        
        rb.linearVelocity = motion * speed;
        
        
        if (Input.GetKeyDown(exitKey))
        {
            Cursor.lockState = CursorLockMode.None;  
            Cursor.visible = true;
            return;
        }

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        cam.transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
