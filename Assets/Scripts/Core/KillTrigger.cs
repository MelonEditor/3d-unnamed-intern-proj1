using System;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    public FPSController Player;
    private void OnCollisionEnter(Collision other)
    {
        Player.ReturnToSpawnPoint();
    }
}
