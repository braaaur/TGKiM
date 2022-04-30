using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnEnable()
    {
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, doubleSpeed, ref currentSpeedVelocity, currentSpeedTime);
        }
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, normalSpeed, ref currentSpeedVelocity, currentSpeedTime);
        }
        
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (inputVector.magnitude > 0.1f)
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

        UpdateAnimations();
    }

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
