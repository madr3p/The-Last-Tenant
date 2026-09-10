using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction")]
    public float interactDistance = 10f;

    [Header("UI")]
    public GameObject crosshair;

    private GameInputActions inputActions;
    private Camera playerCamera;
    private PlayerGrab playerGrab;
    private NPCDialogue npcDialogue;

    private void Awake()
    {
        inputActions = new GameInputActions();
        playerCamera = Camera.main;
        playerGrab = GetComponent<PlayerGrab>();
        npcDialogue = GetComponent<NPCDialogue>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        CheckInteraction();

        if (inputActions.Player.Interact.WasPressedThisFrame())
        {
            TryInteract();
        }

        if (inputActions.Player.Drop.WasPressedThisFrame())
        {
            playerGrab.Drop();
        }
    }

    private void CheckInteraction()
    {
        // Let NPCDialogue handle the UI when looking at an NPC
        if (npcDialogue != null && npcDialogue.IsLookingAtNPC())
        {
            return;
        }

        // Holding an item
        if (playerGrab.IsHolding())
        {
            crosshair.SetActive(true);
            playerGrab.UpdateUI(false);
            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        bool lookingAtItem = false;

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            GrabbableItem item =
                hit.collider.GetComponentInParent<GrabbableItem>();

            if (item != null)
            {
                lookingAtItem = true;
            }
        }

        crosshair.SetActive(!lookingAtItem);

        playerGrab.UpdateUI(lookingAtItem);
    }

    private void TryInteract()
    {
        // NPCDialogue handles NPC interaction
        if (npcDialogue != null && npcDialogue.IsLookingAtNPC())
        {
            return;
        }

        if (playerGrab.IsHolding())
        {
            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            GrabbableItem item =
                hit.collider.GetComponentInParent<GrabbableItem>();

            if (item != null)
            {
                playerGrab.Grab(item);
            }
        }
    }
}
