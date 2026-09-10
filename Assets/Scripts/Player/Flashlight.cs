using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;

    private GameInputActions inputActions;

    private void Awake()
    {
        inputActions = new GameInputActions();
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
        if (inputActions.Player.Flashlight.WasPressedThisFrame())
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}