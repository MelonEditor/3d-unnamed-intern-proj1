using UnityEngine;

public class GrowTrigger : MonoBehaviour
{
    public bool grow = true;
    private void OnCollisionStay(Collision other)
    {
        if (grow)
        {
            other.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        }
        else
        {
            other.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
