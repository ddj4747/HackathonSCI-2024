using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

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

    private Queue<Vector3> _positionHistory;

    private void Awake()
    {
        GetComponents();
        SetInputs();
    }

    private void Start()
    {
        OnEnable();
    }

    private float elapsed = 0f;

    private void Update()
    {
        elapsed += Time.deltaTime; 

        if (elapsed > 0.05f)
        {
            PositionHistory.Enqueue(transform.position);
            elapsed -= 0.1f;
        }
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
            PositionHistory.Clear();
        }
        else if (context.canceled)
        {
            GameObject sh = playerShadow.gameObject;

            Instantiate(sh, transform.position, Quaternion.identity);

            PlayerShadow cp = sh.GetComponent<PlayerShadow>();

            cp.TimeToDie = 5;
            cp.GoDie = true;

            transform.position = PositionHistory.Dequeue();
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

}