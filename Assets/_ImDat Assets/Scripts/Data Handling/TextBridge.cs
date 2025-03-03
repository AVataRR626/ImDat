using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

public class TextBridge : MonoBehaviour
{
    public TextAsset textAsset;
    public string lineSplit;
    public string colSplit;
    public string[] lines;
    public string[] fieldNames;

    public List<FieldValues> data = new List<FieldValues>();


    public void Start()
    {
        Debug.Log(textAsset.text);
        ReadData();
    }

    [ContextMenu("Read Data")]
    public void ReadData()
    {
        // "\n|\r|\r\n"
        lines = Regex.Split(textAsset.text, lineSplit);
        //lines = textAsset.text.Split('\n');

        //fieldNames = lines[0].Split('\t');
        //fieldNames = lines[0].Split(colSplit);
        fieldNames = Regex.Split(lines[0], colSplit);

        foreach(string field in fieldNames)
        {
            FieldValues fieldValues = new FieldValues();
            fieldValues.fieldName = field;
            data.Add(fieldValues);
        }

        for(int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Length > 1)
            {
                string[] values = Regex.Split(lines[i], colSplit);

                for (int j = 0; j < data.Count; j++)
                {
                    data[j].fieldValues.Add(values[j]);
                }
            }
        }
    }
}
