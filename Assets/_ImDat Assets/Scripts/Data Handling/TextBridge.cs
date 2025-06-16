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
    public Vector3 postLoadScale = Vector3.one;
    public Vector3 postLoadOffset;


    [Header("System Stuff (Usually Don't Touch)")]
    public string[] rawLines;
    public string[] fieldNames;
    public List<string> cleanedLines;
    public List<FieldValueList> dataPerField = new List<FieldValueList>();


    public void Start()
    {
        Debug.Log(textAsset.text);        
        ReadData();
    }

    [ContextMenu("Read Data")]
    public void ReadData()
    {
        // "\n|\r|\r\n"
        rawLines = Regex.Split(textAsset.text, lineSplit);
        //lines = textAsset.text.Split('\n');

        //fieldNames = lines[0].Split('\t');
        //fieldNames = lines[0].Split(colSplit);

        if (rawLines.Length > 0)
        {
            cleanedLines = new List<string>();

            foreach (string line in rawLines)
            {
                if (!line.Equals(" ") && line.Length > 0)
                {
                    Debug.Log("z!" + line + "!z");
                    cleanedLines.Add(line);
                }
                
            }
        }

        if (colSplit.Length > 0)
        {
            fieldNames = Regex.Split(rawLines[0], colSplit);

            foreach (string field in fieldNames)
            {
                FieldValueList fieldValues = new FieldValueList();
                fieldValues.fieldName = field;
                dataPerField.Add(fieldValues);
            }
        }
        else if(colSplit.Length == 0)//can this ever be < 0? paranoid :p
        {
            Debug.Log("No column delimiter: Going into PARAGRAPH MODE.");
            FieldValueList oneField = new FieldValueList();
            oneField.fieldName = "paragraph";
            dataPerField.Add(oneField);
        }

        dPointCount = 0;
        for (int i = 1; i < cleanedLines.Count; i++)
        {
            if (cleanedLines[i].Length > 1)
            {
                string[] values = Regex.Split(cleanedLines[i], colSplit);

                //regular mode
                if (colSplit.Length > 0)
                {
                    if (values.Length == dataPerField.Count)
                    {
                        for (int j = 0; j < dataPerField.Count; j++)
                        {
                            dataPerField[j].fieldValues.Add(values[j]);
                        }
                    }
                    else
                    {
                        Debug.LogError("!!! Number of field values != heading definition. Line: " + i);
                    }
                }
                else
                {
                    Debug.Log("PARAGRAPH MODE - adding data to paragraphs");
                    dataPerField[0].fieldValues.Add(cleanedLines[i]);
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

            if (dataPerField.Count > 0)
            {
                for (int j = 0; j < dataPerField.Count; j++)
                {
                    newDpoint.MapValue(dataPerField[j].fieldName,
                        dataPerField[j].fieldValues[i]);
                }
            }
            else
            {

            }
        }

        dPointRoot.transform.localScale = postLoadScale;
        dPointRoot.transform.position = postLoadOffset;
    }
}
