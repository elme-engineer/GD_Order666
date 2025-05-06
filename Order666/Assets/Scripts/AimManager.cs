using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class AimManager : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;

    [SerializeField] float mouseSensitivity = 1f;
    [SerializeField] float controllerSensitivity = 100f;

    private PlayerInputActions inputActions;
    private Vector2 lookInput;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnDisable()
    {
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= ctx => lookInput = Vector2.zero;
        inputActions.Disable();
    }

    void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        bool isGamepad = Gamepad.current != null && Gamepad.current.rightStick.ReadValue() != Vector2.zero;

        float xSensitivity = isGamepad ? controllerSensitivity : mouseSensitivity;
        float ySensitivity = isGamepad ? controllerSensitivity : mouseSensitivity;

        // Scale controller input by deltaTime to reduce sensitivity
        float x = lookInput.x * xSensitivity * Time.deltaTime;
        float y = lookInput.y * ySensitivity * Time.deltaTime;

        xAxis.m_InputAxisValue = x;
        yAxis.m_InputAxisValue = y;

        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        yAxis.Value = Mathf.Clamp(yAxis.Value, -80f, 80f);
    }

    void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
}
