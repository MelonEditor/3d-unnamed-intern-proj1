using System;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickableLayerMask;
    
    private Transform playerCameraTransform;
    
    [SerializeField]
    [Min(1)]
    private float hitRange = 3;

    private GameObject inHandItem = null;
    private RaycastHit hit;

    //[SerializeField]
    //private AudioSource pickUpSource;

    private void Start()
    {
        playerCameraTransform = Camera.main.transform;
    }

    private void Drop()
    {
        if (inHandItem != null)
        {
            inHandItem.transform.SetParent(null);
            inHandItem = null;
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
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
                //pickUpSource.Play();
                inHandItem = pickableItem.PickUp();
                inHandItem.transform.position = playerCameraTransform.position + playerCameraTransform.forward * hitRange;
                inHandItem.transform.SetParent(transform, true);
            }
        }
    }
    
    private void Update()
    {
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);

        if (inHandItem != null)
        {
            inHandItem.transform.position = playerCameraTransform.position + playerCameraTransform.forward * hitRange;
            
            if (Input.GetKeyDown (KeyCode.E)) {
                Drop ();
            }
            
            return;
        }

        if (Physics.Raycast(
                playerCameraTransform.position, 
                playerCameraTransform.forward, 
                out hit, 
                hitRange, 
                pickableLayerMask))
        {
            if (Input.GetKeyDown (KeyCode.E)) {
                PickUp ();
            }
        }
    }
}
