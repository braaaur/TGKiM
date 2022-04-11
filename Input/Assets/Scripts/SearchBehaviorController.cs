using UnityEngine;

public class SearchBehaviorController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float normalSpeed = 3f;

    private EnemyController enemyController;

    private void OnEnable()
    {
        enemyController = GetComponentInParent<EnemyController>();
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
        enemyController.NavigatorController.MoveTo(enemyController.Waypoints[Random.Range(0, enemyController.Waypoints.Length)].position);
    }
}
