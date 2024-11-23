using System.Collections.Generic;
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

    public bool _grounded;

    private InputSystem_Actions _inputActions;
    private Vector2 _moveInput;
    private bool _isJumping = false;
    private bool _isKillButtonPressed = false;

    public float MaxXSpeed = 1.0f;
    public float JumpForce = 2.0f;
    public float Gravity = 9.81f;

    [SerializeField]
    private float _killButtonHoldTime;

    //private <Vector2> _positionHistory = new List<Vector2>();


    private void Awake()
    {
        EnableObject();
        GetComponents();
        SetInputs();
    }

    private void Update()
    {
        if (state == State.Enabled)
        {
            HandleKillTime();
        }
        else if (state == State.Disabled)
        {
            DisabledDo();
        }
    }

    private void FixedUpdate()
    {
        if (state == State.Enabled)
        {
            HandleMovement();
        } 
    }

    private void GetComponents()
    {
        _rigidbdy2D = GetComponent<Rigidbody2D>();
    }

    private void EnableObject()
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
        if (context.performed)
        {
            _isJumping = true;
            Jump();
        }
        else if (context.canceled)
        {
            _isJumping = false;
        }
    }

    private void OnNew(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isKillButtonPressed = true;
            _killButtonHoldTime = 0f;
        }
        else if (context.canceled)
        {
            _isKillButtonPressed = false;
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

    private void DisabledDo()
    {
        _killButtonHoldTime -= Time.deltaTime;

        if (_killButtonHoldTime <= 0)
        {
            Die();
        }
        else
        {
            GoDark();
        }
    }

    private void HandleKillTime()
    {
        if (_isKillButtonPressed)
        {
            _killButtonHoldTime += Time.deltaTime;
        }
    }
    
    private void SpawnNewPlayer()
    {
        FreezeCurrentPlayer();

        //Instantiate(new Player(), new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void FreezeCurrentPlayer()
    {
        state = State.Disabled;
        OnDisable();
        _rigidbdy2D.bodyType = RigidbodyType2D.Static;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void GoDark()
    {

    }

    private void Jump()
    {
        if (_isJumping /*&& _grounded*/)
        {
            _rigidbdy2D.linearVelocityY = JumpForce;
        }
    }

    private void CheckGround()
    {
        
    }

    private void HandleMovement()
    {
        _rigidbdy2D.linearVelocity = new Vector2(_moveInput.x * MaxXSpeed, _rigidbdy2D.linearVelocityY);

        UpdateFacing();
    }

    private void UpdateFacing()
    {
        float direction = Mathf.Sign(_moveInput.x);
        transform.localScale = new Vector3(direction, 1, 1);
    }
}
