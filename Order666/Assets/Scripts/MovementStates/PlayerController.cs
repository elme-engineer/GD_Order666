using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float currentSpeed = 5f;
    public float walkSpeed = 3, walkBackSpeed = 2;
    public float runSpeed = 7, runBackSpeed = 5;
    public float crouchSpeed = 2, crouchBackSpeed = 1;

    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] float jumpForce = 3f;
    [HideInInspector] public bool jumped;
    private Vector3 velocity;
    private bool hasJumpedThisFrame = false;

    [Header("Ground Check")]
    [SerializeField] private float groundYOffset;
    [SerializeField] private LayerMask groundMask;
    private Vector3 spherePos;

    [Header("Camera")]
    [SerializeField] private Transform cam;

    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Animator animator;

    // Input system
    private PlayerInputActions inputActions;
    public PlayerInputActions.PlayerActions playerInput;
    private Vector2 moveInput;

    // Movement info
    [HideInInspector] public float hzInput, vInput;
    [HideInInspector] public Vector3 dir;

    // State management
    public MovementBaseState previousState;
    private MovementBaseState currentState;
    public bool crouchToggleEnabled = false; // This could later be updated by a game settings menu
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();
    public JumpState Jump = new JumpState();

    private static readonly int HzInputHash = Animator.StringToHash("hzInput");
    private static readonly int VInputHash = Animator.StringToHash("vInput");

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        playerInput = inputActions.Player;
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Move.performed += OnMove;
        playerInput.Move.canceled += ctx => moveInput = Vector2.zero;

    }

    private void OnDisable()
    {
        playerInput.Move.performed -= OnMove;
        playerInput.Move.canceled -= ctx => moveInput = Vector2.zero;
        
        playerInput.Disable();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        SwitchState(Idle);
    }

    private void Update()
    {
        GetDirectionAndMove();
        ApplyGravity();

        animator.SetFloat(HzInputHash, hzInput);
        animator.SetFloat(VInputHash, vInput);

        currentState.UpdateState(this);
    }

    private void LateUpdate()
    {
        hasJumpedThisFrame = false;
    }

    private void GetDirectionAndMove()
    {
        hzInput = moveInput.x;
        vInput = moveInput.y;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        dir = camForward * vInput + camRight * hzInput;
        dir.Normalize();

        Vector3 move = dir * currentSpeed;
        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void JumpForce()
    {
        if (hasJumpedThisFrame) return;

        velocity.y = jumpForce;
        hasJumpedThisFrame = true;

    }
    public void Jumped() => jumped = true;

    private void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    public void SwitchState(MovementBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    public bool IsMoving()
    {
        return dir.magnitude > 0.1f;
    }

    public bool IsSprintPressed()
    {
        return Keyboard.current.leftShiftKey.isPressed || Gamepad.current?.leftStickButton.isPressed == true;
    }

    public bool IsJumpPressed()
    {
        
        return playerInput.Jump.triggered;
        
    }

    public bool IsCrouchPressed()
    {
        return Keyboard.current.leftCtrlKey.isPressed || Gamepad.current?.buttonEast.isPressed == true;
    }

    public bool IsCrouchInitiated()
    {
        // This handles both toggle and hold detection
        if (crouchToggleEnabled)
            return Keyboard.current.leftCtrlKey.wasPressedThisFrame || Gamepad.current?.buttonEast.wasPressedThisFrame == true;
        
        return IsCrouchPressed();
    }

}
