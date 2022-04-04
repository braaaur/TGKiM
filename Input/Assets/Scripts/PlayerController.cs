
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public LayerMask mask;


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
    public float speed = 3f;

    private void OnEnable()
    {
        _tranfrom = transform;

        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        //Move();
        MyCharacterUpdate();
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
        if (other.gameObject.name == "Room1")
        {
            Debug.Log("Room1 entered");
            room1Entered.Invoke();
        }
        else if (other.gameObject.name == "Room2")
        {
            Debug.Log("Room2 entered");
            room2Entered.Invoke();
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
                        meshRenderer.material.color = Color.red;
                        return;
                    }
                }
            }
        }

        meshRenderer.material.color = Color.green;
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
