using UnityEngine;
using UnityEngine.AI;

public class NavigatorController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent meshAgent;

    [SerializeField] private Transform currentTarget;

    [SerializeField] private bool isClickToGo;

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

        if (isClickToGo)
        {
            CheckClickToGo();
        }
    }

    public void MoveTo(Vector3 givenPosition)
    {
        meshAgent.SetDestination(givenPosition);
    }

    private void CheckClickToGo()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Ray mousePositionRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            Ray mousePositionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(mousePositionRay, out hitInfo))
            {
                //Debug.Log("Hit! " + hitInfo.collider.gameObject.name);

                MoveTo(hitInfo.point);
            }
            else
            {
                //Debug.Log("Hit nothing!");
            }
        }
    }

    public NavMeshAgent MeshAgent
    {
        get
        {
            return meshAgent;
        }
    }
}
