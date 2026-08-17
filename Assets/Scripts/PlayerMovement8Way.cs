using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement8Way : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private CharacterController controller;

    private Vector2 moveInput;
    private Vector2 lastFacingDirection = Vector2.down;

    private float verticalVelocity;

    private static readonly int MoveX =
        Animator.StringToHash("MoveX");

    private static readonly int MoveY =
        Animator.StringToHash("MoveY");

    private static readonly int IsMoving =
        Animator.StringToHash("IsMoving");

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        ReadInput();
        MovePlayer();
        UpdateAnimation();
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

        if (keyboard.dKey.isPressed ||
            keyboard.rightArrowKey.isPressed)
        {
            x += 1f;
        }

        if (keyboard.sKey.isPressed ||
            keyboard.downArrowKey.isPressed)
        {
            y -= 1f;
        }

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

    private void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );

        if (controller.isGrounded &&
            verticalVelocity < 0f)
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

    private void UpdateAnimation()
    {
        bool isMoving =
            moveInput.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            // Horizontal animation has priority during diagonal movement.
            if (Mathf.Abs(moveInput.x) > 0.01f)
            {
                lastFacingDirection =
                    new Vector2(
                        Mathf.Sign(moveInput.x),
                        0f
                    );
            }
            else
            {
                lastFacingDirection =
                    new Vector2(
                        0f,
                        Mathf.Sign(moveInput.y)
                    );
            }
        }

        animator.SetFloat(
            MoveX,
            lastFacingDirection.x
        );

        animator.SetFloat(
            MoveY,
            lastFacingDirection.y
        );

        animator.SetBool(
            IsMoving,
            isMoving
        );
    }
}