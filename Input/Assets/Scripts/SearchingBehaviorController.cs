using UnityEngine;

public class SearchingBehaviorController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float searchingSpeed = 3f;

    private EnemyController enemyController;

    private void OnEnable()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    private void OnDisable()
    {
        enemyController = null;
    }

    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (enemyController == null)
        {
            //log error
            return;
        }
        
        enemyController.NavigatorController.MeshAgent.speed = searchingSpeed;

        if (enemyController.NavigatorController.MeshAgent.remainingDistance <= enemyController.NavigatorController.MeshAgent.stoppingDistance)
        {
            TargetReached();
        }
    }

    private void TargetReached()
    {
        enemyController.NavigatorController.MoveTo(waypoints[Random.Range(0, waypoints.Length)].position);
    }
}
