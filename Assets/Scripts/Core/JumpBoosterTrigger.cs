using System;
using UnityEngine;

public class JumpBoosterTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public FPSController Player;
    public float jumpForceMultiplier;
    private float oldJumpForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oldJumpForce = Player.jumpForce;
            Player.jumpForce = jumpForceMultiplier * oldJumpForce;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.jumpForce = oldJumpForce;
        }
    }
}
