using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vector3Relay : MonoBehaviour
{
    public Vector3 value;
    public TextMeshProUGUI display;
    public float displayScale = 1;

    [Header("System Stuff - Usually Don't Touch")]
    Vector3 displayValue;

    private void Update()
    {
        if(display != null)
        {
            displayValue = value * displayScale;
            display.text = displayValue.ToString();
        }
    }

}
