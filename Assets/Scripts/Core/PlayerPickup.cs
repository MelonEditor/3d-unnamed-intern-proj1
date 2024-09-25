using System;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickableLayerMask;
    [SerializeField]
    private LayerMask groundLayerMask;
    
    private Transform playerCameraTransform;
    
    [SerializeField]
    [Min(1)]
    private float hitRange = 3;

    private GameObject inHandItem = null;
    private RaycastHit hit;

    private Rigidbody inHandItemRigidbody;
    private Collider playerCollider;
    //[SerializeField]
    //private AudioSource pickUpSource;

    [SerializeField]
    private float moveForce = 500f; // Force applied to move the object
    [SerializeField]
    private float maxVelocity = 5f; // Maximum velocity of the held object
    [SerializeField]
    private float highDragDistance = 0.5f; // Distance at which to apply high drag
    [SerializeField]
    private float lowDrag = 0.5f; // Drag when not close to target
    [SerializeField]
    private float highDrag = 10f; // Drag when close to target

    private Rigidbody rb;
    private void Start()
    {
        playerCameraTransform = Camera.main.transform;
        playerCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        // Remove this line as we'll get the Rigidbody when we pick up an item
        // inHandItemRigidbody = inHandItem.GetComponent<Rigidbody>();
    }

    private void Drop()
    {
        inHandItemRigidbody.useGravity = true;
        inHandItemRigidbody.linearDamping = 0f;
        inHandItemRigidbody.angularDamping = 0.05f;
        if (inHandItem != null)
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            // Re-enable collision between the dropped item and the player
            Collider itemCollider = inHandItem.GetComponent<Collider>();
            if (itemCollider != null && playerCollider != null)
            {
                Physics.IgnoreCollision(itemCollider, playerCollider, false);
            }
            inHandItem = null;
            inHandItemRigidbody = null;
        }
    }
    private void PickUp()
    {
        if(hit.collider != null && inHandItem == null)
        {
            Pickable pickableItem = hit.collider.GetComponent<Pickable>();
            if (pickableItem != null)
            {
                // Disable collision between the picked up item and the player
                Collider itemCollider = pickableItem.GetComponent<Collider>();
                if (itemCollider != null && playerCollider != null)
                {
                    Physics.IgnoreCollision(itemCollider, playerCollider, true);
                }
                //pickUpSource.Play();
                inHandItem = pickableItem.PickUp();
                inHandItemRigidbody = inHandItem.GetComponent<Rigidbody>();
                if (inHandItemRigidbody != null)
                {
                    inHandItemRigidbody.useGravity = false;
                }
            }
        }
    }
    
    private void Update()
    {
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);

        if (inHandItem != null)
        {
            Vector3 targetPosition = playerCameraTransform.position + playerCameraTransform.forward * hitRange;
            RaycastHit obstacleHit;
            Collider inHandItemCollider = inHandItem.GetComponent<Collider>();
            float colliderExtent = inHandItemCollider.bounds.extents.magnitude;

            if (Physics.SphereCast(playerCameraTransform.position, colliderExtent, playerCameraTransform.forward, out obstacleHit, hitRange, groundLayerMask))
            {
                targetPosition = obstacleHit.point - playerCameraTransform.forward * colliderExtent;
            }

            // Calculate distance to target
            float distanceToTarget = Vector3.Distance(inHandItem.transform.position, targetPosition);

            // Adjust drag based on distance to target
            if (distanceToTarget < highDragDistance)
            {
                inHandItemRigidbody.linearVelocity = Vector3.zero;
                inHandItemRigidbody.linearDamping = highDrag;
                inHandItemRigidbody.angularDamping = highDrag / 8;
                
            }
            else
            {
                inHandItemRigidbody.linearDamping = lowDrag;
                inHandItemRigidbody.angularDamping = lowDrag;
                
                // Ensure the object moves smoothly towards the target position
                Vector3 targetPositionSmooth = Vector3.Lerp(inHandItem.transform.position, targetPosition, Time.deltaTime * 1); // Add smooth factor
                Vector3 moveDirection = (targetPositionSmooth - inHandItem.transform.position).normalized; // Calculate direction
                inHandItemRigidbody.linearVelocity = moveDirection * moveForce; // Use velocity for controlled movement
            }
            
            // Limit the velocity
            if (inHandItemRigidbody.linearVelocity.magnitude > maxVelocity)
            {
                inHandItemRigidbody.linearVelocity = inHandItemRigidbody.linearVelocity.normalized * maxVelocity;
            }

            if (Input.GetKeyDown (KeyCode.E)) {
                Drop ();
            }
            
            return;
        }

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickableLayerMask))
        {
            if (Input.GetKeyDown (KeyCode.E)) {
                PickUp ();
            }
        }
    }
}
