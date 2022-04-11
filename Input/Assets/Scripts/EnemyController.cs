using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Transform[] waypoints;

    [SerializeField] private NavigatorController navigatorController;

    [SerializeField] private Transform player;

    #region Public Accessors
    public Transform Player
    {
        get
        {
            return player;
        }
    }

    public MeshRenderer MeshRenderer
    {
        get
        {
            return meshRenderer;
        }
    }

    public Transform[] Waypoints
    {
        get
        {
            return waypoints;
        }
    }

    public NavigatorController NavigatorController
    {
        get
        {
            return navigatorController;
        }
    }
    #endregion
}
