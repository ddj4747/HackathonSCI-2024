using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbdy2D;

    public enum State
    {
        Enabled,
        Disabled
    }

    public State state;

    //private bool _freezed = false;

    private bool _grounded;

    private InputSystem_Actions _inputActions;
    private Vector2 _moveInput;
    private bool _isJumping = false;

    public float MaxXSpeed = 1.0f;
    public float JumpForce = 2.0f;
    public float Gravity = 9.81f;

    private float _timeTillDie;


    private void Awake()
    {
        TurnOn();
        GetComponents();
        SetInputs();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if(state == State.Enabled)
        {
            HandleMovement();
        } else if(state == State.Disabled)
        {
            return;
        }
    }

    private void GetComponents()
    {
        _rigidbdy2D = GetComponent<Rigidbody2D>();
    }

    private void TurnOn()
    {
        state = State.Enabled;
    }

    private void SetInputs()
    {
        _inputActions = new InputSystem_Actions();

        _inputActions.Player.Move.started += OnMove;
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
        _inputActions.Player.Jump.performed += OnJump;
        _inputActions.Player.Jump.canceled += OnJump;
        _inputActions.Player.SpawnNew.performed += OnNew;
        _inputActions.Player.SpawnNew.canceled += OnNew;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _isJumping = true;
            Jump();
        }
        else
        {
            _isJumping = false;
        }
    }

    private void OnNew(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SpawnNewPlayer();
        }
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void Jump()
    {
        if (_isJumping /*&& _grounded*/)
        {
            _rigidbdy2D.linearVelocityY = JumpForce;
        }
    }

    private void HandleMovement()
    {
        _rigidbdy2D.linearVelocity = new Vector2(_moveInput.x * MaxXSpeed, _rigidbdy2D.linearVelocityY);
    }

    private void SpawnNewPlayer()
    {
        //Instantiate(new Player(), new Vector3(0, 0, 0), Quaternion.identity);

        FreezeCurrentPlayer();
    }

    private void FreezeCurrentPlayer()
    {
        state = State.Disabled;
        OnDisable();
        _rigidbdy2D.bodyType = RigidbodyType2D.Static;
    }

    
}
