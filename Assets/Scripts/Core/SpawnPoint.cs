using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private FPSController player;
    public AudioSource spawnSound;

    private void Start()
    {
        player = FindAnyObjectByType<FPSController>();
        if (player == null)
        {
            Debug.LogError("FPSController not found in the scene!");
        }
    }
    public Vector3 gravityDirection;
    private void OnCollisionEnter(Collision other)
    {
        if(player.SpawnPoint == this){
            return;
        }
        if (player != null)
        {
            gravityDirection = player.gravityDirection;
            player.SpawnPoint = this;
            spawnSound.Play();
        }
        else
        {
            Debug.LogWarning("Player reference is null. Cannot apply spawn point.");
        }
    }
}
