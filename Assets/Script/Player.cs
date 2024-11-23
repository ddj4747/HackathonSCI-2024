using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;

    private bool _grounded;

    private InputSystem_Actions _inputActions;
    private Vector2 _moveInput;
    private bool _isJumping = false;
    private bool _isKillButtonPressed = false;

    public float MaxXSpeed = 1.0f;
    public float JumpForce = 2.0f;
    public float Gravity = 9.81f;

    [SerializeField]
    private float _killButtonHoldTime;
    [SerializeField]
    private GameObject playerShadow;
    private PlayerShadow playerShadowScript;

    public Queue<Vector3> PositionHistory;
    public float SavePositionTime = 0.25f;
    private float _timeSinceLastSave = 0f;
    public int MaxHistorySize = 20;

    public float TimeToNextPosition = 0.5f;
    private float _timerToNextPosition = 0f;
    public bool ReturnedBack = false;
    public bool Rewinding = false;

    private void Awake()
    {
        GetComponents();
        SetInputs();
    }

    private void Start()
    {
        OnEnable();
    }

    private void Update()
    {
        HandleKillTime();
        HandleRewindTime();
        HandleSavingPosition();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void GetComponents()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        PositionHistory = new Queue<Vector3>();
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
            CreateShadow();
            Rewinding = true;
        }playerShadow
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void HandleKillTime()
    {
        if (_isKillButtonPressed)
        {
            _killButtonHoldTime += Time.deltaTime;
        }
    }

    private void HandleSavingPosition()
    {
        _timeSinceLastSave += Time.deltaTime;

        if (_timeSinceLastSave >= SavePositionTime && _moveInput.x != 0)
        {
            SavePosition();
            _timeSinceLastSave = 0f;
        }
    }

    private void SavePosition()
    {
        PositionHistory.Enqueue(transform.position);

        if (PositionHistory.Count >= MaxHistorySize)
        {
            PositionHistory.Dequeue();
        }
    }

    public void CreateShadow()
    {
        if (playerShadow != null)
        {
            playerShadow = Instantiate(playerShadow, transform.position, Quaternion.identity);
            playerShadowScript = playerShadow.GetComponent<PlayerShadow>();
            playerShadowScript.GoDie = false;
            playerShadowScript.TimeToDie = _killButtonHoldTime;
        }
    }

    private void Jump()
    {
        if (_isJumping /*&& _grounded*/)
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, JumpForce);
        }
    }

    private void CheckGround()
    {
        // Tutaj sprawdź, czy gracz jest na ziemi, używając np. raycastu
    }

    private void HandleMovement()
    {
        _rigidbody2D.linearVelocity = new Vector2(_moveInput.x * MaxXSpeed, _rigidbody2D.linearVelocity.y);

        UpdateFacing();
    }

    private void UpdateFacing()
    {
        float direction = Mathf.Sign(_moveInput.x);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void HandleRewindTime()
    {
        if (Rewinding)
        {
            int rewindCount = Mathf.FloorToInt(_killButtonHoldTime / SavePositionTime);
            rewindCount = Mathf.Min(rewindCount, PositionHistory.Count); 

            if (rewindCount > 0)
            {
                for (int i = 0; i < rewindCount; i++)
                {
                    Vector3 currPosition = Vector3.zero;
                    if (PositionHistory.Count > 0)
                    {
                        currPosition = PositionHistory.Peek();
                        transform.position = currPosition;
                        PositionHistory.Dequeue();
                    }
                    if (transform.position == currPosition)
                    {
                        Rewinding = false;
                        playerShadowScript.GoDie = true;
                        break;
                    }
                }
            }
            
        }
        
    }
}