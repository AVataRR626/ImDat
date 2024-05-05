using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unparent : MonoBehaviour
{
    public bool setGlobalPosToLocal = true;

    Vector3 origLocalPos;

    // Start is called before the first frame update
    void Start()
    {
        origLocalPos = transform.localPosition;

        transform.parent = null;

        if (setGlobalPosToLocal)
            transform.position = origLocalPos;
    }

}
