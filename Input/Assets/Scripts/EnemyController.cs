using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Transform[] waypoints;

    [SerializeField] private NavigatorController navigatorController;

    [SerializeField] private Transform player;

    private Vector3 currentDirection;
    private float currentAngle;
    [SerializeField] private float tresholdAngle = 30f;
    private bool isPlayerVisible;

    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float chaseSpeed = 5f;

    private void Update()
    {
        UpdateBehavior();
        CheckSeePlayer();
    }

    private void UpdateBehavior()
    {
        if (IsPlayerVisible)
        {
            CheckChase();
        }
        else
        {
            CheckDistance();
        }
    }

    private void CheckChase()
    {
        navigatorController.MoveTo(player.position);
        navigatorController.MeshAgent.speed = chaseSpeed;
    }

    private void CheckDistance()
    {
        navigatorController.MeshAgent.speed = normalSpeed;

        if (navigatorController.MeshAgent.remainingDistance <= navigatorController.MeshAgent.stoppingDistance)
        {
            TargetReached();
        }
    }

    private void TargetReached()
    {
        navigatorController.MoveTo(waypoints[Random.Range(0, waypoints.Length)].position);
    }

    private void CheckSeePlayer()
    {
        currentDirection = player.position - transform.position;


        if (Physics.Raycast(transform.position, currentDirection, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.transform == player)
            {
                currentAngle = Vector3.Angle(transform.forward, currentDirection);

                if (currentAngle <= tresholdAngle)
                {
                    IsPlayerVisible = true;

                    return;
                }
            }
        }

        IsPlayerVisible = false;
    }

    private bool IsPlayerVisible
    {
        set
        {
            isPlayerVisible = value;

            if (value)
            {
                meshRenderer.material.color = Color.red;
            }
            else
            {
                meshRenderer.material.color = Color.blue;
            }
        }
        get
        {
            return isPlayerVisible;
        }
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position, player.position - transform.position);
    }
    */
}
