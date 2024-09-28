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
    Vector3 velocity;
    Vector3 smoothV;

    private Rigidbody rb;

    public GameObject camHolder;
    public SpawnPoint SpawnPoint;
    bool isGrounded;

    public AudioSource walkingSound;

    void Start () {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }
    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, -gravityDirection) > 0.7f)
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }
    void OnCollisionExit()
    {
        isGrounded = false;
    }
    public bool isMenuActive = true;
    public void StartGame(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camHolder.transform.rotation = Quaternion.identity;
        isMenuActive = false;
    }
    bool walkingSoundActive;
    private IEnumerator PlaySoundWithDelay(AudioSource audioSource)
    {
        walkingSoundActive = true;
        audioSource.Play();
        yield return new WaitForSeconds(Input.GetKey (KeyCode.LeftShift) ? 0.3f : 0.7f);
        walkingSoundActive = false;
    }
    void FixedUpdate(){
       if(isMenuActive){
            return;
        }
        rb.AddForce(gravityDirection * gravity, ForceMode.Acceleration);

        if (isJumping) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x + jumpForce * -gravityDirection.x, rb.linearVelocity.y + jumpForce * -gravityDirection.y,
                rb.linearVelocity.z + jumpForce * -gravityDirection.z);
            isJumping = false;
        }
    }
    bool cursorLock = false;
    bool isJumping = false;
    void Update () {
        if(isMenuActive){
            return;
        }
        if (Input.GetKeyDown (KeyCode.Escape) && !cursorLock) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorLock = true;
        }else if(Input.GetKeyDown (KeyCode.Escape) && cursorLock){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorLock = false;
        }
        if(cursorLock){
            return;
        }

        if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {
            isJumping = true;
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
        
        // Play walking sound if moving
        if (input.magnitude > 0 && isGrounded && !walkingSoundActive)
        {
            StartCoroutine(PlaySoundWithDelay(walkingSound));
        }


        Vector3 inputDir;

        
        camHolder.transform.position = transform.position - gravityDirection * 0.7f;
        float currentSpeed = Input.GetKey (KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (gravityDirection == Vector3.down)
        {
            inputDir = new Vector3 (input.x, 0, input.y).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);
        
            rb.linearVelocity = new Vector3 (velocity.x, rb.linearVelocity.y, velocity.z);

            transform.eulerAngles = new Vector3 (0, 0, 0);

            cam.transform.localEulerAngles = Vector3.right * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 0);
        }
        else if (gravityDirection == Vector3.up)
        {
            inputDir = new Vector3 (-input.x, 0, input.y).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);
        
            rb.linearVelocity = new Vector3 (velocity.x, rb.linearVelocity.y, velocity.z);

            transform.eulerAngles = new Vector3 (180, 0, 0);
            
            cam.transform.localEulerAngles = Vector3.left * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 180);
        }
        else if (gravityDirection == Vector3.left)
        {
            inputDir = new Vector3 (0, -input.x, input.y).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);

            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, velocity.y, velocity.z);
            
            transform.eulerAngles = new Vector3 (0, 0, -90);
            
            cam.transform.localEulerAngles = Vector3.down * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, -90);
        }
        else if (gravityDirection == Vector3.right)
        {
            inputDir = new Vector3 (0, input.x, input.y).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);

            rb.linearVelocity = new Vector3 (rb.linearVelocity.x, velocity.y, velocity.z);
            
            transform.eulerAngles = new Vector3 (0, 0, 90);
            
            cam.transform.localEulerAngles = Vector3.up * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 90);
        }
        else if (gravityDirection == Vector3.forward)
        {
            inputDir = new Vector3 (input.x, input.y, 0).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);
            
            rb.linearVelocity = new Vector3 (velocity.x, velocity.y, rb.linearVelocity.z);
            
            transform.eulerAngles = new Vector3 (-90, 0, 0);
            
            cam.transform.localEulerAngles = Vector3.right * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(-90 + cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
        }
        else if (gravityDirection == Vector3.back)
        {
            inputDir = new Vector3 (input.x, -input.y, 0).normalized;
            Vector3 worldInputDir = camHolder.transform.TransformDirection (inputDir);

            Vector3 targetVelocity = worldInputDir * currentSpeed;
            velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);
            
            rb.linearVelocity = new Vector3 (velocity.x, velocity.y, rb.linearVelocity.z);
            
            transform.eulerAngles = new Vector3 (90, 0, 0);
            
            cam.transform.localEulerAngles = Vector3.right * smoothPitch;
            cam.transform.localEulerAngles = new Vector3(90 + cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
        }
        // Smoothly interpolate the camHolder rotation

        Vector3 targetRotation = -gravityDirection * smoothYaw;
        if(rotationSmooth){
            camHolder.transform.rotation = Quaternion.Slerp(camHolder.transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * 10f);
        }
        else{
            camHolder.transform.rotation = Quaternion.Euler(targetRotation);
        }

        if (rb.linearVelocity.magnitude > terminalVelocity)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * terminalVelocity;
        }
        
    }
    bool rotationSmooth = false;
    public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        yaw += rot.eulerAngles.y;
        smoothYaw += rot.eulerAngles.y;
        transform.eulerAngles = -gravityDirection * smoothYaw;
        velocity = toPortal.TransformVector (fromPortal.InverseTransformVector (velocity));
        Update();
        Physics.SyncTransforms ();
    }

    public void CameraRotationTransition(Vector3 newGravityDirection){
        rotationSmooth = true;
        gravityDirection = newGravityDirection;
        StartCoroutine(RotationSmooth());
    }
    private IEnumerator RotationSmooth(){
        yield return new WaitForSeconds(0.4f);
        rotationSmooth = false;
    }
    public void ReturnToSpawnPoint()
    {
        transform.position = new Vector3(SpawnPoint.transform.position.x - 1.2f * SpawnPoint.gravityDirection.x,
        SpawnPoint.transform.position.y - 1.2f * SpawnPoint.gravityDirection.y, SpawnPoint.transform.position.z - 1.2f * SpawnPoint.gravityDirection.z);
        transform.rotation = SpawnPoint.transform.rotation;
        gravityDirection = SpawnPoint.gravityDirection;
        rb.linearVelocity = Vector3.zero;
    }
}