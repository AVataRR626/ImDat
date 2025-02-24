using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSync : MonoBehaviour
{
    public List<GameObject> syncObjects;

    public void OnDisable()
    {
        foreach (GameObject o in syncObjects)
            if(o != null)
                o.SetActive(false);
    }
}
