using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterMovementController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterCamera;

    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float doubleSpeed = 6f;

    private float currentSpeed;
    private Vector3 inputVector;
    private Vector3 localVector;

    private float targetAngle;
    private float smoothedTargetAngle;
    private float smoothedTargetVelocity;
    [SerializeField] private float smoothedTargetAngleTime = 1f;

    private float currentSpeedVelocity;
    [SerializeField] private float currentSpeedTime = 0.25f;

    private Vector3 currentMovement;
    private Vector3 currentMovementVelocity;
    [SerializeField] private float currentMovementTime = 0.25f;

    [Header("Animations")]
    [SerializeField] private Animator animator;
    private Vector3 currentVelocity;
    private Vector3 currentVelocityLocal;
    private float speedRatio;
    [SerializeField] private float animDirMuliplier = 0.2f;

    [Header("Aim")]
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    private CinemachinePOV povAim;
    //[SerializeField] private float aimSensitivityYaw = 100f; 
    //[SerializeField] private float aimSensitivityPitch = 100f;
    private bool isAiming;
    //private float aimTargetYaw;
    //private float aimTargetPitch;
    //[SerializeField] private Vector3 aimCameraRotOffset;
    [SerializeField] private GameObject aimCrosshair;
    [SerializeField] private float aimingRange = 30f;
    //[SerializeField] private float aimingRotationOffset = 5f;

    private void OnEnable()
    {
        currentSpeed = normalSpeed;

        povAim = aimCamera.GetCinemachineComponent<CinemachinePOV>();

        if (povAim == null)
        {
            Debug.LogError("povAim == null !");
        }
        
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        SetRunState();

        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (!isAiming && inputVector.magnitude > 0.1f)
        {
            targetAngle = Mathf.Atan2(inputVector.x, inputVector.z) * Mathf.Rad2Deg + characterCamera.eulerAngles.y;
            smoothedTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothedTargetVelocity, smoothedTargetAngleTime);

            transform.rotation = Quaternion.Euler(0f, smoothedTargetAngle, 0f);

            localVector = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            localVector = Vector3.zero;
        }

        currentMovement = Vector3.SmoothDamp(currentMovement, localVector.normalized, ref currentMovementVelocity, currentMovementTime);
        characterController.Move(currentMovement * Time.deltaTime * currentSpeed);

        SetAimCameraState();

        UpdateAnimations();
    }

    private void SetRunState()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, doubleSpeed, ref currentSpeedVelocity, currentSpeedTime);
        }
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, normalSpeed, ref currentSpeedVelocity, currentSpeedTime);
        }
    }

    private void SetAimCameraState()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            IsAiming = true;

            SetAimPose();
            SetAimRotation();
        }
        else
        {
            IsAiming = false;
        }
    }

    private bool IsAiming
    {
        set
        {
            if (!isAiming && value)
            {
                aimCamera.gameObject.SetActive(true);
                aimCrosshair.SetActive(true);

                povAim.m_HorizontalAxis.Value = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
                povAim.m_VerticalAxis.Value = 0f;

                povAim.m_HorizontalAxis.m_MinValue = povAim.m_HorizontalAxis.Value - aimingRange;
                povAim.m_HorizontalAxis.m_MaxValue = povAim.m_HorizontalAxis.Value + aimingRange;

                animator.SetBool("isAiming", true);
            }
            else if (isAiming && !value)
            {
                aimCamera.gameObject.SetActive(false);
                aimCrosshair.SetActive(false);

                animator.SetBool("isAiming", false);
            }

            isAiming = value;
        }
    }

    private void SetAimPose()
    {
        animator.SetFloat("aimPose", 0.5f - povAim.m_VerticalAxis.Value / (povAim.m_VerticalAxis.m_MaxValue - povAim.m_VerticalAxis.m_MinValue));
    }

    private void SetAimRotation()
    {
        //targetAngle = povAim.m_HorizontalAxis.Value + aimingRotationOffset;
        targetAngle = povAim.m_HorizontalAxis.Value;
        smoothedTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothedTargetVelocity, smoothedTargetAngleTime);

        transform.rotation = Quaternion.Euler(0f, smoothedTargetAngle, 0f);
    }

    /*
    private void RotateAimCamera()
    {
        aimTargetYaw = Mathf.Clamp(aimTargetYaw + Input.GetAxis("Mouse Y") * Time.deltaTime * aimSensitivityYaw, -30f, 30f);
        aimTargetPitch = Mathf.Clamp(aimTargetPitch + Input.GetAxis("Mouse X") * Time.deltaTime * aimSensitivityPitch, float.MinValue, float.MaxValue) ;

        aimCamera.transform.rotation = Quaternion.Euler(aimTargetYaw + aimCameraRotOffset.x, aimTargetPitch + aimCameraRotOffset.y, aimCameraRotOffset.z);
    }
    */

    private void UpdateAnimations()
    {
        if (animator == null)
        {
            return;
        }

        currentVelocity = characterController.velocity;
        currentVelocityLocal = transform.InverseTransformVector(currentVelocity);

        //

        //animator.SetFloat("leftRight", currentVelocityLocal.x);
        //animator.SetFloat("backwardForward", currentVelocityLocal.z);
        animator.SetFloat("leftRight", currentVelocityLocal.x * animDirMuliplier);
        animator.SetFloat("backwardForward", currentVelocityLocal.z * animDirMuliplier);

        speedRatio = Mathf.Clamp01((currentVelocity.magnitude - normalSpeed) / (doubleSpeed - normalSpeed));

        animator.SetFloat("speed", speedRatio);
    }
}
