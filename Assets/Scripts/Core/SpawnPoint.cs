using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public FPSController Player;
    public Vector3 gravityDirection;
    private void OnCollisionEnter(Collision other)
    {
        gravityDirection = Player.gravityDirection;
        Player.SpawnPoint = this;
    }
}
