using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbdy2D;

    private InputSystem_Actions _inputActions;
    private Vector2 _moveInput;
    private bool _isJumping = false;
    private bool _isSprinting = false;
    private bool _isCrouching = false;

    public float MaxXSpeed = 1.0f;
    public float JumpForce = 5.0f;
    public float Gravity = 9.81f;


    private void Awake()
    {
        GetComponents();
        SetInputs();
    }

    private void Update()
    {
        _rigidbdy2D.linearVelocityX = _moveInput.x * MaxXSpeed;
    }

    private void GetComponents()
    {
        _rigidbdy2D = GetComponent<Rigidbody2D>();
    }

    private void SetInputs()
    {
        _inputActions = new InputSystem_Actions();

        _inputActions.Player.Move.started += onMove;
        _inputActions.Player.Move.performed += onMove;
        _inputActions.Player.Move.canceled += onMove;
    }

    private void onMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
}
