using System;
using UnityEngine;

public class JumpBoosterTrigger : MonoBehaviour
{
    private FPSController player;
    public float jumpForceMultiplier;
    private float oldJumpForce;

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
        if (other.CompareTag("Player"))
        {
            oldJumpForce = player.jumpForce;
            player.jumpForce = jumpForceMultiplier * oldJumpForce;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.jumpForce = oldJumpForce;
        }
    }
}
