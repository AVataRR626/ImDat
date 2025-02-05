using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Vector3RelaySetterPosDelta : MonoBehaviour, IRelayReferencePoint
{
    public Vector3Relay output;
    public Transform anchorPoint;
    public Transform referencePoint;

    // Start is called before the first frame update
    void Start()
    {
        if(output == null)
        {
            output = GetComponent<Vector3Relay>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(anchorPoint != null)
        {
            if(referencePoint != null)
            {
                output.value = (referencePoint.position - anchorPoint.position) / anchorPoint.localScale.x;
            }
        }
    }

    public Transform GetReferencePoint()
    {
        return referencePoint;
    }
}

public interface IRelayReferencePoint
{   public Transform GetReferencePoint();
}
