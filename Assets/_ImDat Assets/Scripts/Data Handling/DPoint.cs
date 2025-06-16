/*
 * ImDat - Immersive Data Visualisation 
 *  
 * The atomic data "pixel!" Da-El?
 * 
 * - Matt Cabanag, 24 Apr 2024
 *  
 */

using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPoint : MonoBehaviour
{
    public Vector3 rotationSpeed;
    public Vector3 oscilationSpeed;
    public Color primaryColour;
    public GameObject primaryGraphics;
    public TextMeshProUGUI textDisplayTMPG;

    public List<ValueMap> valueMap = new List<ValueMap>();
    

    [Header("System Stuff [Dont Touch! (usually)]")]
    public string albedoKey;
    public Renderer primaryRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if(primaryGraphics != null)
            primaryRenderer = primaryGraphics.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Instanced shader value changing
        //myRenderer.sharedMaterial.SetFloat(highlightKey, highlightValue);
        if(primaryRenderer != null)
            primaryRenderer.material.SetColor(albedoKey, primaryColour);

    }

    public void MapValue(string fieldName, string rawString)
    {
        Debug.Log("fieldName: " + fieldName);
        float floatConversion = float.Parse(rawString);
        MapValue(fieldName, floatConversion);
    }

    public void MapValue(string fieldName, float value)
    {
        foreach (ValueMap mapping in valueMap)
        {
            Debug.Log("CheckMapping: " + mapping.fieldName);
            if (mapping.fieldName.Contains(fieldName) || 
                fieldName.Contains(mapping.fieldName))
            {
                Debug.Log("Mapping Found: " + fieldName);
                MapAttribute(mapping.mappedAttribute, value);
                break;
            }
        }
    }

    public void MapAttribute(string mappedAttribute, float value)
    {
        Debug.Log("Mapped Attribute: " + mappedAttribute);

        if(mappedAttribute.Equals("p.x"))
        {
            Debug.Log("Mapping p.x");
            Vector3 pos = transform.position;
            pos.x = value;
            transform.localPosition = pos;
        }

        if (mappedAttribute.Equals("p.y"))
        {
            Debug.Log("Mapping p.y");
            Vector3 pos = transform.position;
            pos.y = value;
            transform.localPosition = pos;
        }

        if (mappedAttribute.Equals("p.z"))
        {
            Debug.Log("Mapping p.z: " + value);
            Vector3 pos = transform.position;
            pos.z = value;
            transform.localPosition = pos;
        }
    }
}
