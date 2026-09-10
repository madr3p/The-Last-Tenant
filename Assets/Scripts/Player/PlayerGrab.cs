using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Grab")]
    public Transform holdPoint;

    [Header("UI")]
    public GameObject grabPrompt;
    public GameObject dropPrompt;

    private GrabbableItem heldItem;

    public bool IsHolding()
    {
        return heldItem != null;
    }

    public GrabbableItem GetHeldItem()
{
    return heldItem;
}

public void ClearHeldItem()
{
    heldItem = null;
}

    public void Grab(GrabbableItem item)
    {
        if (heldItem != null)
            return;

        heldItem = item;

        item.Grab(holdPoint);

        grabPrompt.SetActive(false);
    }

    public void Drop()
    {
        if (heldItem == null)
            return;

        heldItem.Drop();
        heldItem = null;
    }

    public void UpdateUI(bool lookingAtItem)
    {
        if (heldItem != null)
        {
            grabPrompt.SetActive(false);
            dropPrompt.SetActive(true);
            return;
        }

        grabPrompt.SetActive(lookingAtItem);
        dropPrompt.SetActive(false);
    }
}