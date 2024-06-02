using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3WorldBase : MonoBehaviour
{
    public GameObject myHandle;
    public bool trackRotation = false;
    public Vector3Relay value;
    public Vector3Relay referenceValue;
    public Renderer myRenderer;

    public void Start()
    {
        myHandle.transform.parent = null;
        if (value == null)
            value = GetComponent<Vector3Relay>();
    }

    public void Update()
    {
        if (myHandle != null)
        {
            if (myHandle.activeSelf)
            {
                transform.position = myHandle.transform.position;

                if (trackRotation)
                    transform.rotation = myHandle.transform.rotation;
            }
        }

        if(referenceValue != null)
        {
            value.value = referenceValue.value;
        }

    }


    public void DetachHandle()
    {
        myHandle.SetActive(false);
        if(myRenderer != null)
        {
            myRenderer.enabled = false;
        }
    }
}
