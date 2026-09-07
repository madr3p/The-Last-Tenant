using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;

    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private GameInputActions inputActions;
    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
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
    // Ground
    if (controller.isGrounded && velocity.y < 0)
    {
        velocity.y = -2f;
    }

    // Jump
    if (inputActions.Player.Jump.WasPressedThisFrame() && controller.isGrounded)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    // Movement
    Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

    Vector3 move = transform.right * input.x + transform.forward * input.y;

    float currentSpeed = inputActions.Player.Sprint.IsPressed()
        ? sprintSpeed
        : walkSpeed;

    controller.Move(move * currentSpeed * Time.deltaTime);

    // Gravity
    velocity.y += gravity * Time.deltaTime;

    controller.Move(velocity * Time.deltaTime);
}
}