using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{

    public Transform subject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(subject != null)
        {
            transform.position = subject.position;
            transform.rotation = subject.rotation;
        }
    }
}
