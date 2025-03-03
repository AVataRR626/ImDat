using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FieldValues
{
    public string fieldName;
    public List<string> fieldValues;

    public FieldValues()
    {
        fieldValues = new List<string>();
    }
}
