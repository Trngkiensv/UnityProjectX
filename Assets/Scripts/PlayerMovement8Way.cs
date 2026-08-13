using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement8Way : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;

    private Vector2 moveInput;
    private float verticalVelocity;

    public Vector2 LastMoveDirection { get; private set; } = Vector2.down;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ReadInput();

        Vector3 moveDirection = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        if (moveInput.sqrMagnitude > 0.01f)
        {
            LastMoveDirection = moveInput.normalized;
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity =
            moveDirection * moveSpeed;

        velocity.y = verticalVelocity;

        controller.Move(
            velocity * Time.deltaTime
        );
    }

    private void ReadInput()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
        {
            moveInput = Vector2.zero;
            return;
        }

        float x = 0f;
        float y = 0f;

        if (keyboard.aKey.isPressed ||
            keyboard.leftArrowKey.isPressed)
        {
            x -= 1f;
        }

        // RIGHT
        if (keyboard.dKey.isPressed ||
            keyboard.rightArrowKey.isPressed)
        {
            x += 1f;
        }

        // DOWN
        if (keyboard.sKey.isPressed ||
            keyboard.downArrowKey.isPressed)
        {
            y -= 1f;
        }

        // UP
        if (keyboard.wKey.isPressed ||
            keyboard.upArrowKey.isPressed)
        {
            y += 1f;
        }

        moveInput = new Vector2(x, y);

        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput.Normalize();
        }
    }
}