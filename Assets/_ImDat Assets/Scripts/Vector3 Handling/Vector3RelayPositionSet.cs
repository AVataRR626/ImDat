using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3RelayPositionSet : MonoBehaviour
{
    public Vector3Relay referenceValue;
    public Transform anchorPoint;
    public Transform referencePoint;    

    // Update is called once per frame
    void Update()
    {
        
        transform.rotation = Quaternion.identity;

        if(referenceValue != null)
        {
            if(referencePoint != null)
            {
                referencePoint.localPosition = referenceValue.value;
            }
        }
    }
}
