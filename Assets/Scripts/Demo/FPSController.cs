using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : PortalTraveller {

    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;
    public float terminalVelocity = 100;

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Vector2 pitchMinMax = new Vector2 (-40, 85);
    public float rotationSmoothTime = 0.1f;

    public Vector3 gravityDirection = Vector3.down;
    
    Camera cam;
    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    float gravitationalVelocity;
    Vector3 velocity;
    Vector3 smoothV;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    bool jumping;
    float lastGroundedTime;
    bool disabled;

    private Rigidbody rb;

    public GameObject camHolder;

    bool isGrounded;
    void Start () {
        cam = Camera.main;
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        

        yaw = transform.eulerAngles.y;
        pitch = cam.transform.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;
        
        rb = GetComponent<Rigidbody>();
    }
    void OnCollisionStay()
    {
        isGrounded = true;
    }
    void OnCollisionExit()
    {
        isGrounded = false;
    }
    void Update () {
        
        if (Input.GetKeyDown (KeyCode.P)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Break ();
        }
        if (Input.GetKeyDown (KeyCode.O)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            disabled = !disabled;
        }

        if (disabled) {
            return;
        }

        if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {

            rb.linearVelocity = new Vector3(rb.linearVelocity.x + jumpForce * -gravityDirection.x, rb.linearVelocity.y + jumpForce * -gravityDirection.y,
                rb.linearVelocity.z + jumpForce * -gravityDirection.z);
        }

        float mX = Input.GetAxisRaw ("Mouse X");
        float mY = Input.GetAxisRaw ("Mouse Y");

        float mMag = Mathf.Sqrt (mX * mX + mY * mY);
        if (mMag > 5) {
            mX = 0;
            mY = 0;
        }

        yaw += mX * mouseSensitivity;
        pitch -= mY * mouseSensitivity;
        pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);
        smoothPitch = Mathf.SmoothDampAngle (smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle (smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);
        
        
        Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

        Vector3 inputDir = input.normalized;
        Physics.gravity = gravityDirection * gravity;
        


        if (gravityDirection == Vector3.down)
        {
            inputDir = new Vector3 (input.x, 0, input.y).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            float currentSpeed = (Input.GetKey (KeyCode.LeftShift)) ? runSpeed : walkSpeed;
            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);
        
            rb.linearVelocity = new Vector3 (velocity.x, rb.linearVelocity.y, velocity.z);

            transform.eulerAngles = new Vector3 (0, smoothYaw, 0);
            
            camHolder.transform.eulerAngles = -gravityDirection * smoothYaw;
            cam.transform.localEulerAngles = Vector3.right * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 0);
        }
        else if (gravityDirection == Vector3.up)
        {
            inputDir = new Vector3 (-input.x, 0, input.y).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            float currentSpeed = (Input.GetKey (KeyCode.LeftShift)) ? runSpeed : walkSpeed;
            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);
        
            rb.linearVelocity = new Vector3 (velocity.x, rb.linearVelocity.y, velocity.z);

            transform.eulerAngles = new Vector3 (180, -smoothYaw, 0);
            
            camHolder.transform.eulerAngles = -gravityDirection * smoothYaw;
            cam.transform.localEulerAngles = Vector3.left * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 180);
        }
        else if (gravityDirection == Vector3.left)
        {
            inputDir = new Vector3 (0, -input.x, input.y).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            float currentSpeed = (Input.GetKey (KeyCode.LeftShift)) ? runSpeed : walkSpeed;
            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);

            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, velocity.y, velocity.z);
            
            transform.eulerAngles = new Vector3 (smoothYaw, 0, -90);
            
            camHolder.transform.eulerAngles = -gravityDirection * smoothYaw;
            cam.transform.localEulerAngles = Vector3.down * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, -90);
        }
        else if (gravityDirection == Vector3.right)
        {
            inputDir = new Vector3 (0, input.x, input.y).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            float currentSpeed = (Input.GetKey (KeyCode.LeftShift)) ? runSpeed : walkSpeed;
            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);

            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, velocity.y, velocity.z);
            
            transform.eulerAngles = new Vector3 (-smoothYaw, 0, 90);
            
            camHolder.transform.eulerAngles = -gravityDirection * smoothYaw;
            cam.transform.localEulerAngles = Vector3.up * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 90);
        }
        else if (gravityDirection == Vector3.forward)
        {
            inputDir = new Vector3 (input.x, input.y, 0).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            float currentSpeed = (Input.GetKey (KeyCode.LeftShift)) ? runSpeed : walkSpeed;
            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);
            
            rb.linearVelocity = new Vector3 (velocity.x, velocity.y, rb.linearVelocity.z);
            
            transform.eulerAngles = new Vector3 (-90, 0, 0);
            
            camHolder.transform.eulerAngles = -gravityDirection * smoothYaw;
            cam.transform.localEulerAngles = Vector3.right * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(-90 + cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
        }
        else if (gravityDirection == Vector3.back)
        {
            inputDir = new Vector3 (input.x, -input.y, 0).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            float currentSpeed = (Input.GetKey (KeyCode.LeftShift)) ? runSpeed : walkSpeed;
            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);
            
            rb.linearVelocity = new Vector3 (velocity.x, velocity.y, rb.linearVelocity.z);
            
            transform.eulerAngles = new Vector3 (90, 0, 0);
            
            camHolder.transform.eulerAngles = -gravityDirection * smoothYaw;
            cam.transform.localEulerAngles = Vector3.right * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(90 + cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
        }
    }

    public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle (smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector (fromPortal.InverseTransformVector (velocity));
        Physics.SyncTransforms ();
    }

}