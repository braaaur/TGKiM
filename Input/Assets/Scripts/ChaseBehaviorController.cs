using UnityEngine;

public class ChaseBehaviorController : MonoBehaviour
{
    [Header("Settings")]
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

    void Update()
    {
        if (enemyController == null)
        {
            Debug.LogError("enemyController == null in Update() in SearchBehaviorController.cs");
            return;
        }
        
        CheckChase();
    }

    private void CheckChase()
    {
        enemyController.NavigatorController.MoveTo(enemyController.Player.position, true);
        enemyController.NavigatorController.MeshAgent.speed = chaseSpeed;
    }
}
