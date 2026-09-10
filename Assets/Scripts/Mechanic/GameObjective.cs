using UnityEngine;
using TMPro;

public class GameObjective : MonoBehaviour
{
    [Header("Objectives UI")]
    public GameObject objectiveList;
    public TMP_Text trashBagObjective;

    [Header("Trash Bags")]
    public int totalTrashBags = 5;

    private void Start()
    {
        objectiveList.SetActive(true);
    }

    private void Update()
    {
        UpdateTrashBagObjective();
    }

    private void UpdateTrashBagObjective()
    {
        GrabbableItem[] items =
            FindObjectsByType<GrabbableItem>(FindObjectsSortMode.None);

        int remainingTrashBags = 0;

        foreach (GrabbableItem item in items)
        {
            if (item.isTrashBag)
            {
                remainingTrashBags++;
            }
        }

        int thrownTrashBags =
            totalTrashBags - remainingTrashBags;

        thrownTrashBags =
            Mathf.Clamp(thrownTrashBags, 0, totalTrashBags);

        trashBagObjective.text =
            $"Throw Trash Bags {thrownTrashBags}/{totalTrashBags}";

        if (thrownTrashBags >= totalTrashBags)
        {
            trashBagObjective.color = Color.green;
        }
        else
        {
            trashBagObjective.color = Color.yellow;
        }
    }
}