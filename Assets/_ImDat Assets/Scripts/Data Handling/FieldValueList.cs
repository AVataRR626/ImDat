using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FieldValueList
{
    public string fieldName;
    public List<string> fieldValues;

    public FieldValueList()
    {
        fieldValues = new List<string>();
    }
}
