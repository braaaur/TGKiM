using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointController : MonoBehaviour
{
    private ConfigurableJoint joint;

    private void OnEnable()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    void Update()
    {
        
    }
}
