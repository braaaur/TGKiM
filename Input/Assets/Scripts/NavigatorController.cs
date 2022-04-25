using UnityEngine;
using UnityEngine.AI;

public class NavigatorController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent meshAgent;

    [SerializeField] private Transform currentTarget;

    [SerializeField] private bool isClickToGo;

    [SerializeField] private Animator animator;
    private Vector3 currentVelocity;
    private Vector3 currentVelocityLocal;
    private float speedRatio;

    [SerializeField] private float animDirMuliplier = 0.2f;

    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float doubleSpeed = 6f;
    /*
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    */

    private void Update()
    {
        /*
        if (currentTarget != null)
        {
            MoveTo(currentTarget.position);
        }
        */

        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        if (animator == null)
        {
            return;
        }

        currentVelocity = meshAgent.velocity;
        currentVelocityLocal = transform.InverseTransformVector(currentVelocity);

        //

        //animator.SetFloat("leftRight", currentVelocityLocal.x);
        //animator.SetFloat("backwardForward", currentVelocityLocal.z);
        animator.SetFloat("leftRight", currentVelocityLocal.x * animDirMuliplier);
        animator.SetFloat("backwardForward", currentVelocityLocal.z * animDirMuliplier);

        speedRatio = Mathf.Clamp01((currentVelocity.magnitude - normalSpeed) / (doubleSpeed - normalSpeed));

        animator.SetFloat("speed", speedRatio);
    }

    public void MoveTo(Vector3 givenPosition, bool isDouble)
    {
        meshAgent.SetDestination(givenPosition);

        if (isDouble)
        {
            meshAgent.speed = doubleSpeed;
        }
        else
        {
            meshAgent.speed = normalSpeed;
        }
    }
   

    public NavMeshAgent MeshAgent
    {
        get
        {
            return meshAgent;
        }
    }

    public Animator Animator
    {
        get
        {
            return animator;
        }
    }
}
