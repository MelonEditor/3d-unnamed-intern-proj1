using UnityEngine;

public class Pickable : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update(){
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 20);
    }
    public GameObject PickUp()
    {
        if (rb != null)
        {
            //rb.isKinematic = true;
        }

        //transform.position = Vector3.zero;
        //transform.rotation = Quaternion.identity;
        return this.gameObject;
    }
}
