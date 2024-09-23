using System;
using UnityEngine;

public class GravityChangeTrigger : MonoBehaviour
{
    private FPSController player;

    private void Start()
    {
        player = FindAnyObjectByType<FPSController>();
        if (player == null)
        {
            Debug.LogError("FPSController not found in the scene!");
        }
    }

    public Vector3 GravityDirection;
    private void OnTriggerEnter(Collider other)
    {
//        if (other.CompareTag("Player"))
        {
            player.gravityDirection = GravityDirection;
        }
    }

}
