using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    public float throwDistance = 3f;

    [Header("UI")]
    public GameObject throwPrompt;
    public GameObject dropPrompt;
    public GameObject crosshair;

    private GameInputActions inputActions;
    private PlayerGrab playerGrab;
    private Camera playerCamera;

    private void Awake()
    {
        inputActions = new GameInputActions();
        playerGrab = GetComponent<PlayerGrab>();
        playerCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void LateUpdate()
    {
        if (!playerGrab.IsHolding())
        {
            throwPrompt.SetActive(false);
            return;
        }

        CheckForDumpster();
    }

    private void CheckForDumpster()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        RaycastHit[] hits = Physics.RaycastAll(ray, throwDistance);

        foreach (RaycastHit hit in hits)
        {
            Dumpster dumpster =
                hit.collider.GetComponentInParent<Dumpster>();

            if (dumpster != null)
            {
                throwPrompt.SetActive(true);
                dropPrompt.SetActive(false);
                crosshair.SetActive(false);

                if (inputActions.Player.Interact.WasPressedThisFrame())
                {
                    ThrowTrashBag();
                }

                return;
            }
        }

        throwPrompt.SetActive(false);
    }

    private void ThrowTrashBag()
    {
        GrabbableItem heldItem = playerGrab.GetHeldItem();

        if (heldItem == null)
            return;

        if (!heldItem.isTrashBag)
            return;

        Destroy(heldItem.gameObject);

        playerGrab.ClearHeldItem();

        throwPrompt.SetActive(false);
        dropPrompt.SetActive(false);
        crosshair.SetActive(true);
    }
}