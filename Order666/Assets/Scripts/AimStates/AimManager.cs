using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Animations.Rigging;

public class AimManager : MonoBehaviour
{

    AimBaseState currentState;
    public HipFireState hip = new HipFireState();
    public AimState aim = new AimState();

    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;

    [SerializeField] float mouseSensitivity = 1f;
    [SerializeField] float controllerSensitivity = 100f;

    [HideInInspector] public CinemachineVirtualCamera virtualCamera;
    public float aimFov = 40;
    [HideInInspector] public float hipFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10;

    [SerializeField] Transform aimPos;
    [SerializeField] float aimSmoothSpeed = 20;
    [SerializeField] LayerMask aimMask;

    [HideInInspector] public Animator animator;

    [SerializeField] private MultiAimConstraint handAimConstraint;
    [SerializeField] private TwoBoneIKConstraint twoBoneIKConstraint;
    [SerializeField] private GameObject catWeapon;

    private PlayerInputActions inputActions;
    private Vector2 lookInput;

    private bool isAiming = false;
    private bool isShooting = false;

    private bool hasGun = false;
    private bool isGunMode = false;


    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void Start()
    {

        catWeapon.SetActive(false);
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        hipFov = virtualCamera.m_Lens.FieldOfView;
        animator = GetComponent<Animator>();

        animator.SetLayerWeight(animator.GetLayerIndex("Shooting"), 0); // Disable shooting layer
        SwitchState(hip);        
    }

    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        inputActions.Player.Aim.started += OnAimStarted;
        inputActions.Player.Aim.canceled += OnAimCanceled;

        inputActions.Player.Shoot.performed += OnShootPerformed;
        inputActions.Player.Shoot.canceled += OnShootCanceled;

        inputActions.Player.SwitchMode.performed += OnSwitchMode;

    }

    void OnDisable()
    {
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= ctx => lookInput = Vector2.zero;

        inputActions.Player.Aim.started -= OnAimStarted;
        inputActions.Player.Aim.canceled -= OnAimCanceled;

        inputActions.Player.Shoot.performed -= OnShootPerformed;
        inputActions.Player.Shoot.canceled -= OnShootCanceled;
        inputActions.Player.SwitchMode.performed -= OnSwitchMode;


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

        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);

        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);

        UpdateConstraints();

        currentState.UpdateState(this);
    }

    void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {

        currentState = state;
        currentState.EnterState(this);
    }

    void OnAimStarted(InputAction.CallbackContext context)
    {
        if (!hasGun || !isGunMode) return;
        isAiming = true;
        SwitchState(aim);
    }

    void OnAimCanceled(InputAction.CallbackContext context)
    {
        if (!hasGun || !isGunMode) return;
        isAiming = false;
        SwitchState(hip);
    }

    void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (!hasGun || !isGunMode) return;

        isShooting = true;
        if (isAiming)
        {

            
            // animator.SetTrigger("Shoot"); // Optional: call actual shooting logic
            // e.g., FireBullet();
        }
    }

    void OnShootCanceled(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    private void OnSwitchMode(InputAction.CallbackContext context)
    {
        if (!hasGun) return;

        isGunMode = !isGunMode;

        // Toggle the Shooting animation layer
        int shootingLayer = animator.GetLayerIndex("Shooting");
        animator.SetLayerWeight(shootingLayer, isGunMode ? 1 : 0);
        catWeapon.SetActive(isGunMode ? true : false);
    }

    void UpdateConstraints()
    {
        if (handAimConstraint == null || twoBoneIKConstraint == null) {

            Debug.Log("No hand aim constraint or twoBoneIK constraint");
            return;
        }


        handAimConstraint.weight = hasGun && isGunMode ? 1f : 0f;
        twoBoneIKConstraint.weight = hasGun && isGunMode ? 1f : 0f;

    }

    public void UnlockGunMode()
    {
        hasGun = true;
        isGunMode = true;
        int shootingLayer = animator.GetLayerIndex("Shooting");
        animator.SetLayerWeight(shootingLayer, 1);
        catWeapon.SetActive(true);
        Debug.Log("Gun mode unlocked!");
    }


}
