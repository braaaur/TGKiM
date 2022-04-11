using UnityEngine;
using UnityEngine.Events;

public class ObserveBehaviorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float tresholdAngle = 30f; 
    private Vector3 currentDirection;
    private float currentAngle;

    private bool isPlayerVisible;

    private EnemyController enemyController;

    [SerializeField] private UnityEvent playerSpotted;
    [SerializeField] private UnityEvent playerLost;

    private void OnEnable()
    {
        enemyController = GetComponentInParent<EnemyController>();

        enemyController.MeshRenderer.material.color = Color.blue;
    }

    private void OnDisable()
    {
        enemyController = null;
    }

    void Update()
    {
        if (enemyController == null)
        {
            Debug.LogError("enemyController == null in Update() in ObserveBehaviorController.cs");
            return;
        }

        CheckSeePlayer();
    }

    private void CheckSeePlayer()
    {
        currentDirection = enemyController.Player.position - transform.position;

        if (Physics.Raycast(transform.position, currentDirection, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.transform == enemyController.Player)
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
            if (isPlayerVisible != value)
            {
                if (value)
                {
                    enemyController.MeshRenderer.material.color = Color.red;
                    playerSpotted.Invoke();
                }
                else
                {
                    enemyController.MeshRenderer.material.color = Color.blue;
                    playerLost.Invoke();
                }
            }

            isPlayerVisible = value;
        }
        get
        {
            return isPlayerVisible;
        }
    }

    private void OnDrawGizmos()
    {
        if (enemyController == null)
        {
            //Debug.LogError("enemyController == null in OnDrawGizmos() in ObserveBehaviorController.cs");
            return;
        }
        
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position, enemyController.Player.position - transform.position);
    }
}
