using UnityEngine;

public class GrowTrigger : MonoBehaviour
{
    public bool grow = true;
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) { return; }
        
        if (grow)
        {
            if (other.transform.localScale.x > 20f || other.transform.localScale.y > 20f ||
                other.transform.localScale.z > 20f) { return; }
            other.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        }
        else
        {
            if (other.transform.localScale.x < 0.3f || other.transform.localScale.y < 0.3f ||
                other.transform.localScale.z < 0.3f) { return; }
            
            other.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
