using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnapper : MonoBehaviour
{
    public Vector3 gridSize = new Vector3(0.01f,0.01f,0.01f);
    public Vector3 gridPos;
    public float snapSpeed = 1;
    public bool autoSnap = false;
    public float forceSnapDistance = 0.01f;

    [Header("Debug")]
    public Vector3 floatFactor;
    public Vector3 intFactor;
    public Rigidbody myRbody;

    // Start is called before the first frame update
    void Start()
    {
        myRbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(autoSnap)
            SmoothSnap();
    }

    public void SmoothSnap()
    {
        GetGridPos();
        Float2SnapPos();
    }

    public void GetGridPos()
    {
        floatFactor.x = transform.position.x / gridSize.x;
        floatFactor.y = transform.position.y / gridSize.y;
        floatFactor.z = transform.position.z / gridSize.z;

        intFactor.x = Mathf.Round(floatFactor.x);
        intFactor.y = Mathf.Round(floatFactor.y);
        intFactor.z = Mathf.Round(floatFactor.z);

        gridPos.x = intFactor.x * gridSize.x;
        gridPos.y = intFactor.y * gridSize.y;
        gridPos.z = intFactor.z * gridSize.z;
    }
    
    public void Float2SnapPos()
    {
        if(Vector3.Distance(transform.position,gridPos) <= forceSnapDistance)
            Snap2Grid();
        else
            transform.position = Vector3.Lerp(transform.position, gridPos, snapSpeed * Time.deltaTime);
    }

    public void Snap2Grid()
    {
        Debug.Log("--- Snap2Grid --- ");

        GetGridPos();
        transform.position = gridPos;

        if (myRbody != null)
        {
            myRbody.linearVelocity = Vector3.zero;
        }
        
    }
}
