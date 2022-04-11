using UnityEngine;
using UnityEngine.Events;

public class WaitBehaviorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float waitingTime = 5f;
    private float currentTime;
    
    private EnemyController enemyController;

    [SerializeField] private UnityEvent waitFinished;

    private void OnEnable()
    {
        enemyController = GetComponentInParent<EnemyController>();

        currentTime = 0f;
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

        Wait();
    }

    private void Wait()
    {
        currentTime = Mathf.Clamp(currentTime + Time.deltaTime, 0f, waitingTime);

        if (currentTime >= waitingTime)
        {
            waitFinished.Invoke();
        }
    }
}
