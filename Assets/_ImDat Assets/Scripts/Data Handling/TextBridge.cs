using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using System.Linq;

public class TextBridge : MonoBehaviour
{
    [Header("Text Read Settings")]
    public TextAsset textAsset;
    public string lineSplit;
    public string colSplit;

    [Header("Visualisation Generation")]
    public Transform dPointRoot;
    public DPoint dPointPrefab;
    public int dPointCount;
    public Vector3 postLoadScale;
    public Vector3 postLoadOffset;


    [Header("System Stuff (Usually Don't Touch)")]
    public string[] lines;
    public string[] fieldNames;
    public List<FieldValueList> data = new List<FieldValueList>();


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

        

        foreach (string field in fieldNames)
        {
            FieldValueList fieldValues = new FieldValueList();
            fieldValues.fieldName = field;
            data.Add(fieldValues);
        }

        dPointCount = 0;
        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Length > 1)
            {
                string[] values = Regex.Split(lines[i], colSplit);
                    

                for (int j = 0; j < data.Count; j++)
                {
                    data[j].fieldValues.Add(values[j]);
                }
                dPointCount++;
            }
        }
    }

    [ContextMenu("Generate Visuals")]
    public void GenerateVisuals()
    {
        for(int i = 0; i < dPointCount; i++)
        {
            DPoint newDpoint = Instantiate(dPointPrefab, dPointRoot);
            for(int j = 0; j < data.Count; j++)
            {                            
                newDpoint.MapValue(data[j].fieldName,
                    data[j].fieldValues[i]);
            }
        }

        dPointRoot.transform.localScale = postLoadScale;

        dPointRoot.transform.position = postLoadOffset;
    }
}
