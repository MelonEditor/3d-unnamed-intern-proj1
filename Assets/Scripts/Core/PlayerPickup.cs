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

    private Collider playerCollider;
    //[SerializeField]
    //private AudioSource pickUpSource;

    private void Start()
    {
        playerCameraTransform = Camera.main.transform;
        playerCollider = GetComponent<Collider>();
    }

    private void Drop()
    {
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
        }
    }
    private void PickUp()
    {
        if(hit.collider != null && inHandItem == null)
        {
            Debug.Log("Picking up");
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
                inHandItem.transform.position = obstacleHit.point - playerCameraTransform.forward;
            }
            else
            {
                inHandItem.transform.position = targetPosition;
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
