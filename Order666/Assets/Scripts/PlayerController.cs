using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    [SerializeField] float jumpHeight = 3f;

    [SerializeField] Transform cam;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += OnJump;

    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Disable();
        inputActions.Player.Jump.performed -= OnJump;

    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        // Only horizontal movement (ignore vertical camera tilt)
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = (camForward * moveInput.y + camRight * moveInput.x);
        controller.Move(move * speed * Time.deltaTime);

        Gravity();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }


    bool IsGrounded() 
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask))
            return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded())
            velocity.y += gravity * Time.deltaTime;
        else if(velocity.y < 0)
            velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }
}
