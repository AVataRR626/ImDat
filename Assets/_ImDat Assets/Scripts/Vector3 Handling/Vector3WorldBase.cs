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

    [Header("Cloning Settings")]
    public Vector3WorldBase clonePrefab;
    public Transform clonePoint;
    public bool cloneAllow = false;
    public float spawnClock = 0;

    public void Start()
    {
        myHandle.transform.parent = null;
        if (value == null)
            value = GetComponent<Vector3Relay>();
    }

    public void FixedUpdate()
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

        if (referenceValue != null)
        {
            value.value = referenceValue.value;
        }


        if (spawnClock > 0)
        {
            spawnClock -= Time.fixedDeltaTime;
        }
        else
        {
            spawnClock = 0;
        }

    }


    public void DetachHandle()
    {
        myHandle.SetActive(false);
        if (myRenderer != null)
        {
            myRenderer.enabled = false;
        }
    }

    public void SpawnClone()
    {
        if (spawnClock <= 0)
        { 
            Vector3WorldBase newClone = Instantiate(clonePrefab, clonePoint.position, Quaternion.identity);
            newClone.referenceValue = value;
            spawnClock = 0.5f;
        }
    }

    public void OnDisable()
    {
        if(myHandle != null)
            myHandle.gameObject.SetActive(false);

        //if(referenceValue != null)
            //referenceValue.gameObject.SetActive(false);
    }
}
