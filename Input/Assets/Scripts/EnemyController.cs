using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private NavigatorController navigatorController;

    #region Accessors
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

    public NavigatorController NavigatorController
    {
        get
        {
            return navigatorController;
        }
    }
    #endregion
}
