using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidpointPositioner : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
	public bool alwaysUpOrientation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 newPos = (pointA.position + pointB.position) / 2;
        transform.position = newPos;

		if(alwaysUpOrientation)
		{
			transform.rotation = Quaternion.identity;
		}
	}
}
