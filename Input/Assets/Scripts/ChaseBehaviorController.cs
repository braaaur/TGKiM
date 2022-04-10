using UnityEngine;

public class ChaseBehaviorController : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 5f;

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
        CheckChase();
    }

    private void CheckChase()
    {
        if (enemyController == null)
        {
            //log error
            return;
        }

        enemyController.NavigatorController.MoveTo(enemyController.Player.position);
        enemyController.NavigatorController.MeshAgent.speed = chaseSpeed;
    }
}
