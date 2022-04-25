
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public LayerMask mask;

    [SerializeField] private NavigatorController navigatorController;

    private Transform _tranfrom;

    private MeshRenderer meshRenderer;

    //public UnityEvent room2Entered;

    //public UnityEvent room2Stay;

    //private float room2Time = 0f;

    private RaycastHit[] sphereHits;
    //Vector3 direction;

    public UnityEvent room1Entered;
    public UnityEvent room2Entered;

    private Vector3 position;
    [SerializeField] private float speed = 3f;

    [SerializeField] private float dobuleClickTimeWindow = 0.5f;
    private float currentDobuleClickTime;

    private bool isDead;

    private void OnEnable()
    {
        _tranfrom = transform;

        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        //Move();
        //MyCharacterUpdate();

        CheckClickToGo();
        CheckReload();
    }

    private void CheckClickToGo()
    {
        if (isDead)
        {
            return;
        }
        
        currentDobuleClickTime = Mathf.Clamp01(currentDobuleClickTime + Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            if (currentDobuleClickTime <= dobuleClickTimeWindow)
            {
                Click(true);
            }
            else
            {
                Click(false);
            }
            
            currentDobuleClickTime = 0f;
        }
    }

    private void Click(bool isDouble)
    {
        Ray mousePositionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(mousePositionRay, out hitInfo))
        {
            //Debug.Log("Hit! " + hitInfo.collider.gameObject.name);

            navigatorController.MoveTo(hitInfo.point, isDouble);
        }
        else
        {
            //Debug.Log("Hit nothing!");
        }
    }

    private void CheckReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            navigatorController.Animator.SetTrigger("callReloading");
        }
    }

    private void Move()
    {
        position = transform.position;

        position.x += Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        position.z += Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            isDead = true;
            navigatorController.Animator.SetBool("isDead", true);
        }
    }

    private void MyCharacterUpdate()
    {
        sphereHits = Physics.SphereCastAll(transform.position, 10f, Vector3.up, Mathf.Infinity, mask, QueryTriggerInteraction.Collide);

        if (sphereHits.Length > 0)
        {
            for (int i = 0; i < sphereHits.Length; i++)
            {
                if (Physics.Raycast(transform.position, sphereHits[i].collider.transform.position - transform.position, out RaycastHit hitInfo))
                {
                    if (hitInfo.collider == sphereHits[i].collider)
                    {
                        //meshRenderer.material.color = Color.red;
                        return;
                    }
                }
            }
        }

        //meshRenderer.material.color = Color.green;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (sphereHits != null)
        {
            for (int i = 0; i < sphereHits.Length; i++)
            {
                Gizmos.DrawRay(transform.position, sphereHits[i].collider.transform.position - transform.position);
            }
        }
    }
}
