using System;
using UnityEngine;

public class GravityChangeTrigger : MonoBehaviour
{
    private FPSController player;
    public AudioSource gravityChangeSound;
    public Vector3 GravityDirection;
    private void Start()
    {
        player = FindAnyObjectByType<FPSController>();
        if (player == null)
        {
            Debug.LogError("FPSController not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        if(player.gravityDirection == GravityDirection){
            return;
        }
        if (player != null)
        {
            player.CameraRotationTransition(GravityDirection);
            gravityChangeSound.Play();
        }
    }
}
