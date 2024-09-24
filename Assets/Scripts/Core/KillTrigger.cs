using System;
using UnityEngine;

public class KillTrigger : MonoBehaviour
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

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (player != null)
        {
            player.ReturnToSpawnPoint();
        }
        else
        {
            Debug.LogWarning("Player reference is null. Cannot return to spawn point.");
        }
    }
}
