using UnityEngine;
using UnityEngine.Events;

public class ObservationBehaviorController : MonoBehaviour
{
    private bool isPlayerVisible;
    private Vector3 currentDirection;
    private float currentAngle;

    [SerializeField] private float tresholdAngle = 30f;

    private EnemyController enemyController;

    public UnityEvent playerSpotted;
    public UnityEvent playerLost;

    private void OnEnable()
    {
        enemyController = GetComponentInParent<EnemyController>();

        enemyController.MeshRenderer.material.color = Color.blue;
    }

    private void OnDisable()
    {
        enemyController = null;
    }

    private void Update()
    {
        CheckSeePlayer();
    }

    private void CheckSeePlayer()
    {
        if (enemyController == null)
        {
            //log error
            return;
        }
        
        currentDirection = enemyController.Player.position - transform.position;

        if (Physics.Raycast(transform.position, currentDirection, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.transform == enemyController.Player)
            {
                currentAngle = Vector3.Angle(transform.forward, currentDirection);

                if (currentAngle <= tresholdAngle)
                {
                    SetIsPlayerVisible(true);

                    return;
                }
            }
        }

        SetIsPlayerVisible(false);
    }

    private void SetIsPlayerVisible(bool value)
    {
        if (isPlayerVisible != value)
        {
            isPlayerVisible = value;

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
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position, player.position - transform.position);
    }
    */
}
