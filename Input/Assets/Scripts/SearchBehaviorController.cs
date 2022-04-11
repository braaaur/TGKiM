using UnityEngine;
using UnityEngine.Events;

public class SearchBehaviorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float normalSpeed = 3f;

    private EnemyController enemyController;

    [SerializeField] private UnityEvent targetReached;

    private void OnEnable()
    {
        enemyController = GetComponentInParent<EnemyController>();

        MoveToNextWaypoint();
    }

    private void OnDisable()
    {
        enemyController = null;
    }

    void Update()
    {
        if (enemyController == null)
        {
            Debug.LogError("enemyController == null in Update() in SearchBehaviorController.cs");
            return;
        }

        CheckDistance();
    }

    private void CheckDistance()
    {
        enemyController.NavigatorController.MeshAgent.speed = normalSpeed;

        if (enemyController.NavigatorController.MeshAgent.remainingDistance <= enemyController.NavigatorController.MeshAgent.stoppingDistance)
        {
            TargetReached();
        }
    }

    private void TargetReached()
    {
        targetReached.Invoke();
        //enemyController.NavigatorController.MoveTo(enemyController.Waypoints[Random.Range(0, enemyController.Waypoints.Length)].position);
    }

    private void MoveToNextWaypoint()
    {
        enemyController.NavigatorController.MoveTo(enemyController.Waypoints[Random.Range(0, enemyController.Waypoints.Length)].position);
    }
}
