using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovmentUpdateMode { Update, FixedUpdate };
public enum MovmentUpdateExe { Transform, RigidBody };

public class PlayerDynaController : MonoBehaviour
{
    [SerializeField] private MovmentUpdateMode movmentUpdateMode;
    [SerializeField] private MovmentUpdateExe movmentUpdateExe;

    private Vector2 moveInputVector = new Vector2();
    private Vector3 currentPosition = new Vector3();

    [SerializeField] private float defaultSpeed = 3f;

    [SerializeField] private Rigidbody playerRigidBody;

    private void Update()
    {
        if (movmentUpdateMode == MovmentUpdateMode.Update)
        {
            UpdateMovement(Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (movmentUpdateMode == MovmentUpdateMode.FixedUpdate)
        {
            UpdateMovement(Time.fixedDeltaTime);
        }
    }

    private void UpdateMovement(float deltaTime)
    {
        moveInputVector.x = Input.GetAxisRaw("Horizontal");
        moveInputVector.y = Input.GetAxisRaw("Vertical");

        switch (movmentUpdateExe)
        {
            case MovmentUpdateExe.Transform:
                UpdateTransform(deltaTime);
            break;
            case MovmentUpdateExe.RigidBody:
                UpdateRigidBody();
            break;
        }
        
    }

    private void UpdateTransform(float deltaTime)
    {
        currentPosition = transform.position;

        currentPosition.x += moveInputVector.x * deltaTime * defaultSpeed;
        currentPosition.z += moveInputVector.y * deltaTime * defaultSpeed;

        transform.position = currentPosition;
    }

    private void UpdateRigidBody()
    {
        currentPosition.x = moveInputVector.x * defaultSpeed;
        currentPosition.y = 0f;
        currentPosition.z = moveInputVector.y * defaultSpeed;

        playerRigidBody.velocity = currentPosition;
    }
}
