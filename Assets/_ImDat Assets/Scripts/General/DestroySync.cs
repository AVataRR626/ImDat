using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySync : MonoBehaviour
{
    public GameObject[] syncObjects;

    public void OnDestroy()
    {
        foreach (GameObject o in syncObjects)
            if (o != null)
                o.SetActive(false);
    }
}
