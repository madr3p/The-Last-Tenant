using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float grabDistance = 3f;
    public GameObject interactPrompt;
    public Transform holdPoint;

    private GameInputActions inputActions;
    private Camera playerCamera;

    private void Awake()
    {
        inputActions = new GameInputActions();
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

    private void Update()
    {
        if (inputActions.Player.Drop.WasPressedThisFrame())
{
    GrabbableItem[] items = GetComponentsInChildren<GrabbableItem>();

    foreach (GrabbableItem item in items)
    {
        if (item.isGrabbed)
        {
            item.Drop();
            break;
        }
    }
}

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                interactPrompt.SetActive(false);
                return;
            }

            GrabbableItem item = hit.collider.GetComponent<GrabbableItem>();

            if (item != null && !item.isGrabbed)
            {
                interactPrompt.SetActive(true);

                if (inputActions.Player.Interact.IsPressed())
                {
                    item.Grab(holdPoint);
                    interactPrompt.SetActive(false);
                }

                return;
            }
        }

        interactPrompt.SetActive(false);
    }
}