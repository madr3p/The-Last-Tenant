using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    public bool isGrabbed = false;
    public bool isTrashBag = false;

    public void Grab(Transform holdPoint)
    {
        isGrabbed = true;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void Drop()
    {
        isGrabbed = false;

        transform.SetParent(null);

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}