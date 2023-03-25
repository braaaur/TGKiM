using UnityEngine;
using System;

public enum Orientation { motionX, motionZ };

public class LabController : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint joint;
    [SerializeField] private Path[] paths;

    private static readonly float tolerance = 0.05f;

    //private bool allowXMotion;
    //private bool allowZMotion;

    private void Update()
    {
        UpdatePaths();
    }

    private void UpdatePaths()
    {
        //Debug.Log(joint.connectedAnchor.ToString());
        
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        foreach (Path path in paths)
        {
            if (path.orientation == Orientation.motionX)
            {
                if (Mathf.Abs(joint.transform.localPosition.z - path.pathTransform.localPosition.z) <= tolerance)
                {
                    joint.xMotion = ConfigurableJointMotion.Limited;
                    joint.connectedAnchor = new Vector3(path.pathTransform.position.x, joint.connectedAnchor.y, path.pathTransform.position.z);
                }
            }
            else if (path.orientation == Orientation.motionZ)
            {
                if (Mathf.Abs(joint.transform.localPosition.x - path.pathTransform.localPosition.x) <= tolerance)
                {
                    joint.zMotion = ConfigurableJointMotion.Limited;
                    joint.connectedAnchor = new Vector3(path.pathTransform.position.x, joint.connectedAnchor.y, path.pathTransform.position.z);
                }
            }
        }
    }
}

[Serializable]
public class Path
{
    public Transform pathTransform;
    public Orientation orientation;
}
