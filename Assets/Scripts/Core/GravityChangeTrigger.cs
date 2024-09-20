using System;
using UnityEngine;

public class GravityChangeTrigger : MonoBehaviour
{
    public FPSController Player;
    public Vector3 GravityDirection;
    private void OnTriggerEnter(Collider other)
    {
//        if (other.CompareTag("Player"))
        {
            Player.gravityDirection = GravityDirection;
        }
    }

}
