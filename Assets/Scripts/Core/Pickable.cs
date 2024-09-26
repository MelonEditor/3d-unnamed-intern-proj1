using System.Collections;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private Rigidbody rb;
    public AudioSource collisionSound; // Reference to the AudioSource component
    public float velocityThreshold = 5f; // Set your desired threshold

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(UnmuteCollisionSound());
    }

    private IEnumerator UnmuteCollisionSound()
    {
        yield return new WaitForSeconds(1f);
        collisionSound.mute = false;
    }

    void Update()
    {
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 20);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.linearVelocity.magnitude > velocityThreshold) // Check if linearVelocity exceeds threshold
        {
            collisionSound.Play(); // Play the sound
        }
    }

    public GameObject PickUp()
    {
        if (rb != null)
        {
            //rb.isKinematic = true;
        }

        //transform.position = Vector3.zero;
        //transform.rotation = Quaternion.identity;
        return this.gameObject;
    }
}
